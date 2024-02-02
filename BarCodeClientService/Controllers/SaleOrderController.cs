using BarCodeClientService.Models;
using BarCodeLibrary.APICall;
using BarCodeLibrary.Request.SAP;
using BarCodeLibrary.Respones.SAP;
using BarCodeLibrary.Respones.SAP.Bank;
using BarCodeLibrary.Respones.SAP.Tengkimleang;
using Barcodesystem.Contract.RouteApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BarCodeLibrary.Request.SAP.Vichika;
using BarCodeLibrary.Respones.SAP.Pannreaksmey;
using BarCodeLibrary.Respones.SAP.Vichika;
using QRCoder;
using Rotativa.AspNetCore;

namespace BarCodeClientService.Controllers
{
    public class SaleOrderController : Controller
    {

        private readonly ILogger<SaleOrderController> _logger;

        public SaleOrderController(ILogger<SaleOrderController> logger)
        {
            _logger = logger;
        }

        public IActionResult CreateSaleOrder()
        {
            return View();
        }

        public IActionResult CreateDelivery()
        {
            return View();
        }
        public IActionResult CreateDeliveryNofity()
        {
            return View();
        }
        public IActionResult CreateBarCodeSaleOrder()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetSO(string cardCode, string typeShow)
        {
            //var a = API.Read<ResponseGetORDR>("api/Delivery/GetSO/" + cardCode + "/" + typeShow);
            var a = API.Read<ResponseGetORDR>(APIRoute.Delivery.Controller + APIRoute.Delivery.GetSO + cardCode + "/" + typeShow);
            if (a.ErrorCode!=0)
            {
                return BadRequest(a);
            }
            else
            {
                return Ok(a.Data);
            }
        }

        public IActionResult GetSONofity(string cardCode, string typeShow)
        {
            //var a = API.Read<ResponseGetORDR>("api/Delivery/GetSO/" + cardCode + "/" + typeShow);
            var a = API.Read<ResponseGetORDRNofity>(APIRoute.Delivery.Controller + APIRoute.Delivery.GetSONofity + cardCode + "/" + typeShow);
            if (a.ErrorCode != 0)
            {
                return BadRequest(a);
            }
            else
            {
                return Ok(a.Data);
            }
        }

        [HttpGet]
        public IActionResult GetBatchActionResult(string itemCode,string whsCode)
        {
            var a = API.Read<ResponseGetBatch>(APIRoute.Delivery.Controller + APIRoute.Delivery.GetBatch + itemCode + "/"+whsCode);
            if (a.ErrorCode != 0)
            {
                return BadRequest(a);
            }
            else
            {
                return Ok(a.Data);
            }
        }
        [HttpGet]
        public IActionResult GetSerialActionResult(string itemCode, string whsCode)
        {
            var a = API.Read<ResponseGetSerial>(APIRoute.Delivery.Controller + APIRoute.Delivery.GetSerial + itemCode + "/" + whsCode);
            if (a.ErrorCode != 0)
            {
                return BadRequest(a);
            }
            else
            {
                return Ok(a.Data);
            }
        }
        [HttpGet]
        public IActionResult GetSaleOrderListActionResult()
        {
            var a = API.Read<ResponseGetSaleOrder>(APIRoute.Delivery.Controller + APIRoute.Delivery.GetSaleOrderList);
            if (a.ErrorCode != 0)
            {
                return BadRequest(a);
            }
            else
            {
                return Ok(a.Data);
            }
        }
        [HttpGet]
        public IActionResult GenerateQRCodeSaleOrder(string DocNum)
        {
            var qRCodeGenerator = new QRCodeGenerator();
            var qRCodeData = qRCodeGenerator.CreateQrCode(DocNum, QRCodeGenerator.ECCLevel.Q);
            var qRCode = new QRCode(qRCodeData);
            var bitmap = qRCode.GetGraphic(15);
            var bitmapBytes = ConvertBitmapToBytes(bitmap);
            return File(bitmapBytes, "image/jpeg");
        }
        private byte[] ConvertBitmapToBytes(Bitmap bitmap)
        {
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Png);
                return ms.ToArray();
            }
        }
        [HttpPost]
        public IActionResult PrintBarCodeSaleOrderAction(CreateBarCodeSaleOrder print)
        {
            BarCodeSaleOrderStatic.Data = print.Data;
            return Ok(1);
        }
        public IActionResult PrintLableBarCodeSaleOrder()
        {
            CreateBarCodeSaleOrder responseBarCodeSaleOrder = new CreateBarCodeSaleOrder();
            responseBarCodeSaleOrder.Data = BarCodeSaleOrderStatic.Data;
            BarCodeSaleOrderStatic.Data = null;
            //4.5cm=170.0787401575px
            //7cm=264.5669291339px
            //6cm=226.7716535433px
            return new ViewAsPdf(responseBarCodeSaleOrder)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.Letter,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = new Rotativa.AspNetCore.Options.Margins(0, 0, 1, 0),
                PageWidth = 120,
                PageHeight = 90
                //CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 12"
            };
        }

        [HttpPost]
        public IActionResult PostDeliveryActionResult(SendDelivery sendDelivery)
        {
            //return Ok(sendDelivery);
            var a = API.PostWithReturn<ResponseDelivery>(APIRoute.Delivery.Controller + APIRoute.Delivery.POSTDelivery, sendDelivery);
            if (a.ErrorCode != 0)
            {
                return BadRequest(a);
            }
            else
            {
                return Ok(a);
            }
        }

        [HttpPost]
        public IActionResult PostDelivery(SendDelivery sendDelivery)
        {
            var a = API.PostWithReturn<ResponseDelivery>("api/Delivery/POSTDelivery", sendDelivery);
            if (a.ErrorCode != 0)
            {
                return BadRequest(a);
            }
            else
            {
                return Ok(a);
            }
        }

        //public IActionResult GetSeriesCode(string yyyy, string typeSeries)
        //{
        //    string yy = DateTime.Now.Year.ToString();
        //    string cmm;
        //    int mm = Convert.ToInt32(DateTime.Now.Month.ToString());

        //    if (mm < 10)
        //    {
        //        cmm = "0" + mm;
        //    }
        //    else
        //    {
        //        cmm = "" + mm;
        //    }

        //    string xyymm = yy + "-" + cmm;
        //    string xTypeSeries = typeSeries;
        //    //var a = API.Read<ResponseGetSeriesCode>("GetSeriesCode/"+ xYYYY + "/"+ xTypeSeries);
        //    //var a = API.Read<ResponseGetSeriesCode>("GetSeriesCode/2021/IC");
        //    var a = API.Read<ResponseGetSeriesCode>("api/SeriesCV/GetSeriesCode/" + xyymm + "/DE");

        //    return Ok(a);
        //}

        //public IActionResult ListSO()
        //{
        //    ResponseGetDataFromSO responseGetDataFromSO = new ResponseGetDataFromSO();
        //    responseGetDataFromSO.Data = SOStatic.Data;
        //    SOStatic.Data = null;
        //    return View(responseGetDataFromSO);
        //}

        //[HttpPost]
        //public IActionResult PostDataFromSO(ResponseGetDataFromSO sdata)
        //{
        //    SOStatic.Data = sdata.Data;
        //    return Ok(1);
        //}

        //public IActionResult frmDelivery()
        //{
        //    ResponseGetSOLine responseGetSOLine = new ResponseGetSOLine();
        //    responseGetSOLine.Data = SOLStatic.Data;
        //    SOLStatic.Data = null;
        //    return View(responseGetSOLine);
        //    //return View();
        //}

        //[HttpPost]
        //public IActionResult PostDataDelivery(ResponseGetSOLine sdata)
        //{
        //    SOLStatic.Data = sdata.Data;
        //    return Ok(1);
        //}

        //[HttpPost]
        //public IActionResult PostDeliveryToSAP(SendDelivery sendDelivery)
        //{
        //    var a = API.PostWithReturn<ResponseDelivery>("api/Delivery/SendDelivery", sendDelivery);
        //    return Ok(a);
        //}

    }
}
