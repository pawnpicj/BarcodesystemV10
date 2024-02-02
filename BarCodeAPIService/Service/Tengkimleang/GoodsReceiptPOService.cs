using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BarCodeAPIService.Connection;
using BarCodeAPIService.Models;
using BarCodeLibrary.Contract.RouteProcedure;
using BarCodeLibrary.Request.SAP.Tengkimleang;
using BarCodeLibrary.Request.SAP.TengKimleang;
using BarCodeLibrary.Respones.SAP.Tengkimleang;
using SAPbobsCOM;

namespace BarCodeAPIService.Service
{
    public class GoodsReceiptPOService : IGoodsReceiptPOService
    {
        private int ErrCode;
        private string ErrMsg;

        #region Post
        public Task<ResponseGoodReceiptPO> PostGoodReceiptPO(SendGoodReceiptPO sendGoodReceiptPO)
        {
            try
            {
                Documents oGoodReceiptPO;
                Company oCompany;
                var Retval = 0;
                Login login = new();
                if (login.LErrCode == 0)
                {
                    oCompany = login.Company;
                    oGoodReceiptPO = (Documents)oCompany.GetBusinessObject(BoObjectTypes.oPurchaseDeliveryNotes);
                    oGoodReceiptPO.CardCode = sendGoodReceiptPO.CardCode;
                    oGoodReceiptPO.DocDate = sendGoodReceiptPO.DocDate;
                    oGoodReceiptPO.DocDueDate = sendGoodReceiptPO.TaxDate;
                    oGoodReceiptPO.Series = Convert.ToInt32(sendGoodReceiptPO.Series);
                    oGoodReceiptPO.DocCurrency = sendGoodReceiptPO.CurrencyCode;
                    oGoodReceiptPO.Comments = (sendGoodReceiptPO.Remark==null)? "": sendGoodReceiptPO.Remark;
                    oGoodReceiptPO.SalesPersonCode = Convert.ToInt32(sendGoodReceiptPO.SlpCode);
                    oGoodReceiptPO.UserFields.Fields.Item("U_WebID").Value = sendGoodReceiptPO.Series + DateTime.Today.Day
                        + DateTime.Today.Month
                        + DateTime.Today.Year
                        + DateTime.Today.DayOfYear
                        + DateTime.Today.Hour
                        + DateTime.Today.Minute
                        + DateTime.Today.Second
                        + DateTime.Today.Millisecond;
                    foreach (var l in sendGoodReceiptPO.Line)
                    {
                        oGoodReceiptPO.Lines.ItemCode = l.ItemCode;
                        oGoodReceiptPO.Lines.Quantity = l.Quantity;
                        oGoodReceiptPO.Lines.UnitPrice = l.PriceBeforeDis;
                        oGoodReceiptPO.Lines.DiscountPercent = l.Discount;
                        oGoodReceiptPO.Lines.WarehouseCode = l.Whs;
                        oGoodReceiptPO.Lines.UoMEntry = Convert.ToInt32(l.UomName);
                        if (l.DocEntry != null)
                        {
                            oGoodReceiptPO.Lines.BaseEntry = Convert.ToInt32(l.DocEntry);
                            oGoodReceiptPO.Lines.BaseType = 22;
                            oGoodReceiptPO.Lines.BaseLine = l.LineNum;
                        }

                        if (l.ManageItem == "S")
                            foreach (var serial in l.Serial)
                            {
                                oGoodReceiptPO.Lines.SerialNumbers.Quantity = 1;
                                oGoodReceiptPO.Lines.SerialNumbers.ManufacturerSerialNumber = serial.MfrSerialNo;
                                oGoodReceiptPO.Lines.SerialNumbers.InternalSerialNumber = serial.SerialNumber;
                                oGoodReceiptPO.Lines.SerialNumbers.ExpiryDate = serial.ExpDate;
                                oGoodReceiptPO.Lines.SerialNumbers.Add();
                            }
                        else if (l.ManageItem == "B")
                            foreach (var batch in l.Batches)
                            {
                                oGoodReceiptPO.Lines.BatchNumbers.AddmisionDate = batch.AdmissionDate;
                                oGoodReceiptPO.Lines.BatchNumbers.ExpiryDate = batch.ExpirationDate;
                                oGoodReceiptPO.Lines.BatchNumbers.ManufacturingDate = batch.MfrDate;
                                oGoodReceiptPO.Lines.BatchNumbers.Quantity = batch.Qty;
                                oGoodReceiptPO.Lines.BatchNumbers.BatchNumber = batch.SerialAndBatch;
                                oGoodReceiptPO.Lines.BatchNumbers.Add();
                            }

                        oGoodReceiptPO.Lines.Add();
                    }

                    Retval = oGoodReceiptPO.Add();
                    if (Retval != 0)
                    {
                        oCompany.GetLastError(out ErrCode, out ErrMsg);
                        return Task.FromResult(new ResponseGoodReceiptPO
                        {
                            ErrorCode = ErrCode,
                            ErrorMsg = ErrMsg,
                            DocEntry = null
                        });
                    }

                    return Task.FromResult(new ResponseGoodReceiptPO
                    {
                        ErrorCode = 0,
                        ErrorMsg = "",
                        DocEntry = oCompany.GetNewObjectKey()
                    });
                }

                return Task.FromResult(new ResponseGoodReceiptPO
                {
                    ErrorCode = login.LErrCode,
                    ErrorMsg = login.SErrMsg
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGoodReceiptPO
                {
                    ErrorCode = ex.HResult,
                    ErrorMsg = ex.Message
                });
            }
        }

        #endregion

        #region Get
        public Task<ResponseCustomerGet> responseCustomerGets(string cusType)
        {
            var customerGets = new List<CustomerGet>();
            var dt = new DataTable();
            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {
                    var Query =
                        $"CALL \"{ConnectionString.CompanyDB}\".{ProcedureRoute._USP_CALLTRANS_TENGKIMLEANG} ('{ProcedureRoute.Type.CustomerGet}','{cusType}','','','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                        customerGets.Add(new CustomerGet
                        {
                            CardCode = row["CardCode"].ToString(),
                            CardName = row["CardName"].ToString(),
                            Address = row["Address"].ToString(),
                            Phone = row["Phone"].ToString()
                        });
                    return Task.FromResult(new ResponseCustomerGet
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = customerGets.ToList()
                    });
                }

                return Task.FromResult(new ResponseCustomerGet
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }

            catch (Exception ex)
            {
                return Task.FromResult(new ResponseCustomerGet
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }
        public Task<ResponseOPORGetPO> responseOPORGetPO(string cardName)
        {
            var oPORs = new List<OPOR>();
            var pOR1s = new List<POR1>();
            var dt = new DataTable();
            var dtLine = new DataTable();
            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {
                    var Query =
                        $"CALL \"{ConnectionString.CompanyDB}\".{ProcedureRoute._USP_CALLTRANS_TENGKIMLEANG} ('{ProcedureRoute.Type.GetPO}','{cardName}','','','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        dtLine = new DataTable();
                        Query =
                            $"CALL \"{ConnectionString.CompanyDB}\".{ProcedureRoute._USP_CALLTRANS_TENGKIMLEANG} ('{ProcedureRoute.Type.GetPOLine}','{row["DocENtry"]}','','','','')";
                        login.AD = new OdbcDataAdapter(Query, login.CN);
                        login.AD.Fill(dtLine);
                        pOR1s = new List<POR1>();
                        foreach (DataRow drLine in dtLine.Rows)
                            pOR1s.Add(new POR1
                            {
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
                        oPORs.Add(new OPOR
                        {
                            DocEntry = Convert.ToInt32(row["DocENtry"].ToString()),
                            CardCode = row["CardCode"].ToString(),
                            CardName = row["CardName"].ToString(),
                            CntctCode = Convert.ToInt32(row["CntctCode"].ToString()),
                            NumAtCard = row["NumAtCard"].ToString(),
                            DocNum = Convert.ToInt32(row["DocNum"].ToString()),
                            DocStatus = row["DocStatus"].ToString(),
                            DocDate = Convert.ToDateTime(row["DocDate"]).ToShortDateString(),
                            DocDueDate = Convert.ToDateTime(row["DocDueDate"]).ToShortDateString(),
                            TaxDate = Convert.ToDateTime(row["TaxDate"]).ToShortDateString(),
                            DocTotal = Convert.ToDouble(row["DocTotal"]),
                            DiscPrcnt = Convert.ToDouble(row["DiscPrcnt"]),
                            DiscountAMT = Convert.ToDouble(row["DiscSum"].ToString()),
                            Line = pOR1s.ToList()
                        });
                    }

                    return Task.FromResult(new ResponseOPORGetPO
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = oPORs.ToList()
                    });
                }

                return Task.FromResult(new ResponseOPORGetPO
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }

            catch (Exception ex)
            {
                return Task.FromResult(new ResponseOPORGetPO
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }
        public Task<ResponseGetSeries> responseGetSeries(string objectCode, string dateOfMonth)
        {
            var getSeries = new List<GetSeries>();
            var dt = new DataTable();
            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {
                    var Query =
                        $"CALL \"{ConnectionString.CompanyDB}\".{ProcedureRoute._USP_CALLTRANS_TENGKIMLEANG} ('{ProcedureRoute.Type.GetSeries}','{objectCode}','{dateOfMonth}','','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                        getSeries.Add(new GetSeries
                        {
                            Code = Convert.ToInt32(row["Code"].ToString()),
                            Name = row["Name"].ToString()
                        });
                    return Task.FromResult(new ResponseGetSeries
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = getSeries.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetSeries
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }

            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetSeries
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }
        public Task<ResponseGetSaleEmployee> responseGetSaleEmployees()
        {
            var getSaleEmployees = new List<GetSaleEmployee>();
            var dt = new DataTable();
            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {
                    var Query =
                        $"CALL \"{ConnectionString.CompanyDB}\".{ProcedureRoute._USP_CALLTRANS_TENGKIMLEANG} ('{ProcedureRoute.Type.GetSaleEmployee}','','','','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                        getSaleEmployees.Add(new GetSaleEmployee
                        {
                            Code = Convert.ToInt32(row["Code"].ToString()),
                            Name = row["Name"].ToString()
                        });
                    return Task.FromResult(new ResponseGetSaleEmployee
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = getSaleEmployees.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetSaleEmployee
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }

            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetSaleEmployee
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }
        public Task<ResponseGetCurrency> responseGetCurrency(string cardCode)
        {
            var getCurrencies = new List<GetCurrency>();
            var dt = new DataTable();
            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {
                    var Query =
                        $"CALL \"{ConnectionString.CompanyDB}\".{ProcedureRoute._USP_CALLTRANS_TENGKIMLEANG} ('{ProcedureRoute.Type.GetCurrency}','{cardCode}','','','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                        getCurrencies.Add(new GetCurrency
                        {
                            Code = row["Code"].ToString(),
                            Name = row["Name"].ToString()
                        });
                    return Task.FromResult(new ResponseGetCurrency
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = getCurrencies.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetCurrency
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }

            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetCurrency
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }
        public Task<ResponseGetGenerateBatchSerial> responseGetGenerateBatchSerial(
            GetGenerateSerialBatchRequest generateSerialBatchRequest)
        {
            var getGenerateBatchSerials = new List<GetGenerateBatchSerial>();
            var dt = new DataTable();
            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SqlHana);
                if (login.lErrCode == 0)
                {
                    var Query = "";
                    foreach (var qu in generateSerialBatchRequest.ListSerials)
                        if (qu.TypeSerialGen == "2")
                        {
                            for (var k = qu.SerialFrom; k <= qu.SerialTo; k++)
                            {
                                Query =
                                    $"CALL \"{ConnectionString.BarcodeDb}\".\"{ProcedureRoute._USP_GENERATE_SERIAL_SqlHana}\" ('{qu.itemCode}','{qu.qty}')";
                                login.AD = new OdbcDataAdapter(Query, login.CN);
                                login.AD.Fill(dt);
                                foreach (DataRow row in dt.Rows)
                                    getGenerateBatchSerials.Add(new GetGenerateBatchSerial
                                    {
                                        SerialAndBatch = row["SerialOrBatchGen"].ToString(),
                                        Script = row["SCRIPT_SERIAL"].ToString(),
                                        MfrDate = qu.MfrNo,
                                        ExpirationDate = qu.ExpireDate
                                    });
                            }
                        }
                        else
                        {
                            Query =
                                $"CALL \"{ConnectionString.BarcodeDb}\".\"{ProcedureRoute._USP_GENERATE_SERIAL_SqlHana}\" ('{qu.itemCode}','{qu.qty}')";
                            login.AD = new OdbcDataAdapter(Query, login.CN);
                            login.AD.Fill(dt);
                            foreach (DataRow row in dt.Rows)
                                getGenerateBatchSerials.Add(new GetGenerateBatchSerial
                                {
                                    SerialAndBatch = row["SerialOrBatchGen"].ToString(),
                                    Script = row["SCRIPT_SERIAL"].ToString()
                                });
                        }
                    return Task.FromResult(new ResponseGetGenerateBatchSerial
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = getGenerateBatchSerials.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetGenerateBatchSerial
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }

            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetGenerateBatchSerial
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }
        public Task<ResponseGetGenerateBatchSerial> responseGetGenerateBatchAsync(
            GetBatchGenRequest generateBatchRequest)
        {
            var getGenerateBatchSerials = new List<GetGenerateBatchSerial>();
            var dt = new DataTable();
            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SqlHana);
                if (login.lErrCode == 0)
                {
                    var Query = "";
                    foreach (var qu in generateBatchRequest.ListBatches)
                        if (qu.TypeBatchGen == "2")
                        {
                            for (var k = qu.BatchFrom; k <= qu.BatchTo; k++)
                            {
                                Query =
                                    $"CALL \"{ConnectionString.BarcodeDb}\".\"{ProcedureRoute._USP_GENERATE_Batch_SqlHana}\" ('{qu.ItemCode}','{qu.Qty.ToString()}')";
                                login.AD = new OdbcDataAdapter(Query, login.CN);
                                dt = new DataTable();
                                login.AD.Fill(dt);
                                foreach (DataRow row in dt.Rows)
                                    getGenerateBatchSerials.Add(new GetGenerateBatchSerial
                                    {
                                        ItemCode = qu.ItemCode,
                                        Qty = qu.Qty,
                                        SerialAndBatch = row["SerialOrBatchGen"].ToString(),
                                        Script = row["SCRIPT_SERIAL"].ToString(),
                                        MfrDate = qu.ManufactureDate.ToShortDateString(),
                                        ExpirationDate = qu.ExpireDate.ToShortDateString(),
                                        AdmissionDate = qu.AdmissionDate.ToShortDateString()
                                    });
                            }
                        }
                        else
                        {
                            Query =
                                $"CALL \"{ConnectionString.BarcodeDb}\".\"{ProcedureRoute._USP_GENERATE_Batch_SqlHana}\" ('{qu.ItemCode}','{qu.Qty.ToString()}')";
                            login.AD = new OdbcDataAdapter(Query, login.CN);
                            login.AD.Fill(dt);
                            foreach (DataRow row in dt.Rows)
                                getGenerateBatchSerials.Add(new GetGenerateBatchSerial
                                {
                                    ItemCode = qu.ItemCode,
                                    Qty = qu.Qty,
                                    SerialAndBatch = row["SerialOrBatchGen"].ToString(),
                                    Script = row["SCRIPT_SERIAL"].ToString(),
                                    MfrDate = qu.ManufactureDate.ToShortDateString(),
                                    ExpirationDate = qu.ExpireDate.ToShortDateString(),
                                    AdmissionDate = qu.AdmissionDate.ToShortDateString()
                                });
                        }

                    return Task.FromResult(new ResponseGetGenerateBatchSerial
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = getGenerateBatchSerials.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetGenerateBatchSerial
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }

            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetGenerateBatchSerial
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }
        public Task<ResponseGetItemCode> responseGetItemCodes()
        {
            var getItemCodes = new List<GetItemCode>();
            var dt = new DataTable();
            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {
                    var Query =
                        $"CALL \"{ConnectionString.CompanyDB}\".{ProcedureRoute._USP_CALLTRANS_TENGKIMLEANG} ('{ProcedureRoute.Type.GetItemCode}','','','','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                        getItemCodes.Add(new GetItemCode
                        {
                            ItemCode = row["ITEMCODE"].ToString(),
                            ItemName = row["ITEMNAME"].ToString(),
                            UnitPrice = row["PRICE"].ToString(),
                            Quantity = row["QTYONHAND"].ToString(),
                            UomName = row["UOMNAME"].ToString(),
                            BarCode = row["BARCODE"].ToString(),
                            ManageItem = row["MANAGEITEM"].ToString()
                        });
                    return Task.FromResult(new ResponseGetItemCode
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = getItemCodes.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetItemCode
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }

            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetItemCode
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }
        public Task<ResponseGetVatCode> responseGetVatCodes()
        {
            var getVatCodes = new List<GetVatCode>();
            var dt = new DataTable();
            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {
                    var Query =
                        $"CALL \"{ConnectionString.CompanyDB}\".{ProcedureRoute._USP_CALLTRANS_TENGKIMLEANG} ('{ProcedureRoute.Type.GetVatCode}','','','','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                        getVatCodes.Add(new GetVatCode
                        {
                            Code = row["Code"].ToString(),
                            Name = row["Name"].ToString(),
                            Rate = Convert.ToDouble(row["Rate"].ToString())
                        });
                    return Task.FromResult(new ResponseGetVatCode
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = getVatCodes.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetVatCode
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }

            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetVatCode
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }
        public Task<ResponseGetWarehouse> responseGetWarehouses()
        {
            var getWarehouses = new List<GetWarehouse>();
            var dt = new DataTable();
            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {
                    var Query =
                        $"CALL \"{ConnectionString.CompanyDB}\".{ProcedureRoute._USP_CALLTRANS_TENGKIMLEANG} ('{ProcedureRoute.Type.GetWarehouse}','','','','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                        getWarehouses.Add(new GetWarehouse
                        {
                            Code = row["Code"].ToString(),
                            Name = row["Name"].ToString()
                        });
                    return Task.FromResult(new ResponseGetWarehouse
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = getWarehouses.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetWarehouse
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }

            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetWarehouse
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }
        public Task<ResponseGetUnitOfMeasure> responseGetUnitOfMeasure(string ItemCode,string UOMType)
        {
            var getUnitOfMeasure = new List<GetUnitOfMeasure>();
            var dt = new DataTable();
            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {
                    var Query =
                        $"CALL \"{ConnectionString.CompanyDB}\".{ProcedureRoute._USP_CALLTRANS_TENGKIMLEANG} ('{ProcedureRoute.Type.GetUom}','{ItemCode}','{UOMType}','','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                        getUnitOfMeasure.Add(new GetUnitOfMeasure
                        {
                            Code = row["Code"].ToString(),
                            Name = row["Name"].ToString()
                        });
                    return Task.FromResult(new ResponseGetUnitOfMeasure
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = getUnitOfMeasure.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetUnitOfMeasure
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }

            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetUnitOfMeasure
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }
        public Task<ResponseORPDGetGoodReturn> responseORPDGetGoodReturn(string cardName)
        {
            var listOrpds = new List<ORPD>();
            var dt = new DataTable();
            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {
                    var Query =
                        $"CALL \"{ConnectionString.CompanyDB}\".{ProcedureRoute._USP_CALLTRANS_TENGKIMLEANG} ('{ProcedureRoute.Type.GetGoodReturn}','{cardName}','','','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                        listOrpds.Add(new ORPD
                        {
                            DocNum = Convert.ToInt32(row["DocNum"].ToString()),
                            DocTotal = Convert.ToDouble(row["DocTotal"].ToString()),
                            CardCode = row["CardCode"].ToString(),
                            DocDate = row["DocDate"].ToString(),
                            DocStatus = row["DocStatus"].ToString()
                        });
                    return Task.FromResult(new ResponseORPDGetGoodReturn
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = listOrpds.ToList()
                    });
                }

                return Task.FromResult(new ResponseORPDGetGoodReturn
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResponseORPDGetGoodReturn
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }
        #endregion
    }
}