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
    public class DeliveryService : IDeliveryService
    {
        private int ErrCode;
        private string ErrMsg;

        public Task<ResponseDelivery> PostDelivery(SendDelivery sendDelivery)
        {
            try
            {
                Documents oDeliveryDocuments;
                Company oCompany;
                var Retval = 0;
                Login login = new();
                if (login.LErrCode == 0)
                {
                    oCompany = login.Company;
                    double SumHTotal = 0;
                    double SumBefTotal = 0;
                    oDeliveryDocuments = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oDeliveryNotes);
                    oDeliveryDocuments.CardCode = sendDelivery.CardCode;
                    oDeliveryDocuments.DocDate = sendDelivery.DocDate;
                    oDeliveryDocuments.DocDueDate = sendDelivery.DocDate;
                    oDeliveryDocuments.Series = Convert.ToInt32(sendDelivery.Series);
                    oDeliveryDocuments.DocCurrency = sendDelivery.CurrencyCode;
                    oDeliveryDocuments.Comments = (sendDelivery.Remark == null) ? "" : sendDelivery.Remark;
                    oDeliveryDocuments.SalesPersonCode = Convert.ToInt32(sendDelivery.SlpCode);
                    oDeliveryDocuments.DiscountPercent = sendDelivery.DiscountPercent;

                    oDeliveryDocuments.UserFields.Fields.Item("U_WebID").Value = "DE" + sendDelivery.Series + DateTime.Today.Day
                        + DateTime.Today.Month
                        + DateTime.Today.Year
                        + DateTime.Today.Hour
                        + "-"
                        + DateTime.Today.Minute
                        + DateTime.Today.Second
                        + DateTime.Today.Millisecond;

                    string strSq_Remark = "";
                    if (sendDelivery.Sq_Remark is not null || sendDelivery.Sq_Remark != "")
                    {
                        strSq_Remark = sendDelivery.Sq_Remark;
                    }
                    else
                    {
                        strSq_Remark = "";
                    }
                    oDeliveryDocuments.UserFields.Fields.Item("U_sq_remark").Value = (sendDelivery.Sq_Remark == null) ? "" : sendDelivery.Sq_Remark;


                    foreach (var l in sendDelivery.Lines)
                    {
                        if (l.YesNo == "Yes")
                        {
                            // oDeliveryDocuments.Lines.UoMEntry = Convert.ToInt32(l.UomName);
                            if (l.DocEntry != null)
                            {
                                oDeliveryDocuments.Lines.BaseEntry = Convert.ToInt32(l.DocEntry);
                                oDeliveryDocuments.Lines.BaseType = 17;
                                oDeliveryDocuments.Lines.BaseLine = l.LineNum;

                                //Calc
                                double sVat;
                                string PriceBefore = "";
                                string PriceAfter = "";
                                if (l.TaxCode == "S07")
                                {
                                    sVat = (l.PriceBeforeDis * 0.07) + l.PriceBeforeDis;
                                }
                                else
                                {
                                    sVat = l.PriceBeforeDis;
                                }
                                PriceBefore = l.PriceBeforeDis.ToString("F"); //420.5607
                                PriceAfter = sVat.ToString("F"); //450
                                double calcLine;
                                string calcLineStr = "";

                                if (PriceAfter == "450.00")
                                {
                                    calcLine = System.Math.Round((l.PriceBeforeDis * l.Quantity), 2);
                                    calcLineStr = calcLine.ToString("F");
                                }
                                else
                                {
                                    //calcLine = l.PriceBeforeDis * l.Quantity;
                                    calcLine = System.Math.Round(l.PriceBeforeDis * l.Quantity, 2);
                                    calcLineStr = calcLine.ToString("F");
                                }
                                decimal DecimalVar = Convert.ToDecimal(calcLineStr);
                                //=======

                                oDeliveryDocuments.Lines.DiscountPercent = l.Discount;
                                oDeliveryDocuments.Lines.TaxCode = l.TaxCode;
                                oDeliveryDocuments.Lines.LineTotal = (double)Math.Round(DecimalVar, 2);
                                oDeliveryDocuments.Lines.Rate = 0.00;

                                double DoubleVar = System.Math.Round(Convert.ToDouble(calcLineStr), 2);
                                SumHTotal = SumHTotal + DoubleVar;
                            }

                            if (l.ManageItem == "S") {
                                int nS = 0;
                                foreach (var serial in l.Serial)
                                {
                                    oDeliveryDocuments.Lines.SerialNumbers.Quantity = 1;
                                    //oGoodReceiptPO.Lines.SerialNumbers.ManufacturerSerialNumber = serial.MfrSerialNo;
                                    oDeliveryDocuments.Lines.SerialNumbers.InternalSerialNumber = serial.SerialNumber;
                                    oDeliveryDocuments.Lines.SerialNumbers.Add();

                                    oDeliveryDocuments.Lines.BinAllocations.SerialAndBatchNumbersBaseLine = nS;
                                    oDeliveryDocuments.Lines.BinAllocations.BinAbsEntry = serial.BinEntry;
                                    oDeliveryDocuments.Lines.BinAllocations.Quantity = 1;
                                    oDeliveryDocuments.Lines.BinAllocations.Add();
                                    nS++;
                                }

                            }
                            else if (l.ManageItem == "B")
                            {
                                int nB = 0;
                                foreach (var batch in l.Batches)
                                {
                                    //oGoodReceiptPO.Lines.BatchNumbers.AddmisionDate = batch.AdmissionDate;
                                    //oDeliveryDocuments.Lines.BatchNumbers.ExpiryDate = Convert.ToDateTime(batch.ExpDate);
                                    //oGoodReceiptPO.Lines.BatchNumbers.ManufacturingDate = batch.MfrDate;
                                    oDeliveryDocuments.Lines.BatchNumbers.Quantity = batch.Qty;
                                    oDeliveryDocuments.Lines.BatchNumbers.BatchNumber = batch.BatchNumber;
                                    oDeliveryDocuments.Lines.BatchNumbers.Add();

                                    oDeliveryDocuments.Lines.BinAllocations.SerialAndBatchNumbersBaseLine = nB;
                                    oDeliveryDocuments.Lines.BinAllocations.BinAbsEntry = batch.BinEntry;
                                    oDeliveryDocuments.Lines.BinAllocations.Quantity = batch.Qty;
                                    oDeliveryDocuments.Lines.BinAllocations.Add();
                                    nB++;
                                }
                            }
                            else if (l.ManageItem == "N")
                            {
                                oDeliveryDocuments.Lines.BinAllocations.SerialAndBatchNumbersBaseLine = 0;
                                oDeliveryDocuments.Lines.BinAllocations.BinAbsEntry = l.BinEntry;
                                oDeliveryDocuments.Lines.BinAllocations.Quantity = l.Quantity;
                                oDeliveryDocuments.Lines.BinAllocations.Add();
                            }


                            oDeliveryDocuments.Lines.Add();

                            
                            //End Line
                        }

                    }

                    double DiscPerc = sendDelivery.DiscountPercent;
                    double calcDiscPrcnt = DiscPerc / 100;

                    //Calc Discount
                    double cXBefPriceTotal = System.Math.Round(SumHTotal * calcDiscPrcnt, 2);
                    double cYBefPriceTotal = System.Math.Round(SumHTotal - cXBefPriceTotal, 2);

                    //Calc TaxTotal
                    double calcTaxTotal = System.Math.Round(cYBefPriceTotal * 0.07, 2);

                    
                    double cSumHTotal = System.Math.Round(cYBefPriceTotal + calcTaxTotal, 2);
                    cSumHTotal = System.Math.Round(cSumHTotal, 0);

                    oDeliveryDocuments.DocTotal = cSumHTotal;

                    Retval = oDeliveryDocuments.Add();
                    if (Retval != 0)
                    {
                        oCompany.GetLastError(out ErrCode, out ErrMsg);
                        return Task.FromResult(new ResponseDelivery
                        {
                            ErrorCode = ErrCode,
                            ErrorMsg = ErrMsg,
                            DocEntry = null
                        });
                    }

                    return Task.FromResult(new ResponseDelivery
                    {
                        ErrorCode = 0,
                        ErrorMsg = "",
                        DocEntry = oCompany.GetNewObjectKey()
                    });
                }

                return Task.FromResult(new ResponseDelivery
                {
                    ErrorCode = login.LErrCode,
                    ErrorMsg = login.SErrMsg
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResponseDelivery
                {
                    ErrorCode = ex.HResult,
                    ErrorMsg = ex.Message
                });
            }
        }
        public Task<ResponseGetSaleOrder> responseGetSaleOrder()
        {
            var getSaleOrder = new List<GetSaleOrder>();
            var dt = new DataTable();
            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {
                    var Query =
                        $"CALL \"{ConnectionString.CompanyDB}\".{ProcedureRoute._USP_CALLTRANS_TENGKIMLEANG} ('{ProcedureRoute.Type.GetSaleOrder}','','','','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                        getSaleOrder.Add(new GetSaleOrder
                        {
                            DocNum = row["DocNum"].ToString(),
                            CardCode = row["CardCode"].ToString(),
                            CardName = row["CardName"].ToString(),
                            AddressFrom = row["AddressFrom"].ToString(),
                            AddressTo = row["AddressTo"].ToString(),
                            DeliveryDate = Convert.ToDateTime(row["DeliveryDate"]).ToShortDateString()
                        });
                    return Task.FromResult(new ResponseGetSaleOrder
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = getSaleOrder.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetSaleOrder
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }

            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetSaleOrder
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }
        public Task<ResponseGetORDR> responseGetORDR(string cardCode, string typeShow)
        {
            var oPORs = new List<ORDR>();
            var pOR1s = new List<RDR1>();
            var dt = new DataTable();
            var dtLine = new DataTable();

            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {
                    var Query =
                        $"CALL \"{ConnectionString.CompanyDB}\".{ProcedureRoute._USP_CALLTRANS_TENGKIMLEANG} ('{ProcedureRoute.Type.GetSO}','{cardCode}','','','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        dtLine = new DataTable();
                        //Query = $"CALL \"{ConnectionString.CompanyDB}\".{ProcedureRoute._USP_CALLTRANS_TENGKIMLEANG} ('{ProcedureRoute.Type.GetSOLine}','{row["DocEntry"]}','','','','')";

                        string inputX = "";
                            if (typeShow == "0")
                            {
                                inputX = "RDR1";
                            }
                            else if (typeShow == "1")
                            {
                                inputX = "RDR1-Notify";
                            }
                            else if (typeShow == "2")
                            {
                                inputX = "RDR1-NotifyOnly";
                            }
                            else
                            {
                                inputX = "RDR1";
                            }

                        Query = $"CALL \"{ConnectionString.CompanyDB}\".{ProcedureRoute._USP_CALLTRANS_TENGKIMLEANG} ('{inputX}','{row["DocEntry"]}','','','','')";
                        login.AD = new OdbcDataAdapter(Query, login.CN);
                        login.AD.Fill(dtLine);
                        pOR1s = new List<RDR1>();
                        foreach (DataRow drLine in dtLine.Rows)
                            pOR1s.Add(new RDR1
                            {
                                ItemCode = drLine["ItemCode"].ToString(),
                                Description = drLine["Description"].ToString(),
                                Quatity = Convert.ToDouble(drLine["Quantity"].ToString()),
                                InputQuantity = Convert.ToDouble(drLine["InputQuantity"].ToString()),
                                Price = Convert.ToDouble(drLine["Price"].ToString()),
                                PriceBeforeDis = Convert.ToDouble(drLine["PriceBefDi"].ToString()),
                                DiscPrcnt = Convert.ToDouble(drLine["DiscPrcnt"].ToString()),
                                DiscountAMT = Convert.ToDouble(drLine["DiscountAmt"].ToString()),
                                VatGroup = drLine["LineTotal"].ToString(),
                                WhsCode = drLine["WhsCode"].ToString(),
                                LineTotal = Convert.ToDouble(drLine["LineTotal"].ToString()),
                                ManageItem = drLine["ManageItem"].ToString(),
                                UomName = drLine["UomName"].ToString(),
                                TaxCode = drLine["TaxCode"].ToString(),
                                PriceAfVAT = Convert.ToDouble(drLine["PriceAfVAT"].ToString()),
                                Patient = drLine["Patient"].ToString(),
                                TranferNo = drLine["TranferNo"].ToString(),
                                LineNum = Convert.ToInt32(drLine["LineNum"].ToString())
                            });
                        oPORs.Add(new ORDR
                        {
                            DocEntry = Convert.ToInt32(row["DocENtry"].ToString()),
                            CardCode = row["CardCode"].ToString(),
                            CardName = row["CardName"].ToString(),
                            CntctCode = row["CntctCode"].ToString(),
                            NumAtCard = row["NumAtCard"].ToString(),
                            DocNum = row["DocNum"].ToString(),
                            DocStatus = row["DocStatus"].ToString(),
                            DocDate = Convert.ToDateTime(row["DocDate"]).ToShortDateString(),
                            DocDueDate = Convert.ToDateTime(row["DocDueDate"]).ToShortDateString(),
                            TaxDate = Convert.ToDateTime(row["TaxDate"]).ToShortDateString(),
                            DocTotal = Convert.ToDouble(row["DocTotal"]),
                            DiscPrcnt = Convert.ToDouble(row["DiscPrcnt"]),
                            DiscountAMT = Convert.ToDouble(row["DiscSum"].ToString()),                            
                            ToBinLocation = row["ToBinLocation"].ToString(),
                            SlpCode = Convert.ToInt32(row["SlpCode"].ToString()),
                            SlpName = row["SlpName"].ToString(),
                            Line = pOR1s.ToList()
                        });
                    }

                    return Task.FromResult(new ResponseGetORDR
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = oPORs.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetORDR
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }

            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetORDR
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }
        public Task<ResponseGetBatch> responseGetBatch(string ItemCode, string WhsCode)
        {
            var batchGets = new List<GetBatch>();
            var dt = new DataTable();
            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {
                    var Query =
                        $"CALL \"{ConnectionString.CompanyDB}\".{ProcedureRoute._USP_CALLTRANS_TENGKIMLEANG} ('{ProcedureRoute.Type.GetBatch}','{ItemCode}','{WhsCode}','','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                        batchGets.Add(new GetBatch
                        {
                            ItemCode = row["ItemCode"].ToString(),
                            BatchNumber = row["DistNumber"].ToString(),
                            Qty = Convert.ToDouble(row["Quantity"].ToString()),
                            ExpDate = row["ExpDate"].ToString(),
                            InputQty = Convert.ToDouble(row["InputQty"].ToString())
                        });
                    return Task.FromResult(new ResponseGetBatch
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = batchGets.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetBatch
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }

            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetBatch
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }        
        public Task<ResponseGetSerial> responseGetSerial(string ItemCode, string WhsCode)
        {
            var serialGets = new List<GetSerial>();
            var dt = new DataTable();
            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {
                    var Query =
                        $"CALL \"{ConnectionString.CompanyDB}\".{ProcedureRoute._USP_CALLTRANS_TENGKIMLEANG} ('{ProcedureRoute.Type.GetSerial}','{ItemCode}','{WhsCode}','','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                        serialGets.Add(new GetSerial
                        {
                            ItemCode = row["ItemCode"].ToString(),
                            SerialNumber = row["Serial"].ToString(),
                            Qty = Convert.ToDouble(row["Qty"].ToString())
                        });
                    return Task.FromResult(new ResponseGetSerial
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = serialGets.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetSerial
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }

            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetSerial
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }
        public Task<ResponseGetORDR> responseGetSO(string cardCode)
        {
            var headSO = new List<ORDR>();
            var lineSO = new List<RDR1>();
            var dt = new DataTable();
            var dtLine = new DataTable();
            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {
                    var Query =
                        $"CALL \"{ConnectionString.CompanyDB}\"._USP_CALLTRANS_TENGKIMLEANG ('ORDR','-','','','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {

                        //Start Line
                        dtLine = new DataTable();
                        Query =
                            $"CALL \"{ConnectionString.CompanyDB}\"._USP_CALLTRANS_TENGKIMLEANG ('RDR1','{row["DocEntry"]}','','','','')";
                        login.AD = new OdbcDataAdapter(Query, login.CN);
                        login.AD.Fill(dtLine);
                        lineSO = new List<RDR1>();
                        foreach (DataRow drLine in dtLine.Rows)
                            lineSO.Add(new RDR1
                            {
                                //DataLine
                                ItemCode = drLine["ItemCode"].ToString(),
                                Description = drLine["Description"].ToString(),
                                Quatity = Convert.ToDouble(drLine["Quantity"].ToString()),
                                Price = Convert.ToDouble(drLine["Price"].ToString()),
                                PriceBeforeDis = Convert.ToDouble(drLine["PriceBefDi"].ToString()),
                                DiscPrcnt = Convert.ToDouble(drLine["DiscPrcnt"].ToString()),
                                DiscountAMT = Convert.ToDouble(drLine["DiscountAmt"].ToString()),
                                VatGroup = drLine["LineTotal"].ToString(),
                                WhsCode = drLine["WhsCode"].ToString(),
                                LineTotal = Convert.ToDouble(drLine["LineTotal"].ToString()),
                                ManageItem = drLine["ManageItem"].ToString(),
                                UomName = drLine["UomName"].ToString(),
                                TaxCode = drLine["TaxCode"].ToString(),
                                LineNum = Convert.ToInt32(drLine["LineNum"].ToString())

                            });
                        //End Line

                        //Start Head
                        headSO.Add(new ORDR
                        {
                            //DataHead
                            DocEntry = Convert.ToInt32(row["DocEntry"].ToString())
                            , CardCode = row["CardCode"].ToString()
                            , CardName = row["CardName"].ToString()
                            , CntctCode = row["CntctCode"].ToString()
                            , NumAtCard = row["NumAtCard"].ToString()
                            , DocNum = row["DocNum"].ToString()
                            , DocStatus = row["DocStatus"].ToString()
                            , DocDate = Convert.ToDateTime(row["DocDate"]).ToShortDateString()
                            , DocDueDate = Convert.ToDateTime(row["DocDueDate"]).ToShortDateString()
                            , TaxDate = Convert.ToDateTime(row["TaxDate"]).ToShortDateString()
                            , DocTotal = Convert.ToDouble(row["DocTotal"])
                            , DiscPrcnt = Convert.ToDouble(row["DiscPrcnt"])
                            , DiscountAMT = Convert.ToDouble(row["DiscSum"].ToString())
                            , ToBinLocation = row["ToBinLocation"].ToString()
                            , Line = lineSO.ToList()
                        });
                    }
                    return Task.FromResult(new ResponseGetORDR
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = headSO.ToList()
                    });

                }
                return Task.FromResult(new ResponseGetORDR
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });

            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetORDR
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }

        }
        public Task<ResponseGetORDRNofity> responseGetORDRNofity(string cardCode, string typeShow)
        {
            var hRDR = new List<hORDR>();
            var lRDR = new List<lRDR1>();
            var lRDRBS = new List<lLineBS>();
            var dt = new DataTable();
            var dtLine = new DataTable();
            var dtSoBSLine = new DataTable();

            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {
                    var strCardCode = "";
                    var strDocNum = "";
                    var strWEBDOCNUM = "";
                    var Query =
                        $"CALL \"{ConnectionString.CompanyDB}\".{ProcedureRoute._USP_CALLTRANS_TENGKIMLEANG} ('{ProcedureRoute.Type.GetSO}','{cardCode}','','','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        dtLine = new DataTable();
                        dtSoBSLine = new DataTable();
                        //Query = $"CALL \"{ConnectionString.CompanyDB}\".{ProcedureRoute._USP_CALLTRANS_TENGKIMLEANG} ('{ProcedureRoute.Type.GetSOLine}','{row["DocEntry"]}','','','','')";

                        string inputX = "";
                        if (typeShow == "0")
                        {
                            inputX = "RDR1";
                        }
                        else if (typeShow == "1")
                        {
                            inputX = "RDR1-Notify";
                        }
                        else if (typeShow == "2")
                        {
                            inputX = "RDR1-NotifyOnly";
                        }
                        else
                        {
                            inputX = "RDR1";
                        }
                        string strBalanceQty = "";
                        Query = $"CALL \"{ConnectionString.CompanyDB}\".{ProcedureRoute._USP_CALLTRANS_TENGKIMLEANG} ('{inputX}','{row["DocEntry"]}','','','','')";
                        login.AD = new OdbcDataAdapter(Query, login.CN);
                        login.AD.Fill(dtLine);
                        lRDR = new List<lRDR1>();
                        foreach (DataRow drLine in dtLine.Rows)
                        {
                            strBalanceQty = drLine["U_BalanceQty"].ToString();
                            if (strBalanceQty == "0")
                            {
                                strBalanceQty = "0";
                            }
                            else
                            {
                                strBalanceQty = drLine["InputQuantity"].ToString();
                            }

                            lRDR.Add(new lRDR1
                            {
                                ItemCode = drLine["ItemCode"].ToString(),
                                Description = drLine["Description"].ToString(),
                                Quatity = Convert.ToDouble(drLine["Quantity"].ToString()),
                                InputQuantity = Convert.ToDouble(strBalanceQty),
                                Price = Convert.ToDouble(drLine["Price"].ToString()),
                                PriceBeforeDis = Convert.ToDouble(drLine["PriceBefDi"].ToString()),
                                DiscPrcnt = Convert.ToDouble(drLine["DiscPrcnt"].ToString()),
                                DiscountAMT = Convert.ToDouble(drLine["DiscountAmt"].ToString()),
                                VatGroup = drLine["LineTotal"].ToString(),
                                WhsCode = drLine["WhsCode"].ToString(),
                                LineTotal = Convert.ToDouble(drLine["LineTotal"].ToString()),
                                ManageItem = drLine["ManageItem"].ToString(),
                                UomName = drLine["UomName"].ToString(),
                                TaxCode = drLine["TaxCode"].ToString(),
                                PriceAfVAT = Convert.ToDouble(drLine["PriceAfVAT"].ToString()),
                                Patient = drLine["Patient"].ToString(),
                                TranferNo = drLine["TranferNo"].ToString(),
                                LineNum = Convert.ToInt32(drLine["LineNum"].ToString())
                            });
                        }                            

                        strCardCode = row["CardCode"].ToString();
                        strDocNum = row["IMNo"].ToString();
                        strWEBDOCNUM = row["WebDocNum"].ToString();

                        if(strWEBDOCNUM != "")
                        {
                            //Select Data From BARCODESYSTEMDB.TBSOBATCHSERIAL
                            var Query2 = $"CALL \"{ConnectionString.BarcodeDb}\"._USP_CALLTRANS_SO ('Get_SO_BatchSerial','{strCardCode}','{strDocNum}','{strWEBDOCNUM}','','')";
                            login.AD = new OdbcDataAdapter(Query2, login.CN);
                            login.AD.Fill(dtSoBSLine);
                            lRDRBS = new List<lLineBS>();
                            foreach (DataRow drLine2 in dtSoBSLine.Rows)
                            {
                                lRDRBS.Add(new lLineBS
                                {
                                    LineNum = drLine2["LineNum"].ToString(),
                                    ItemCode = drLine2["ItemCode"].ToString(),
                                    Dscription = drLine2["Dscription"].ToString(),
                                    Quatity = Convert.ToDouble(drLine2["Quantity"].ToString()),
                                    InputQuantity = Convert.ToDouble(drLine2["Quantity"].ToString()),
                                    UomCode = drLine2["UomCode"].ToString(),
                                    GrossPrice = Convert.ToDouble(drLine2["GrossPrice"].ToString()),
                                    BatchSerialType = drLine2["BatchSerialType"].ToString(),
                                    BatchNumber = drLine2["BatchNumber"].ToString(),
                                    SerialNumber = drLine2["SerialNumber"].ToString(),
                                    Warehouse = drLine2["Warehouse"].ToString(),
                                    BinEntry = drLine2["BinEntry"].ToString(),
                                    BinLocation = drLine2["BinLocation"].ToString(),
                                    Patient = drLine2["Patient"].ToString(),
                                    BaseLine = drLine2["BASELINE"].ToString()
                                });
                            }
                        }
                        else
                        {
                            lRDRBS = new List<lLineBS>();
                        }
                        

                        hRDR.Add(new hORDR
                        {
                            DocEntry = Convert.ToInt32(row["DocENtry"].ToString()),
                            CardCode = row["CardCode"].ToString(),
                            CardName = row["CardName"].ToString(),
                            CntctCode = row["CntctCode"].ToString(),
                            NumAtCard = row["NumAtCard"].ToString(),
                            DocNum = row["DocNum"].ToString(),
                            DocStatus = row["DocStatus"].ToString(),
                            DocDate = Convert.ToDateTime(row["DocDate"]).ToShortDateString(),
                            DocDueDate = Convert.ToDateTime(row["DocDueDate"]).ToShortDateString(),
                            TaxDate = Convert.ToDateTime(row["TaxDate"]).ToShortDateString(),
                            DocTotal = Convert.ToDouble(row["DocTotal"]),
                            DiscPrcnt = Convert.ToDouble(row["DiscPrcnt"]),
                            DiscountAMT = Convert.ToDouble(row["DiscSum"].ToString()),
                            ToBinLocation = row["ToBinLocation"].ToString(),
                            SlpCode = Convert.ToInt32(row["SlpCode"].ToString()),
                            SlpName = row["SlpName"].ToString(),
                            SQRemark = row["SQ_Remark"].ToString(),
                            Line = lRDR.ToList(),
                            LineBatchSerial = lRDRBS.ToList()
                        });
                    }

                    return Task.FromResult(new ResponseGetORDRNofity
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = hRDR.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetORDRNofity
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }

            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetORDRNofity
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }
    }
}
