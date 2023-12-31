﻿using BarCodeAPIService.Connection;
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
                    var date = DateTime.Now;
                    strWebID = oSalesOrderDocuments.Series + "-" + date.Day + "" + date.Month + "" + date.Year + "-" + date.Hour + "" + date.Minute + "" + date.Second;
                    oSalesOrderDocuments.UserFields.Fields.Item("U_WebID").Value = strWebID;

                    oSalesOrderDocuments.UserFields.Fields.Item("U_loannum").Value = sendSalesOrderForIM.TranferNo;
                    oSalesOrderDocuments.UserFields.Fields.Item("U_order").Value = "IM" + sendSalesOrderForIM.TranferNo;
                    oSalesOrderDocuments.UserFields.Fields.Item("U_inv_remark").Value = "IM" + sendSalesOrderForIM.TranferNo;

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

                        //if (nLine == countLine)
                        //{
                        //    oSalesOrderDocuments.Lines.UserFields.Fields.Item("U_ItemCodeDes").Value = " ";
                        //}

                        //if (l.ManageItem == "S")
                        //{
                        //    int nS = 0;
                        //    foreach (var serial in l.Serial)
                        //    {
                        //        oSalesOrderDocuments.Lines.SerialNumbers.Quantity = 1;
                        //        oSalesOrderDocuments.Lines.SerialNumbers.InternalSerialNumber = serial.SerialNumber;
                        //        oSalesOrderDocuments.Lines.SerialNumbers.ManufacturerSerialNumber = serial.SerialNumber;
                        //        oSalesOrderDocuments.Lines.SerialNumbers.Add();

                        //        oSalesOrderDocuments.Lines.BinAllocations.SerialAndBatchNumbersBaseLine = nS;
                        //        oSalesOrderDocuments.Lines.BinAllocations.BinAbsEntry = binEntry;
                        //        oSalesOrderDocuments.Lines.BinAllocations.Quantity = 1;
                        //        oSalesOrderDocuments.Lines.BinAllocations.Add();

                        //        nS++;
                        //    }
                        //}
                        //else if (l.ManageItem == "B")
                        //{
                        //    int nB = 0;
                        //    foreach (var batch in l.Batches)
                        //    {
                        //        oSalesOrderDocuments.Lines.BatchNumbers.ExpiryDate = Convert.ToDateTime(batch.ExpDate);
                        //        oSalesOrderDocuments.Lines.BatchNumbers.Quantity = batch.Quantity;
                        //        oSalesOrderDocuments.Lines.BatchNumbers.BatchNumber = batch.BatchNumber;
                        //        oSalesOrderDocuments.Lines.BatchNumbers.Add();

                        //        oSalesOrderDocuments.Lines.BinAllocations.SerialAndBatchNumbersBaseLine = nB;
                        //        oSalesOrderDocuments.Lines.BinAllocations.BinAbsEntry = binEntry;
                        //        oSalesOrderDocuments.Lines.BinAllocations.Quantity = batch.Quantity;
                        //        oSalesOrderDocuments.Lines.BinAllocations.Add();
                        //        nB++;
                        //    }
                        //}
                        oSalesOrderDocuments.Lines.Add();
                    }

                    oSalesOrderDocuments.UserFields.Fields.Item("U_ToBinLoc").Value = strToBinLoc;

                    Retval = oSalesOrderDocuments.Add();
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
