using BarCodeAPIService.Connection;
using BarCodeAPIService.Models;
using BarCodeLibrary.Request.SAP;
using BarCodeLibrary.Respones.SAP;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Threading.Tasks;
using BarCodeLibrary.Contract.RouteProcedure;
using BarCodeLibrary.Respones.SAP.Tengkimleang;
using BarCodeLibrary.Respones.SAP.Vichika;
using SAPbobsCOM;

namespace BarCodeAPIService.Service
{
    public class SalesOrderForIMService : ISalesOrderForIMService
    {
        private int ErrCode;
        private string ErrMsg;

        public Task<ResponseSalesOrder> PostSalesOrder(SendSalesOrderForIM sendSalesOrderForIM)
        {
            try
            {
                Documents oSalesOrderDocuments;
                Company oCompany;
                var Retval = 0;
                Login login = new();
                if (login.LErrCode == 0)
                {
                    oCompany = login.Company;
                    oSalesOrderDocuments = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oOrders);
                    oSalesOrderDocuments.CardCode = sendSalesOrderForIM.CardCode;
                    oSalesOrderDocuments.DocDate = sendSalesOrderForIM.DocDate;
                    oSalesOrderDocuments.DocDueDate = sendSalesOrderForIM.DocDueDate;
                    oSalesOrderDocuments.TaxDate = sendSalesOrderForIM.TaxDate;
                    oSalesOrderDocuments.Series = Convert.ToInt32(sendSalesOrderForIM.SeriesCode);
                    //oSalesOrderDocuments.DocCurrency = sendSalesOrderForIM.CurrencyCode;
                    oSalesOrderDocuments.Comments = (sendSalesOrderForIM.Remark == null) ? "" : sendSalesOrderForIM.Remark;
                    oSalesOrderDocuments.SalesPersonCode = Convert.ToInt32(sendSalesOrderForIM.SlpCode);

                    string strWebID = "";
                    string strInvRemark = "";
                    string strInvRemarkCut = "";
                    var date = DateTime.Now;
                    strWebID = "SO" + oSalesOrderDocuments.Series + "-" + date.Day + "" + date.Month + "" + date.Year + "-" + date.Hour + "" + date.Minute + "" + date.Second;
                    oSalesOrderDocuments.UserFields.Fields.Item("U_WebID").Value = strWebID;

                    oSalesOrderDocuments.UserFields.Fields.Item("U_loannum").Value = sendSalesOrderForIM.TranferNo;
                    oSalesOrderDocuments.UserFields.Fields.Item("U_order").Value = "IM" + sendSalesOrderForIM.TranferNo;

                    //หมายเหตุในใบกำกับ                    
                    strInvRemark = sendSalesOrderForIM.InvRemark;
                    strInvRemarkCut = strInvRemark.Remove(strInvRemark.Length - 2, 1);
                    oSalesOrderDocuments.UserFields.Fields.Item("U_inv_remark").Value = strInvRemarkCut;

                    string strSqRemark = "";
                    if (sendSalesOrderForIM.SqRemark != null || sendSalesOrderForIM.SqRemark != "")
                    {
                        strSqRemark = sendSalesOrderForIM.SqRemark;
                    }
                    else
                    {
                        strSqRemark = "";
                    }
                    oSalesOrderDocuments.UserFields.Fields.Item("U_sq_remark").Value = (sendSalesOrderForIM.SqRemark == null) ? "" : sendSalesOrderForIM.SqRemark;
                    
                    string patient = "";
                    patient = sendSalesOrderForIM.Patient;

                    string strToBinLoc = "";

                    int countLine = 0;
                    foreach (var x in sendSalesOrderForIM.Lines)
                    {
                        countLine++;
                    }

                    int nLine = 0;
                    foreach (var l in sendSalesOrderForIM.Lines)
                    {
                        nLine = nLine + 1;
                        strToBinLoc = "";

                        oSalesOrderDocuments.Lines.ItemCode = l.ItemCode;
                        oSalesOrderDocuments.Lines.Quantity = l.Quantity;
                        oSalesOrderDocuments.Lines.Price = l.Price;
                        oSalesOrderDocuments.Lines.WarehouseCode = l.WhsCode;

                        int binEntry = l.BinEntry;
                        strToBinLoc = l.BinCode;

                        string strTranferNo = "";
                        string xTranferNo = "";
                        strTranferNo = l.U_TranferNo;
                        if (strTranferNo is not null)
                        {
                            xTranferNo = strTranferNo;
                        }
                        else
                        {
                            xTranferNo = " ";
                        }
                        oSalesOrderDocuments.Lines.UserFields.Fields.Item("U_TranferNo").Value = xTranferNo;

                        string strPatient = "";
                        string xPatient = "";
                        strPatient = l.U_Patient;
                        if (strPatient is not null)
                        {
                            xPatient = strPatient;
                        }
                        else
                        {
                            xPatient = " ";
                        }
                        oSalesOrderDocuments.Lines.UserFields.Fields.Item("U_Patient").Value = xPatient;
                        oSalesOrderDocuments.Lines.UserFields.Fields.Item("U_BalanceQty").Value = "0";
                        oSalesOrderDocuments.Lines.Add();
                    }

                    //oSalesOrderDocuments.UserFields.Fields.Item("U_ToBinLoc").Value = strToBinLoc;

                    Retval = oSalesOrderDocuments.Add();
                    Retval = 0;
                    if (Retval != 0)
                    {
                        oCompany.GetLastError(out ErrCode, out ErrMsg);
                        return Task.FromResult(new ResponseSalesOrder
                        {
                            ErrorCode = ErrCode,
                            ErrorMsg = ErrMsg,
                            DocEntry = null
                        });
                    }
                    else
                    {

                        string DocNumIM = "";
                        DocNumIM = sendSalesOrderForIM.TranferNo;
                        int cLine = 0;
                        cLine = countLine - 1;
                        if (DocNumIM != "")
                        {
                            //AddPatientSO
                            oCompany = login.Company;
                            SAPbobsCOM.Recordset oRS = null;
                            oRS = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            string SQL1 = "CALL \"" + ConnectionString.CompanyDB + "\"._USP_CALLTRANS_BANK('AddPatientSO','"+ DocNumIM + "','"+ patient + "',"+ cLine + ",'','')";
                            oRS.DoQuery(SQL1);

                            //Insert Line BatchNumber/SerialNumber
                            foreach (var l in sendSalesOrderForIM.LinesX)
                            {
                                var BatchNumber = "";
                                var SerialNumber = "";
                                if (l.Batches != null) {
                                    foreach (var batch in l.Batches)
                                    {
                                        BatchNumber = batch.BatchNumber;
                                    }
                                }

                                if(l.Serial != null)
                                {
                                    foreach (var serial in l.Serial)
                                    {
                                        SerialNumber = serial.SerialNumber;
                                    }
                                }

                                string INS = "INSERT INTO \"" + ConnectionString.BarcodeDb + "\".TBSOBATCHSERIAL ";
                                INS = INS + "values(";
                                INS = INS + "(select top 1 ID from \"" + ConnectionString.BarcodeDb + "\".TBSOBATCHSERIAL order by ID desc) + 1"; //ID
                                INS = INS + ", 0"; //DocEntry
                                INS = INS + ", '" + sendSalesOrderForIM.TranferNo + "'"; //DocNum
                                INS = INS + ", " + l.LineNum + ""; //LineNum
                                INS = INS + ", '"+ sendSalesOrderForIM.CardCode + "'"; //CardCode
                                INS = INS + ", ''"; //CardName
                                INS = INS + ", '" + l.ItemCode + "'"; //ItemCode
                                INS = INS + ", ''"; //Dscription
                                INS = INS + ", '" + l.ManageItem + "'"; //BatchSerialType
                                INS = INS + ", '" + BatchNumber + "'"; //BatchNumber
                                INS = INS + ", '" + SerialNumber + "'"; //SerialNumber
                                INS = INS + ", " + l.Quantity + ""; //Quantity
                                INS = INS + ", '" + l.UomName + "'"; //UomCode
                                INS = INS + ", ''"; //TaxCode
                                INS = INS + ", NULL"; //UnitPrice
                                INS = INS + ", " + l.Price + ""; //GrossPrice
                                INS = INS + ", '" + l.WhsCode + "'"; //Warehouse
                                INS = INS + ", '" + l.BinCode + "'"; //BinLocation
                                INS = INS + ", '" + l.U_Patient + "'"; //Patient
                                INS = INS + ", '" + strWebID + "'"; //WebDocNum
                                INS = INS + ", " + l.BaseLine + ""; //BaseLine
                                INS = INS + ", " + l.BinEntry + ""; //BinEntry
                                INS = INS + ")";
                                oRS.DoQuery(INS);
                            }
                            
                        }

                        return Task.FromResult(new ResponseSalesOrder
                        {
                            ErrorCode = 0,
                            ErrorMsg = "",
                            DocEntry = oCompany.GetNewObjectKey()
                        });
                    }
                    
                }

                return Task.FromResult(new ResponseSalesOrder
                {
                    ErrorCode = login.LErrCode,
                    ErrorMsg = login.SErrMsg
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResponseSalesOrder
                {
                    ErrorCode = ex.HResult,
                    ErrorMsg = ex.Message
                });
            }

        }
    }
}
