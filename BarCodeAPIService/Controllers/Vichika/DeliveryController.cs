using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Threading.Tasks;

using BarCodeAPIService.Connection;
using BarCodeAPIService.Models;
using BarCodeAPIService.Service;

using BarCodeLibrary.Request.SAP;
using BarCodeLibrary.Respones.SAP;

using Barcodesystem.Contract.RouteApi;
using Microsoft.AspNetCore.Mvc;

using SAPbobsCOM;



namespace BarCodeAPIService.Controllers
{
    [ApiController]
    [Route(APIRoute.Root)]
    public class DeliveryController : ControllerBase
    {
        private readonly IDeliveryService Delivery;

        public DeliveryController(IDeliveryService Delivery)
        {
            this.Delivery = Delivery;
        }

        [HttpGet(APIRoute.Delivery.GetSO+ "{cardCode}/{typeShow}")]
        public async Task<IActionResult> GetDeliveryAsyc(string cardCode, string typeShow)
        {
            var a = await Delivery.responseGetORDR(cardCode, typeShow);
            if (a.ErrorCode == 0)
                return Ok(a);
            return BadRequest(a);
        }

        [HttpGet(APIRoute.Delivery.GetBatch + "{itemCode}/{WhsCode}")]
        public async Task<IActionResult> GetBatchAsync(string itemCode,string WhsCode)
        {
            var a = await Delivery.responseGetBatch(itemCode,WhsCode);
            if (a.ErrorCode == 0)
                return Ok(a);
            return BadRequest(a);
        }

        [HttpGet(APIRoute.Delivery.GetSerial + "{itemCode}/{WhsCode}")]
        public async Task<IActionResult> GetSerialAsyc(string itemCode, string WhsCode)
        {
            var a = await Delivery.responseGetSerial(itemCode, WhsCode);
            if (a.ErrorCode == 0)
                return Ok(a);
            return BadRequest(a);
        }

        [HttpPost(APIRoute.Delivery.POSTDelivery)]
        public async Task<IActionResult> PostDeliveryAsync(SendDelivery sendDelivery)
        {
            //SAPbobsCOM.Company oCompany;
            var a = await Delivery.PostDelivery(sendDelivery);
            if (a.ErrorCode == 0)
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                string DBName = ConnectionString.CompanyDB;

                string Fld1 = "DocEntry";
                string Fld2 = "LineNum";
                string Fld3 = "ItemCode";
                string Fld4 = "PriceAfVAT";
                string Fld5 = "GPBefDisc";
                string Fld6 = "Price";
                string Fld7 = "VatPrcnt";
                string Fld8 = "Quantity";

                string iDocEntry = a.DocEntry;
                //string iDocEntry = "92560";

                string PRODUCTS_TABLE = "DLN1";
                var dt = new DataTable();
                var dtH = new DataTable();
                string strSQL = "SELECT \"" + Fld1 + "\", \"" + Fld2 + "\", \"" + Fld3 + "\", \"" + Fld4 + "\", \"" + Fld5 + "\", \"" + Fld6 + "\", \"" + Fld7 + "\", \"" + Fld8 + "\"" +
                    " FROM \"" + ConnectionString.CompanyDB + "\".\"" + PRODUCTS_TABLE + "\" " +
                    " WHERE \"" + Fld1 + "\" = " + iDocEntry + " ";
                login.AD = new OdbcDataAdapter(strSQL, login.CN);
                login.AD.Fill(dt);

                double BefPriceTotal = 0;
                double AftPriceTotal = 0;
                double TaxPriceTotal = 0;
                foreach (DataRow row in dt.Rows)
                {
                    string DocEntry = row["DocEntry"].ToString();
                    string LineNum = row["LineNum"].ToString();
                    string ItemCode = row["ItemCode"].ToString();
                    double PriceAfVAT = Convert.ToDouble(row["PriceAfVAT"].ToString());
                    double GPBefDisc = Convert.ToDouble(row["GPBefDisc"].ToString());
                    double Price = Convert.ToDouble(row["Price"].ToString());
                    double VatPrcnt = Convert.ToDouble(row["VatPrcnt"].ToString());
                    double Quantity = Convert.ToDouble(row["Quantity"].ToString());
                    //CalcVat
                    double vVat = VatPrcnt / 100;

                    string cPriceAfter = "";
                    cPriceAfter = string.Format("{0:F2}", PriceAfVAT);
                    double dblPriceAfter = System.Math.Round(Convert.ToDouble(cPriceAfter), 0);

                    double BefPriceLine = System.Math.Round(Price * Quantity, 2);
                    double AftPriceLine = System.Math.Round(dblPriceAfter * Quantity, 2);
                    double TaxPrice = Price * vVat;
                    double TaxPriceLine = System.Math.Round(TaxPrice * Quantity, 2);
                    //92567

                    BefPriceTotal = BefPriceTotal + BefPriceLine;
                    AftPriceTotal = AftPriceTotal + AftPriceLine;
                    TaxPriceTotal = TaxPriceTotal + TaxPriceLine;

                    string uLine = "UPDATE \"" + ConnectionString.CompanyDB + "\".\"" + PRODUCTS_TABLE + "\" SET " +
                                " \"" + Fld4 + "\" = " + dblPriceAfter + ", \"" + Fld5 + "\" = " + dblPriceAfter + "" +
                                " WHERE \"" + Fld1 + "\"=" + DocEntry + " AND \"" + Fld2 + "\"=" + LineNum + " AND \"" + Fld3 + "\"='" + ItemCode + "' ";
                    login.AD = new OdbcDataAdapter(uLine, login.CN);
                    login.AD.Fill(dt);
                }
                string PRODUCTS_TABLE2 = "ODLN";
                string FldH1 = "DocTotal";
                string FldH2 = "DocTotalSy";
                string FldH3 = "PaidSys";
                string FldH4 = "Max1099";

                string FldH5 = "VatSum";
                string FldH6 = "VatSumSy";
                string FldH7 = "VatPaidSys";

                string FldHDocEntry = "DocEntry";
                //VatSum, DocTotal

                string strHSQL = "SELECT \"DiscPrcnt\", \"DiscSum\"" +
                    " FROM \"" + ConnectionString.CompanyDB + "\".\"ODLN\"" +
                    " WHERE \"" + FldHDocEntry + "\" = " + iDocEntry + " ";
                login.AD = new OdbcDataAdapter(strHSQL, login.CN);
                login.AD.Fill(dtH);

                double submitTotal = 0;
                double cYBefPriceTotal = 0;
                double calcTaxTotal = 0;
                foreach (DataRow hr in dtH.Rows)
                {
                    string DiscPrcnt = hr["DiscPrcnt"].ToString();
                    string DiscSum = hr["DiscSum"].ToString();

                    double cDiscPrcnt = System.Math.Round(Convert.ToDouble(DiscPrcnt), 2);
                    double calcDiscPrcnt = cDiscPrcnt / 100;
                    double cXBefPriceTotal = System.Math.Round(BefPriceTotal * calcDiscPrcnt,2);
                    cYBefPriceTotal = System.Math.Round(BefPriceTotal - cXBefPriceTotal,2);

                    calcTaxTotal = System.Math.Round(cYBefPriceTotal * 0.07, 2);
                }
                //149.26
                submitTotal = cYBefPriceTotal + calcTaxTotal;
                submitTotal = System.Math.Round(submitTotal, 0);
                submitTotal = System.Math.Round(submitTotal, 2);

                string uHead = "UPDATE \"" + ConnectionString.CompanyDB + "\".\"" + PRODUCTS_TABLE2 + "\" SET " +
                               " \"" + FldH1 + "\" = " + submitTotal + ", " +
                               " \"" + FldH2 + "\" = " + submitTotal + ", " +
                               " \"" + FldH3 + "\" = " + submitTotal + ", " +
                               " \"" + FldH4 + "\" = " + submitTotal + ", " +
                               " \"" + FldH5 + "\" = " + calcTaxTotal + ", " +
                               " \"" + FldH6 + "\" = " + calcTaxTotal + ", " +
                               " \"" + FldH7 + "\" = " + calcTaxTotal + " " +
                                " WHERE \"" + FldHDocEntry + "\"=" + iDocEntry + "";
                //login.AD = new OdbcDataAdapter(uHead, login.CN);
                //login.AD.Fill(dt);

                return Ok(a);
            }
            else
            {
                return BadRequest(a);
            }                
        }

        [HttpGet(APIRoute.Delivery.GetSaleOrderList)]
        public async Task<IActionResult> GetSaleOrderListAsync()
        {
            var a = await Delivery.responseGetSaleOrder();
            if (a.ErrorCode == 0)
                return Ok(a);
            return BadRequest(a);
        }

        [HttpGet("GetSODelivery/{cardCode}")]
        public async Task<IActionResult> GetSODeliveryAsyc(string cardCode)
        {
            var a = await Delivery.responseGetSO(cardCode);
            if (a.ErrorCode == 0)
                return Ok(a);
            return BadRequest(a);
        }

        [HttpGet("GetSONofity/{cardCode}/{typeShow}")]
        public async Task<IActionResult> GetSONewAsyc(string cardCode, string typeShow)
        {
            var a = await Delivery.responseGetORDRNofity(cardCode, typeShow);
            if (a.ErrorCode == 0)
            {
                return Ok(a);
            }
            else
            {
                return BadRequest(a);
            }
        }

    }
}