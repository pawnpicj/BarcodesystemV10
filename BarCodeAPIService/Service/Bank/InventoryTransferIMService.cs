using BarCodeAPIService.Connection;
using BarCodeLibrary.Respones.SAP;
using BarCodeAPIService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using System.Data;
using System.Data.Odbc;
using SAPbobsCOM;
using Microsoft.AspNetCore.Mvc;


namespace BarCodeAPIService.Service
{
    public class InventoryTransferIMService : IInventoryTransferIMService
    {
        public Task<ResponseGetOWTR> responseGetOWTR()
        {

            var dataLine = new List<OWTR>();
            var dt = new DataTable();

            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {

                    var Query = $"CALL \"{ConnectionString.CompanyDB}\"._USP_CALLTRANS_BANK ('OWTR-IM','','','','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        dataLine.Add(new OWTR
                        {

                            DocNum = row["DocNum"].ToString(),
                            DocEntry = row["DocEntry"].ToString(),
                            DocDate = row["DocDate"].ToString(),
                            CardCode = row["CardCode"].ToString(),
                            CardName = row["CardName"].ToString(),
                            SlpCode = row["SlpCode"].ToString(),
                            SlpName = row["SlpName"].ToString(),
                            FromWhs = row["FromWhs"].ToString(),
                            ToWhs = row["ToWhs"].ToString(),
                            SeriesName = row["SeriesName"].ToString(),
                            ToBinEntry = row["ToBinEntry"].ToString(),
                            ToBinCode = row["ToBinCode"].ToString(),
                            LoanNum = row["LoanNum"].ToString(),
                            ShipToCode = row["ShipToCode"].ToString(),
                            Address = row["Address"].ToString()
                        });
                    }
                    return Task.FromResult(new ResponseGetOWTR
                    {
                        ErrorCode = 0,
                        ErrorMsg = "",
                        Data = dataLine.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetOWTR
                {
                    ErrorCode = login.lErrCode,
                    ErrorMsg = login.sErrMsg,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetOWTR
                {
                    ErrorCode = ex.HResult,
                    ErrorMsg = ex.Message,
                    Data = null
                });
            }
        }

        public Task<ResponseGetWTRLine> responseGetWTRLine(int DocEntry)
        {
            var dataLine = new List<WTRLine>();
            var dt = new DataTable();

            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {
                    var Query = $"CALL \"{ConnectionString.CompanyDB}\"._USP_CALLTRANS_BANK ('WTR1-IM','{DocEntry}','','','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        dataLine.Add(new WTRLine
                        {
                            DocEntry = Convert.ToInt32(row["DocEntry"].ToString()),
                            LineNum = Convert.ToInt32(row["BaseLine"].ToString()),
                            ItemCode = row["ItemCode"].ToString(),
                            Dscription = row["Dscription"].ToString(),
                            Patient = row["Patient"].ToString(),
                            Quantity = Convert.ToDouble(row["Quantity"].ToString()),
                            InputQuantity = Convert.ToDouble(row["InputQuantity"].ToString()),
                            U_unitprice = Convert.ToDouble(row["U_unitprice"].ToString()),
                            UomCode = row["UomCode"].ToString(),
                            UomEntry = row["UomEntry"].ToString(),
                            unitMsr = row["unitMsr"].ToString(),
                            FromWhsCode = row["FromWhs"].ToString(),
                            FromBinCode = row["FromBinCode"].ToString(),
                            FromBinEntry = Convert.ToInt32(row["FromBinEntry"].ToString()),
                            ToWhsCode = row["ToWhs"].ToString(),
                            ToBinCode = row["ToBinCode"].ToString(),
                            ToBinEntry = Convert.ToInt32(row["ToBinEntry"].ToString()),

                            BatchYN = row["BatchYN"].ToString(),
                            SerialYN = row["SerialYN"].ToString()
                        });
                    }
                    return Task.FromResult(new ResponseGetWTRLine
                    {
                        ErrorCode = 0,
                        ErrorMsg = "",
                        Data = dataLine
                    });
                }
                else
                {
                    return Task.FromResult(new ResponseGetWTRLine
                    {
                        ErrorCode = login.lErrCode,
                        ErrorMsg = login.sErrMsg,
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetWTRLine
                {
                    ErrorCode = ex.HResult,
                    ErrorMsg = ex.Message,
                    Data = null
                });
            }

        }

        public Task<ResponseGetIMHeadLine> responseGetIMByCus(string cusCode)
        {
            var oWTRIM = new List<OWTRIM>();
            var wTR1IM = new List<WTR1IM>();
            var dt = new DataTable();
            var dtLine = new DataTable();
            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {
                    var Query =
                        $"CALL \"{ConnectionString.CompanyDB}\"._USP_CALLTRANS_BANK ('GetIMByCus','{cusCode}','','','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                    {
                        dtLine = new DataTable();
                        Query =
                            $"CALL \"{ConnectionString.CompanyDB}\"._USP_CALLTRANS_BANK ('GetIMLineDocEntry','{row["DocEntry"]}','','','','')";
                        login.AD = new OdbcDataAdapter(Query, login.CN);
                        login.AD.Fill(dtLine);
                        wTR1IM = new List<WTR1IM>();
                        foreach (DataRow drLine in dtLine.Rows)
                            wTR1IM.Add(new WTR1IM
                            {
                                //Line
                                DocEntry = Convert.ToInt32(drLine["DocEntry"].ToString()),
                                LineNum = Convert.ToInt32(drLine["LineNum"].ToString()),
                                ItemCode = drLine["ItemCode"].ToString(),
                                Dscription = drLine["Dscription"].ToString(),
                                Quantity = Convert.ToDouble(drLine["Quantity"].ToString()),
                                Price = Convert.ToDouble(drLine["Price"].ToString()),
                                PriceBefDi = Convert.ToDouble(drLine["PriceBefDi"].ToString()),
                                WhsCode = drLine["WhsCode"].ToString(),
                                BinEntry = Convert.ToInt32(drLine["BinEntry"].ToString()),
                                BinCode = drLine["FisrtBin"].ToString(),
                                LineTotal = Convert.ToDouble(drLine["LineTotal"].ToString()),
                                UomEntry = Convert.ToInt32(drLine["UomEntry"].ToString()),
                                UomCode = drLine["UomCode"].ToString(),
                                FisrtBin = drLine["FisrtBin"].ToString(),
                                IsBtchSerNum = drLine["IsBtchSerNum"].ToString(),
                                BatchSerialNumber = drLine["BatchSerialNumber"].ToString(),
                                BatchSerialQty = Convert.ToDouble(drLine["BatchSerialQty"].ToString()),
                                Patient = drLine["Patient"].ToString()

                            });
                        oWTRIM.Add(new OWTRIM
                        {
                            //Head
                            DocNum = row["DocNum"].ToString(),
                            DocEntry = Convert.ToInt32(row["DocEntry"].ToString()),
                            DocDate = row["DocDate"].ToString(),
                            CardCode = row["CardCode"].ToString(),
                            CardName = row["CardName"].ToString(),
                            SlpCode = row["SlpCode"].ToString(),
                            SlpName = row["SlpName"].ToString(),
                            FromWhs = row["FromWhs"].ToString(),
                            ToWhs = row["ToWhs"].ToString(),
                            SeriesName = row["SeriesName"].ToString(),
                            ToBinEntry = Convert.ToInt32(row["ToBinEntry"].ToString()),
                            ToBinCode = row["ToBinCode"].ToString(),
                            LoanNum = row["LoanNum"].ToString(),
                            Patient = row["Patient"].ToString(),
                            Line = wTR1IM.ToList()
                        });
                    }

                    return Task.FromResult(new ResponseGetIMHeadLine
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = oWTRIM.ToList()
                    });
                }

                return Task.FromResult(new ResponseGetIMHeadLine
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }

            catch (Exception ex)
            {
                return Task.FromResult(new ResponseGetIMHeadLine
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }

        public Task<ResponseIMReport> responseIMReport(string fromDate, string toDate, string customer, string saleEmp)
        {
            var oWTRIM = new List<RPT_OWTRIM>();
            var dt = new DataTable();
            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {

                    var aFromDate = "";
                    var aToDate = "";
                    var cFromDate = "";
                    var cToDate = "";

                    aFromDate = fromDate;
                    aToDate = toDate;

                    var fromDay = aFromDate.Substring(0, 2);
                    var fromMonth = aFromDate.Substring(2, 2);
                    var fromYear = aFromDate.Substring(4, 4);

                    var toDay = aToDate.Substring(0, 2);
                    var toMonth = aToDate.Substring(2, 2);
                    var toYear = aToDate.Substring(4, 4);

                    cFromDate = fromYear + "-" + fromMonth + "-" + fromDay;
                    cToDate = toYear + "-" + toMonth + "-" + toDay;

                    var cCustomer = "";
                    var cSaleEmp = "";
                    if (customer == "" || customer == "empty")
                    {
                        cCustomer = "empty";
                    }
                    else
                    {
                        cCustomer = customer;
                    }


                    if (saleEmp == "" || saleEmp == "empty")
                    {
                        cSaleEmp = "empty";
                    }
                    else
                    {
                        cSaleEmp = saleEmp;
                    }

                    int lineItem = 0;

                    var Query =
                        $"CALL \"{ConnectionString.CompanyDB}\"._USP_CALLTRANS_BANK ('RptTransferIM','{cFromDate}','{cToDate}','{cCustomer}','{cSaleEmp}','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);

                    foreach (DataRow row in dt.Rows)
                    {
                        string strRemark = "";
                        string BatchSerialNumber = "";
                        double BalanceByBS = 0;
                        double iBalance = 0;
                        string prDocNum = "";
                        string prItemCode = "";
                        string prIsBtchSerNum = "";

                        prDocNum = row["DocNum"].ToString();
                        prItemCode = row["ItemCode"].ToString();
                        if (row["IsBtchSerNum"].ToString() == "B")
                        {
                            prIsBtchSerNum = row["BatchesNumber"].ToString();
                        }
                        else if (row["IsBtchSerNum"].ToString() == "S")
                        {
                            prIsBtchSerNum = row["SerialNumber"].ToString();
                        }
                        else if (row["IsBtchSerNum"].ToString() == "N")
                        {
                            prIsBtchSerNum = "";
                        }
                        if (prDocNum == "122050031" && prItemCode == "0102Z" && prIsBtchSerNum == "PDWPU210401")
                        {
                            Console.WriteLine("DocNum : " + prDocNum + " ItemCode : " + prItemCode + " ProductNo : " + prIsBtchSerNum);
                        }
                        else
                        {
                            Console.WriteLine("DocNum : " + prDocNum + " ItemCode : " + prItemCode + " ProductNo : " + prIsBtchSerNum);
                        }

                        double QtyY = double.Parse(row["QtyBSNo"].ToString());
                        double QtyCvY = double.Parse(row["QTYCV"].ToString());
                        double QtyNotifyY = double.Parse(row["QTYNotify"].ToString());
                        double CalcQtyBS = QtyY - (QtyCvY + QtyNotifyY);

                        double QtyX = double.Parse(row["Quantity"].ToString());
                        double QtyCv = double.Parse(row["QTYCV_All"].ToString());
                        double QtyNotify = double.Parse(row["QTYNotify_All"].ToString());
                        double CalcQty = QtyX - (QtyCv + QtyNotify);

                        string UseBaseUn = row["UseBaseUn"].ToString();

                        if (row["YNCV"].ToString() == "Y" || row["YNNotify"].ToString() == "Y")
                        {
                            //str = Convert.ToString(QtyX) + "-(" + Convert.ToString(QtyCv) + "+" + Convert.ToString(QtyNotify) + ") = " + CalcQty + " | " + row["YNCV"].ToString() + "/" + row["YNNotify"].ToString();
                            strRemark = "QtyX:- " + CalcQty + " QtyY:- " + CalcQtyBS + " UseBaseUn:- " + UseBaseUn;
                        }
                        else
                        {
                            //str = Convert.ToString(QtyX) + "-(" + Convert.ToString(QtyCv) + "+" + Convert.ToString(QtyNotify) + ") = " + CalcQty + " | " + row["YNCV"].ToString() + "/" + row["YNNotify"].ToString();
                            strRemark = "QtyX:- " + CalcQty + " QtyY:- " + CalcQtyBS + " UseBaseUn:- " + UseBaseUn;
                        }

                        string dShowHide = "N";
                        string rYNCV = row["YNCV"].ToString();
                        string rYNNotify = row["YNNotify"].ToString();
                        if (rYNNotify == "Y" && QtyNotifyY == 0)
                        {
                            dShowHide = "E";
                        }
                        else if (rYNCV == "Y" && QtyCvY == 0)
                        {
                            dShowHide = "E";
                        }


                        if (row["IsBtchSerNum"].ToString() == "B")
                        {
                            //CalcQtyBS = CalcQtyBS;
                            if (CalcQty == 0)
                            {
                                iBalance = CalcQty;
                            }
                            else
                            {
                                iBalance = CalcQtyBS;
                            }

                            BatchSerialNumber = row["BatchesNumber"].ToString();
                        }
                        else if (row["IsBtchSerNum"].ToString() == "S")
                        {
                            //CalcQtyBS = CalcQtyBS;
                            iBalance = CalcQtyBS;
                            BatchSerialNumber = row["SerialNumber"].ToString();
                        }
                        else if (row["IsBtchSerNum"].ToString() == "N")
                        {
                            //CalcQtyBS = 0;
                            iBalance = CalcQty;
                            BatchSerialNumber = "";
                        }

                        if (iBalance != 0)
                        {
                            if (dShowHide != "E")
                            {
                                //Data Line
                                oWTRIM.Add(new RPT_OWTRIM
                                {
                                    //Head
                                    CardCode = row["CardCode"].ToString(),
                                    CardName = row["CardName"].ToString(),
                                    DocEntry = Convert.ToInt32(row["DocEntry"].ToString()),
                                    DocNum = "IM " + row["DocNum"].ToString(),
                                    DocDate = row["DocDate"].ToString(),
                                    FisrtBin = row["FisrtBin"].ToString(),
                                    ItemCode = row["ItemCode"].ToString(),
                                    Dscription = row["Dscription"].ToString(),
                                    IsBtchSerNum = row["IsBtchSerNum"].ToString(),
                                    BatchSerialNumber = BatchSerialNumber,
                                    ExpDate = row["ExpDate"].ToString(),
                                    Quantity = Convert.ToDouble(row["Quantity"].ToString()),
                                    UomCode = row["UomCode"].ToString(),
                                    Price = Convert.ToDouble(row["Price"].ToString()),
                                    DocTotal = Convert.ToDouble(row["Price"].ToString()) * iBalance,
                                    //DocTotal = Convert.ToDouble(row["DocTotal"].ToString()),
                                    Balance = iBalance,
                                    BalanceByBS = BalanceByBS,
                                    SlpCode = row["SlpCode"].ToString(),
                                    SlpName = row["SlpName"].ToString(),
                                    Remark = strRemark.ToString(),
                                    QTYByBatchSerial = Convert.ToDouble(row["QtyBSNo"].ToString())
                                });
                            }

                        }

                    }

                    return Task.FromResult(new ResponseIMReport
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = oWTRIM.ToList()
                    });
                }

                return Task.FromResult(new ResponseIMReport
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }

            catch (Exception ex)
            {
                return Task.FromResult(new ResponseIMReport
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }
    }
}
