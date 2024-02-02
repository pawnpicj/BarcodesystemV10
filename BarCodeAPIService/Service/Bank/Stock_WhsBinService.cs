using BarCodeAPIService.Connection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BarCodeLibrary.Respones.SAP;
using BarCodeLibrary.Respones.SAP.Bank;
using SAPbobsCOM;
using BarCodeAPIService.Models;
using System.Globalization;
using System.Data;
using System.Data.Odbc;

namespace BarCodeAPIService.Service.Bank
{
    public class Stock_WhsBinService : IStock_WhsBinService
    {
        public Task<ResponseGetStockByWhsBin> responseGetStockByWhsBin(string whsCode, string binCode)
        {
            var dataLine = new List<LineStock>();
            var dt = new DataTable();

            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {

                    var Query = $"CALL \"{ConnectionString.CompanyDB}\"._USP_CALLTRANS_BANK ('GETSTOCKWB','{whsCode}','{binCode}','','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        dataLine.Add(new LineStock
                        {
                            ItemCode = row["ItemCode"].ToString(),
                            ItemName = row["ItemName"].ToString(),
                            WhsCode = row["WhsCode"].ToString(),
                            WhsName = row["WhsName"].ToString(),
                            BinCode = row["BinCode"].ToString(),
                            BinEntry = Convert.ToInt32(row["BinEntry"].ToString()),
                            Quantity = Convert.ToDouble(row["Quantity"].ToString()),
                            UOMCode = row["UOMCode"].ToString(),
                            BatchYN = row["BatchYN"].ToString(),
                            SerialYN = row["SerialYN"].ToString()
                        });
                    }
                    return Task.FromResult(new ResponseGetStockByWhsBin
                    {
                        ErrorCode = 0,
                        ErrorMsg = "",
                        Data = dataLine.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetStockByWhsBin
                {
                    ErrorCode = login.lErrCode,
                    ErrorMsg = login.sErrMsg,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetStockByWhsBin
                {
                    ErrorCode = ex.HResult,
                    ErrorMsg = ex.Message,
                    Data = null
                });
            }
        }

        public Task<ResponseGetStockItemBatchAndSerial> responseGetStockItemBatchBin(string itemCode, string batchNumber, string binEntry)
        {
            var dataLine = new List<GetStockItemBatchAndSerial>();
            var dt = new DataTable();

            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {

                    var Query = $"CALL \"{ConnectionString.CompanyDB}\"._USP_CALLTRANS_BANK('GetStock_Batch_Bin','{itemCode}','{batchNumber}','{binEntry}','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        dataLine.Add(new GetStockItemBatchAndSerial
                        {

                            ItemCode = row["ItemCode"].ToString(),
                            ItemName = row["ItemName"].ToString(),
                            Quantity = Convert.ToDouble(row["Qty"].ToString()),
                            UOMCode = row["UOMCode"].ToString(),
                            WhsEntry = Convert.ToInt32(row["WhsEntry"].ToString()),
                            WhsCode = row["WhsCode"].ToString(),
                            BinEntry = Convert.ToInt32(row["BinEntry"].ToString()),
                            BinCode = row["BinCode"].ToString(),
                            BatchNumber = row["BatchNumber"].ToString(),
                            ExpDate = row["ExpDate"].ToString()

                        });
                    }
                    return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = dataLine.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }

        public Task<ResponseGetStockItemBatchAndSerial> responseGetStockItemSerialBin(string itemCode, string serialNumber, string binEntry)
        {
            var dataLine = new List<GetStockItemBatchAndSerial>();
            var dt = new DataTable();

            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {

                    var Query = $"CALL \"{ConnectionString.CompanyDB}\"._USP_CALLTRANS_BANK ('GetStock_Serial_Bin','{itemCode}','{serialNumber}','{binEntry}','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        dataLine.Add(new GetStockItemBatchAndSerial
                        {

                            ItemCode = row["ItemCode"].ToString(),
                            ItemName = row["ItemName"].ToString(),
                            Quantity = Convert.ToDouble(row["Qty"].ToString()),
                            UOMCode = row["UOMCode"].ToString(),
                            WhsEntry = Convert.ToInt32(row["WhsEntry"].ToString()),
                            WhsCode = row["WhsCode"].ToString(),
                            BinEntry = Convert.ToInt32(row["BinEntry"].ToString()),
                            BinCode = row["BinCode"].ToString(),
                            SerialNumber = row["SerialNumber"].ToString(),
                            ExpDate = row["ExpDate"].ToString()

                        });
                    }
                    return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = dataLine.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }

        public Task<ResponseGetStockItemBatchAndSerial> responseGetStockItemBatch(string itemCode, string batchNumber)
        {
            var dataLine = new List<GetStockItemBatchAndSerial>();
            var dt = new DataTable();

            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {

                    var Query = $"CALL \"{ConnectionString.CompanyDB}\"._USP_CALLTRANS_BANK ('GetStock_Batch','{itemCode}','{batchNumber}','','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        dataLine.Add(new GetStockItemBatchAndSerial
                        {

                            ItemCode = row["ItemCode"].ToString(),
                            ItemName = row["ItemName"].ToString(),
                            Quantity = Convert.ToDouble(row["Qty"].ToString()),
                            UOMCode = row["UOMCode"].ToString(),
                            WhsEntry = Convert.ToInt32(row["WhsEntry"].ToString()),
                            WhsCode = row["WhsCode"].ToString(),
                            BinEntry = Convert.ToInt32(row["BinEntry"].ToString()),
                            BinCode = row["BinCode"].ToString(),
                            BatchNumber = row["BatchNumber"].ToString(),
                            ExpDate = row["ExpDate"].ToString()

                        });
                    }
                    return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = dataLine.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }

        public Task<ResponseGetStockItemBatchAndSerial> responseGetStockItemSerial(string itemCode, string serialNumber)
        {
            var dataLine = new List<GetStockItemBatchAndSerial>();
            var dt = new DataTable();

            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {

                    var Query = $"CALL \"{ConnectionString.CompanyDB}\"._USP_CALLTRANS_BANK ('GetStock_Serial','{itemCode}','{serialNumber}','','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        dataLine.Add(new GetStockItemBatchAndSerial
                        {

                            ItemCode = row["ItemCode"].ToString(),
                            ItemName = row["ItemName"].ToString(),
                            Quantity = Convert.ToDouble(row["Qty"].ToString()),
                            UOMCode = row["UOMCode"].ToString(),
                            WhsEntry = Convert.ToInt32(row["WhsEntry"].ToString()),
                            WhsCode = row["WhsCode"].ToString(),
                            BinEntry = Convert.ToInt32(row["BinEntry"].ToString()),
                            BinCode = row["BinCode"].ToString(),
                            BatchNumber = row["BatchNumber"].ToString(),
                            ExpDate = row["ExpDate"].ToString()

                        });
                    }
                    return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = dataLine.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }

        public Task<ResponseGetStockItemBatchAndSerial> responseGetStockItemBatchW(string itemCode, string batchNumber, string whsCode)
        {
            var dataLine = new List<GetStockItemBatchAndSerial>();
            var dt = new DataTable();

            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {

                    var Query = $"CALL \"{ConnectionString.CompanyDB}\"._USP_CALLTRANS_BANK ('GetStock_Batch_Whs','{itemCode}','{batchNumber}','{whsCode}','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        dataLine.Add(new GetStockItemBatchAndSerial
                        {
                            ItemCode = row["ItemCode"].ToString(),
                            ItemName = row["ItemName"].ToString(),
                            Quantity = Convert.ToDouble(row["Qty"].ToString()),
                            UOMCode = row["UOMCode"].ToString(),
                            WhsEntry = Convert.ToInt32(row["WhsEntry"].ToString()),
                            WhsCode = row["WhsCode"].ToString(),
                            BinEntry = Convert.ToInt32(row["BinEntry"].ToString()),
                            BinCode = row["BinCode"].ToString(),
                            BatchNumber = row["BatchNumber"].ToString(),
                            ExpDate = row["ExpDate"].ToString()
                        });
                    }
                    return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = dataLine.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }

        public Task<ResponseGetStockItemBatchAndSerial> responseGetStockItemBatchWCounting(string itemCode, string batchNumber, string whsCode)
        {
            var dataLine = new List<GetStockItemBatchAndSerial>();
            var dt = new DataTable();

            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {

                    var Query = $"CALL \"{ConnectionString.CompanyDB}\"._USP_CALLTRANS_BANK ('GetStock_Batch_Whs_Counting','{itemCode}','{batchNumber}','{whsCode}','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        dataLine.Add(new GetStockItemBatchAndSerial
                        {
                            ItemCode = row["ItemCode"].ToString(),
                            ItemName = row["ItemName"].ToString(),
                            Quantity = Convert.ToDouble(row["Qty"].ToString()),
                            UOMCode = row["UOMCode"].ToString(),
                            WhsEntry = Convert.ToInt32(row["WhsEntry"].ToString()),
                            WhsCode = row["WhsCode"].ToString(),
                            BinEntry = Convert.ToInt32(row["BinEntry"].ToString()),
                            BinCode = row["BinCode"].ToString(),
                            BatchNumber = row["BatchNumber"].ToString(),
                            ExpDate = row["ExpDate"].ToString()
                        });
                    }
                    return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = dataLine.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }

        public Task<ResponseGetStockItemBatchAndSerial> responseGetStockItemSerialW(string itemCode, string serialNumber, string whsCode)
        {
            var dataLine = new List<GetStockItemBatchAndSerial>();
            var dt = new DataTable();

            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {

                    var Query = $"CALL \"{ConnectionString.CompanyDB}\"._USP_CALLTRANS_BANK ('GetStock_Serial_Whs','{itemCode}','{serialNumber}','{whsCode}','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        dataLine.Add(new GetStockItemBatchAndSerial
                        {
                            ItemCode = row["ItemCode"].ToString(),
                            ItemName = row["ItemName"].ToString(),
                            Quantity = Convert.ToDouble(row["Qty"].ToString()),
                            UOMCode = row["UOMCode"].ToString(),
                            WhsEntry = Convert.ToInt32(row["WhsEntry"].ToString()),
                            WhsCode = row["WhsCode"].ToString(),
                            BinEntry = Convert.ToInt32(row["BinEntry"].ToString()),
                            BinCode = row["BinCode"].ToString(),
                            SerialNumber = row["SerialNumber"].ToString(),
                            ExpDate = row["ExpDate"].ToString()
                        });
                    }
                    return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = dataLine.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }

        public Task<ResponseGetStockItemBatchAndSerial> responseGetStockItemSerialWCounting(string itemCode, string serialNumber, string whsCode)
        {
            var dataLine = new List<GetStockItemBatchAndSerial>();
            var dt = new DataTable();

            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {

                    var Query = $"CALL \"{ConnectionString.CompanyDB}\"._USP_CALLTRANS_BANK ('GetStock_Serial_Whs_Counting','{itemCode}','{serialNumber}','{whsCode}','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        dataLine.Add(new GetStockItemBatchAndSerial
                        {
                            ItemCode = row["ItemCode"].ToString(),
                            ItemName = row["ItemName"].ToString(),
                            Quantity = Convert.ToDouble(row["Qty"].ToString()),
                            UOMCode = row["UOMCode"].ToString(),
                            WhsEntry = Convert.ToInt32(row["WhsEntry"].ToString()),
                            WhsCode = row["WhsCode"].ToString(),
                            BinEntry = Convert.ToInt32(row["BinEntry"].ToString()),
                            BinCode = row["BinCode"].ToString(),
                            SerialNumber = row["SerialNumber"].ToString(),
                            ExpDate = row["ExpDate"].ToString()
                        });
                    }
                    return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = dataLine.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }

        public Task<ResponseScanItemsInIM> responseScanItemsInIM(string docEntry, string itemCode, string batchSerialNo)
        {

            var dataLine = new List<GetItemsLine>();
            var dt = new DataTable();

            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {

                    var Query = $"CALL \"{ConnectionString.CompanyDB}\"._USP_CALLTRANS_BANK ('GetItemInIM','{docEntry}','{itemCode}','{batchSerialNo}','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        dataLine.Add(new GetItemsLine
                        {

                            DocEntry = Convert.ToInt32(row["DocEntry"].ToString()),
                            ItemCode = row["ItemCode"].ToString(),
                            ItemName = row["Dscription"].ToString(),
                            Quantity = Convert.ToDouble(row["Quantity"].ToString()),
                            UOMCode = row["UomCode"].ToString(),

                            FWhsCode = row["WhsCode"].ToString(),
                            FBinEntry = Convert.ToInt32(row["FromBinEntry"].ToString()),
                            FBinCode = row["BinCode"].ToString(),

                            TWhsCode = row["FromWhsCod"].ToString(),
                            TBinEntry = Convert.ToInt32(row["ToBinEntry"].ToString()),
                            TBinCode = row["ToBinCode"].ToString(),

                            BatchNumber = row["BatchNumber"].ToString(),
                            SerialNumber = row["SerialNumber"].ToString(),

                        });
                    }
                    return Task.FromResult(new ResponseScanItemsInIM
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = dataLine.ToList()
                    });
                }

                return Task.FromResult(new ResponseScanItemsInIM
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResponseScanItemsInIM
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }

        public Task<ResponseGetListItemInIM> responseGetListItemInIM(string docEntry)
        {

            var dataLine = new List<GetItemsImLine>();
            var dt = new DataTable();

            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {

                    var Query = $"CALL \"{ConnectionString.CompanyDB}\"._USP_CALLTRANS_BANK ('GetListItemInIM','{docEntry}','','','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        dataLine.Add(new GetItemsImLine
                        {
                            DocEntry = row["DocEntry"].ToString(),
                            ItemCode = row["ItemCode"].ToString(),
                            ItemName = row["Dscription"].ToString(),
                            Quantity = Convert.ToDouble(row["Quantity"].ToString()),
                            UOMCode = row["UomCode"].ToString(),

                            FWhsCode = row["WhsCode"].ToString(),
                            FBinEntry = row["FromBinEntry"].ToString(),
                            FBinCode = row["BinCode"].ToString(),

                            TWhsCode = row["FromWhsCod"].ToString(),
                            TBinEntry = row["ToBinEntry"].ToString(),
                            TBinCode = row["ToBinCode"].ToString(),

                            BatchNumber = row["BatchNumber"].ToString(),
                            SerialNumber = row["SerialNumber"].ToString(),

                            QtyByBatch = Convert.ToDouble(row["QtyByBatch"].ToString()),
                            QtyBySerial = Convert.ToDouble(row["QtyBySerial"].ToString())
                        });
                    }
                    return Task.FromResult(new ResponseGetListItemInIM
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = dataLine.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetListItemInIM
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetListItemInIM
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }

        public Task<ResponseGetStockItemBatchAndSerial> responseGetItemByBarcode(string barCode, string itemCode)
        {

            var dataLine = new List<GetStockItemBatchAndSerial>();
            var dt = new DataTable();

            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {

                    var Query = $"CALL \"{ConnectionString.CompanyDB}\"._USP_CALLTRANS_BANK ('GetBatchNumber','{barCode}','{itemCode}','','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);

                    if (dt.Rows.Count > 0)
                    {

                        foreach (DataRow row in dt.Rows)
                        {
                            dataLine.Add(new GetStockItemBatchAndSerial
                            {

                                ItemCode = row["ItemCode"].ToString(),
                                ItemName = row["ItemName"].ToString(),
                                Quantity = Convert.ToDouble(row["Qty"].ToString()),
                                UOMCode = row["UOMCode"].ToString(),
                                WhsEntry = Convert.ToInt32(row["WhsEntry"].ToString()),
                                WhsCode = row["WhsCode"].ToString(),
                                BinEntry = Convert.ToInt32(row["BinEntry"].ToString()),
                                BinCode = row["BinCode"].ToString(),
                                BatchNumber = row["BatchNumber"].ToString(),
                                ExpDate = row["ExpDate"].ToString(),
                                FDA = row["FDA"].ToString()

                            });
                        }

                    }
                    else
                    {
                        var Query2 = $"CALL \"{ConnectionString.CompanyDB}\"._USP_CALLTRANS_BANK ('GetSerialNumber','{barCode}','{itemCode}','','','')";
                        login.AD = new OdbcDataAdapter(Query2, login.CN);
                        login.AD.Fill(dt);

                        foreach (DataRow row in dt.Rows)
                        {
                            dataLine.Add(new GetStockItemBatchAndSerial
                            {

                                ItemCode = row["ItemCode"].ToString(),
                                ItemName = row["ItemName"].ToString(),
                                Quantity = Convert.ToDouble(row["Qty"].ToString()),
                                UOMCode = row["UOMCode"].ToString(),
                                WhsEntry = Convert.ToInt32(row["WhsEntry"].ToString()),
                                WhsCode = row["WhsCode"].ToString(),
                                BinEntry = Convert.ToInt32(row["BinEntry"].ToString()),
                                BinCode = row["BinCode"].ToString(),
                                SerialNumber = row["SerialNumber"].ToString(),
                                ExpDate = row["ExpDate"].ToString(),
                                FDA = row["FDA"].ToString()

                            });
                        }

                    }

                    


                    return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = dataLine.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }

        public Task<ResponseGetStockItemBatchAndSerial> responseGetItemNoBatchSerial(string itemCode)
        {

            var dataLine = new List<GetStockItemBatchAndSerial>();
            var dt = new DataTable();

            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {

                    var Query = $"CALL \"{ConnectionString.CompanyDB}\"._USP_CALLTRANS_BANK ('GetItemNoBatchSerial','','{itemCode}','','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        dataLine.Add(new GetStockItemBatchAndSerial
                        {

                            ItemCode = row["ItemCode"].ToString(),
                            ItemName = row["ItemName"].ToString(),
                            Quantity = Convert.ToDouble(row["Qty"].ToString()),
                            UOMCode = row["UOMCode"].ToString(),
                            WhsEntry = Convert.ToInt32(row["WhsEntry"].ToString()),
                            WhsCode = row["WhsCode"].ToString(),
                            BinEntry = Convert.ToInt32(row["BinEntry"].ToString()),
                            BinCode = row["BinCode"].ToString(),
                            ExpDate = row["ExpDate"].ToString(),
                            FDA = row["FDA"].ToString()

                        });
                    }
                    return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = dataLine.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }

        public Task<ResponseGetOUOM> responseGetOUOM()
        {

            var dataLine = new List<OUOMList>();
            var dt = new DataTable();

            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {

                    var Query = $"CALL \"{ConnectionString.CompanyDB}\"._USP_CALLTRANS_BANK ('GetPriceUnit','','','','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        dataLine.Add(new OUOMList
                        {
                            UomCode = row["UomName"].ToString()
                        });
                    }
                    return Task.FromResult(new ResponseGetOUOM
                    {
                        ErrorCode = 0,
                        ErrorMsg = "",
                        Data = dataLine.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetOUOM
                {
                    ErrorCode = login.lErrCode,
                    ErrorMsg = login.sErrMsg,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetOUOM
                {
                    ErrorCode = ex.HResult,
                    ErrorMsg = ex.Message,
                    Data = null
                });
            }
        }

        public Task<ResponseGetOUOM> responseGetOUOM2()
        {
            var dataLine = new List<OUOMList>();
            var dt = new DataTable();

            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {

                    var Query = $"CALL \"{ConnectionString.CompanyDB}\"._USP_CALLTRANS_BANK ('GetPriceUnit2','','','','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        dataLine.Add(new OUOMList
                        {
                            UomCode = row["UomName"].ToString()
                        });
                    }
                    return Task.FromResult(new ResponseGetOUOM
                    {
                        ErrorCode = 0,
                        ErrorMsg = "",
                        Data = dataLine.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetOUOM
                {
                    ErrorCode = login.lErrCode,
                    ErrorMsg = login.sErrMsg,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetOUOM
                {
                    ErrorCode = ex.HResult,
                    ErrorMsg = ex.Message,
                    Data = null
                });
            }
        }

        public Task<ResponseGetStockItemBatchAndSerial> responseGetItemByBinCode(string itemCode, string binCode)
        {

            var dataLine = new List<GetStockItemBatchAndSerial>();
            var dt = new DataTable();

            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {

                    var Query = $"CALL \"{ConnectionString.CompanyDB}\"._USP_CALLTRANS_BANK ('GetItemByBinCode','{itemCode}','{binCode}','','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        dataLine.Add(new GetStockItemBatchAndSerial
                        {

                            ItemCode = row["ItemCode"].ToString(),
                            ItemName = row["ItemName"].ToString(),
                            Quantity = Convert.ToDouble(row["Qty"].ToString()),
                            UOMCode = row["UOMCode"].ToString(),
                            WhsEntry = Convert.ToInt32(row["WhsEntry"].ToString()),
                            WhsCode = row["WhsCode"].ToString(),
                            BinEntry = Convert.ToInt32(row["BinEntry"].ToString()),
                            BinCode = row["BinCode"].ToString(),
                            ExpDate = row["ExpDate"].ToString(),
                            FDA = row["FDA"].ToString()

                        });
                    }
                    return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = dataLine.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }

        public Task<ResponseGetStockItemBatchAndSerial> responseGetItemByWhs(string itemCode, string whsCode)
        {

            var dataLine = new List<GetStockItemBatchAndSerial>();
            var dt = new DataTable();

            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {

                    var Query = $"CALL \"{ConnectionString.CompanyDB}\"._USP_CALLTRANS_BANK ('GetItemByWhs','{itemCode}','{whsCode}','','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        dataLine.Add(new GetStockItemBatchAndSerial
                        {
                            ItemCode = row["ItemCode"].ToString(),
                            ItemName = row["ItemName"].ToString(),
                            Quantity = Convert.ToDouble(row["Qty"].ToString()),
                            UOMCode = row["UOMCode"].ToString(),
                            WhsEntry = Convert.ToInt32(row["WhsEntry"].ToString()),
                            WhsCode = row["WhsCode"].ToString(),
                            BinEntry = Convert.ToInt32(row["BinEntry"].ToString()),
                            BinCode = row["BinCode"].ToString(),
                            ExpDate = row["ExpDate"].ToString(),
                            FDA = row["FDA"].ToString()

                        });
                    }
                    return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = dataLine.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetStockItemBatchAndSerial
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }

        public Task<ResponseGetListItemMaster> responseGetListItemMaster()
        {

            var dataLine = new List<lMaster>();
            var dt = new DataTable();

            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {

                    var Query = $"CALL \"{ConnectionString.CompanyDB}\"._USP_CALLTRANS_BANK ('GetListItemMaster','','','','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        dataLine.Add(new lMaster
                        {

                            ItemCode = row["ItemCode"].ToString(),
                            ItemName = row["ItemName"].ToString(),
                            UOM = row["UOM"].ToString(),
                            Quantity = Convert.ToDouble(row["Quantity"].ToString()),
                            BatchYN = row["BatchYN"].ToString(),
                            SerialYN = row["SerialYN"].ToString(),
                            Status = row["Status"].ToString(),

                        });
                    }
                    return Task.FromResult(new ResponseGetListItemMaster
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = dataLine.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetListItemMaster
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetListItemMaster
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }
    }
}
