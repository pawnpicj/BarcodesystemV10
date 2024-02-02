using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using BarCodeClientService.Models;
using BarCodeLibrary.APICall;
using BarCodeLibrary.Request.SAP;
using BarCodeLibrary.Respones.SAP;
using BarCodeLibrary.Respones.SAP.Bank;
using BarCodeLibrary.Respones.SAP.Pannreaksmey;
using BarCodeLibrary.Respones.SAP.Tengkimleang;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Rotativa.AspNetCore;

namespace BarCodeClientService.Controllers
{
    public class InventoryController : Controller
    {
        private readonly ILogger<InventoryController> _logger;
        private IWebHostEnvironment Environment;
        public InventoryController(ILogger<InventoryController> logger, IWebHostEnvironment _environment)
        {
            _logger = logger;
            Environment = _environment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult CreateInventoryTransferBB()
        {
            return View();
        }

        public IActionResult CreateInventoryTransferRBB()
        {
            return View();
        }

        public IActionResult CreateInventoryCounting()
        {
            return View();
        }

        public IActionResult CreateSONotify()
        {
            return View();
        }
        public IActionResult formRptTransferIM()
        {
            return View();
        }

        public IActionResult Waiting()
        {
            return View();
        }

        [Route("Inventory/formRptTransferIMAuto/{mm?}/{yyyy?}")]
        public IActionResult formRptTransferIMAuto()
        {
            //Console.WriteLine();
            return View();
        }
        public IActionResult FrmPrintForTransfer()
        {
            return View();
        }
        public IActionResult HistoryReportIM()
        {
            ////string contentPath = this.Environment.ContentRootPath + "\\HistoryReport";
            //string pathHistoryReportIM = $"{Environment.ContentRootPath}\\HistoryReport\\";
            //string[] filePaths = Directory.GetFiles($"{Environment.ContentRootPath}\\HistoryReport\\");            
            ////Console.WriteLine(pathHistoryReportIM);
            //List<FileModel> files = new List<FileModel>();
            //foreach (string filePath in filePaths)
            //{
            //    files.Add(new FileModel { FileName = Path.GetFileName(filePath) });
            //}
            return View();
        }

        public IActionResult HistoryReportIM2()
        {
            Console.WriteLine("========= HistoryReportIM2 =========");
            string filePathsx = Path.Combine(this.Environment.WebRootPath, "HistoryReport/");
            //string[] filePaths = Directory.GetFiles($"{Environment.ContentRootPath}\\wwwroot\\HistoryReport\\");
            string[] filePaths = Directory.GetFiles(Path.Combine(this.Environment.WebRootPath, "HistoryReport/"));
            Console.WriteLine(filePathsx);
            List<FileModel> files = new List<FileModel>();
            foreach (string filePath in filePaths)
            {
                files.Add(new FileModel { FileName = Path.GetFileName(filePath) });
            }
            var json = System.Text.Json.JsonSerializer.Serialize(files);
            return Ok(json);
        }

        public FileResult DownloadFile(string fileName)
        {
            //Build the File Path.
            string filePathsx = Path.Combine(this.Environment.WebRootPath, "HistoryReport/");
            string path = filePathsx + fileName;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            return File(bytes, "application/octet-stream", fileName);
        }

        public IActionResult GetOWTQ()
        {
            var a = API.Read<ResponseGetOWTQ>("api/InventoryTransferRequest/GetIFR");
            return Ok(a);
        }

        public IActionResult GetOWTR()
        {
            var a = API.Read<ResponseGetOWTR>("api/InventoryTransferIM/GetIFIM");
            return Ok(a);
        }

        public IActionResult GetSaleEmployee()
        {
            var a = API.Read<ResponseOSLPGetSalesEmployee>("api/SaleEmployee");
            return Ok(a);
        }

        public IActionResult GetOBinByCode(string binCode) {
            //api/BinCode/GetBinCodeByCode/01-A01-01-A02
            var a = API.Read<ResponseOBINGetBinCode>("api/BinCode/GetBinCodeByCode/" + binCode);
            return Ok(a);
        }

        public IActionResult GetSeriesIM(string yymm, string typeSeries)
        {
            var yyyy = DateTime.Now.Year.ToString();
            string cmm;
            var mm = Convert.ToInt32(DateTime.Now.Month.ToString());

            if (mm < 10)
                cmm = "0" + mm;
            else
                cmm = "" + mm;

            var xyymm = yyyy + "-" + cmm;
            var xTypeSeries = typeSeries;
            //var a = API.Read<ResponseGetSeriesCode>("GetSeriesCode/"+ xYYYY + "/"+ xTypeSeries);
            //var a = API.Read<ResponseGetSeriesCode>("GetSeriesCode/2021/IC");
            var a = API.Read<ResponseGetSeriesCode>("api/SeriesCV/GetSeriesCode/" + xyymm + "/IM");

            return Ok(a);
        }

        public IActionResult GetSeriesCV(string yymm, string typeSeries)
        {
            var yyyy = DateTime.Now.Year.ToString();
            string cmm;
            var mm = Convert.ToInt32(DateTime.Now.Month.ToString());

            if (mm < 10)
                cmm = "0" + mm;
            else
                cmm = "" + mm;

            var xyymm = yyyy + "-" + cmm;
            var xTypeSeries = typeSeries;
            //var a = API.Read<ResponseGetSeriesCode>("GetSeriesCode/"+ xYYYY + "/"+ xTypeSeries);
            //var a = API.Read<ResponseGetSeriesCode>("GetSeriesCode/2021/IC");
            var a = API.Read<ResponseGetSeriesCode>("api/SeriesCV/GetSeriesCode/" + xyymm + "/CV");

            return Ok(a);
        }

        public IActionResult GetSeriesCode(string yymm, string typeSeries)
        {
            var yyyy = DateTime.Now.Year.ToString();
            string cmm;
            var mm = Convert.ToInt32(DateTime.Now.Month.ToString());

            if (mm < 10)
                cmm = "0" + mm;
            else
                cmm = "" + mm;

            var xyymm = yyyy + "-" + cmm;
            var xTypeSeries = typeSeries;
            //var a = API.Read<ResponseGetSeriesCode>("GetSeriesCode/"+ xYYYY + "/"+ xTypeSeries);
            //var a = API.Read<ResponseGetSeriesCode>("GetSeriesCode/2021/IC");
            var a = API.Read<ResponseGetSeriesCode>("api/SeriesCV/GetSeriesCode/" + xyymm + "/IC");

            return Ok(a);
        }
        
        public IActionResult GetSeriesSO(string yymm, string typeSeries)
        {
            var yyyy = DateTime.Now.Year.ToString();
            string cmm;
            var mm = Convert.ToInt32(DateTime.Now.Month.ToString());

            if (mm < 10)
                cmm = "0" + mm;
            else
                cmm = "" + mm;

            var xyymm = yyyy + "-" + cmm;
            var a = API.Read<ResponseGetSeriesCode>("api/SeriesCV/GetSeriesCode/" + xyymm + "/SO");

            return Ok(a);
        }

        public IActionResult GetWTQLine(string docentry)
        {
            var xDocEntry = docentry;
            var a = API.Read<ResponseGetWTQLine>("GetWTQLine/" + xDocEntry);
            return Ok(a);
        }

        public IActionResult GetBinLocationWhs(string whscode)
        {
            var xwhscode = whscode;
            var a = API.Read<ResponseGetBinLocation>("api/GetBinLocation/GetBinLocationWhs/" + xwhscode);
            return Ok(a);
        }


        public IActionResult GetBinLocationCounting(string whscode,string iyear)
        {
            //Case : RptBinLocationCounting
            var a = API.Read<ResponseGetBinLocation>("api/GetBinLocation/GetBinLocationCounting/" + whscode + "/" + iyear);
            return Ok(a);
        }

        //GetWTRLine
        public IActionResult GetWTRLine(string docentry)
        {
            var a = API.Read<ResponseGetWTRLine>("api/InventoryTransferIM/GetWTRLine/" + docentry);
            return Ok(a);
        }

        public IActionResult GetStcok_BatchSerial(string barcode, string batchserial)
        {
            var xBatchSerial = batchserial;
            var xScanBarcode = barcode;

            var a = API.Read<ResponseGetStockBatchSerial>("GetStcokBatchSerial/" + xScanBarcode + "/" + xBatchSerial + "/" + xBatchSerial);
            return Ok(a);
        }

        public IActionResult GetStock_WhsBin(string whsCode, string binCode)
        {
            var xWhsCode = whsCode;
            var xBinCode = binCode;

            var a = API.Read<ResponseGetStockByWhsBin>("GetStockWhsBin/" + xWhsCode + "/" + xBinCode);
            return Ok(a);
        }

        public IActionResult GetStockItemBatchSerial(string barcode, string batchserial)
        {
            var xScanBarcode = barcode;
            var xBatchSerial = batchserial;
            var a = API.Read<ResponseGetStockItemBatchSerial>("GetStockItemBatchSerial/" + xScanBarcode + "/" + xBatchSerial + "/" + xBatchSerial);
            return Ok(a);
        }

        public IActionResult GetStockItemBatch(string itemcode, string batchnumber)
        {
            var xitemcode = itemcode;
            var xbatchnumber = batchnumber;
            var a = API.Read<ResponseGetStockItemBatchSerial>("GetStockItemBatch/" + xitemcode + "/" + xbatchnumber);
            return Ok(a);
        }

        public IActionResult GetStockItemBatchBin(string itemcode, string batchnumber, string binentry)
        {
            var a = API.Read<ResponseGetStockItemBatchAndSerial>("GetStockItemBatchBin/" + itemcode + "/" + batchnumber + "/" + binentry);
            return Ok(a);
        }

        public IActionResult GetStockItemBatchW(string itemcode, string batchnumber, string whscode)
        {
            var a = API.Read<ResponseGetStockItemBatchAndSerial>("GetStockItemBatchW/" + itemcode + "/" + batchnumber + "/" + whscode);
            return Ok(a);
        }

        public IActionResult GetStockItemBatchWCounting(string itemcode, string batchnumber, string whscode)
        {
            var a = API.Read<ResponseGetStockItemBatchAndSerial>("GetStockItemBatchWCounting/" + itemcode + "/" + batchnumber + "/" + whscode);
            return Ok(a);
        }

        public IActionResult GetStockItemSerial(string itemcode, string serialnumber)
        {
            var xitemcode = itemcode;
            var xserialnumber = serialnumber;
            var a = API.Read<ResponseGetStockItemBatchSerial>("GetStockItemSerial/" + xitemcode + "/" + xserialnumber);
            return Ok(a);
        }

        public IActionResult GetStockItemSerialBin(string itemcode, string serialnumber, string binentry)
        {
            var a = API.Read<ResponseGetStockItemBatchAndSerial>("GetStockItemSerialBin/" + itemcode + "/" + serialnumber + "/" + binentry);
            return Ok(a);
        }

        public IActionResult GetStockItemSerialW(string itemcode, string serialnumber, string whscode)
        {
            var a = API.Read<ResponseGetStockItemBatchAndSerial>("GetStockItemSerialW/" + itemcode + "/" + serialnumber + "/" + whscode);
            return Ok(a);
        }

        public IActionResult GetStockItemSerialWCounting(string itemcode, string serialnumber, string whscode)
        {
            var a = API.Read<ResponseGetStockItemBatchAndSerial>("GetStockItemSerialWCounting/" + itemcode + "/" + serialnumber + "/" + whscode);
            return Ok(a);
        }

        public IActionResult GetStockItemx(string docentry, string itemcode, string batchserialno)
        {
            var a = API.Read<ResponseScanItemsInIM>("GetStockItemx/" + docentry + "/" + itemcode + "/" + batchserialno);
            //var a = API.Read<ResponseScanItemsInIM>("GetStockItemx/19414/KS-10/Ma21071660004");
            return Ok(a);
        }

        public IActionResult GetListItemInIM(string docentry)
        {
            var a = API.Read<ResponseGetListItemInIM>("GetListItemInIM/" + docentry );
            return Ok(a);
        }

        public IActionResult GetItemByBinCode(string itemCode, string binCode)
        {
            var a = API.Read<ResponseGetStockItemBatchSerial>("GetItemByBinCode/" + itemCode + "/" + binCode);
            return Ok(a);
        }

        public IActionResult GetItemByWhs(string itemcode, string whscode)
        {
            var a = API.Read<ResponseGetStockItemBatchSerial>("GetItemByWhs/" + itemcode + "/" + whscode);
            return Ok(a);
        }

        public IActionResult GetItemByBarcode(string barCode, string itemCode)
        {
            var a = API.Read<ResponseGetStockItemBatchAndSerial>("GetItemByBarcode/" + barCode + "/" + itemCode);
            return Ok(a);
        }

        public IActionResult GetItemNoBatchSerial(string itemCode)
        {
            var a = API.Read<ResponseGetStockItemBatchAndSerial>("GetItemNoBatchSerial/" + itemCode);
            return Ok(a);
        }

        //GetListItemMaster
        public IActionResult GetListItemMaster()
        {
            var a = API.Read<ResponseGetListItemMaster>("GetListItemMaster");
            return Ok(a);
        }

        //GetBinLocationList
        public IActionResult GetBinLocationList(string barcode, string itemcode)
        {
            var a = API.Read<ResponseGetBinLocationList>("api/GetBinLocationList/GetBinLocationList/" + barcode + "/" + itemcode);
            return Ok(a);
        }

        //Report Transfer IM
        [HttpGet]
        public IActionResult RptTransferIM(string fromDate, string toDate, string customer, string saleEmp)
        {
            var a = API.Read<ResponseIMReport>("api/InventoryTransferIM/RptTransferIM/" + fromDate + "/" + toDate + "/" + customer + "/" + saleEmp);
            if (a.ErrorCode == 0)
            {
                return Ok(a.Data);
            }
            else
            {
                return BadRequest(a.ErrorMessage);
            }
        }

        //Open Sales Order for Use Notify
        [HttpGet]
        public IActionResult GetIMByCus(string cusCode)
        {
            var a = API.Read<ResponseGetIMHeadLine>("api/InventoryTransferIM/GetIMByCus/" + cusCode);
            if (a.ErrorCode == 0)
            {
                return Ok(a.Data);
            }
            else
            {
                return BadRequest(a.ErrorMessage);
            }
        }

        [HttpPost]
        public IActionResult PrintItemLablePDFAction(ResponsePrintLableINF print)
        {
            PrintLableINFStatic.Data = print.Data;
            return Ok(1);
        }

        [HttpPost]
        public IActionResult PrintBinCAction(ResponsePrintBinC print)
        {
            PrintBinCStatic.Data = print.Data;
            return Ok(1);
        }

        public IActionResult PrintLableForTransfer()
        {
            ResponsePrintLableINF responsePrintLableINF = new ResponsePrintLableINF();
            responsePrintLableINF.Data = PrintLableINFStatic.Data;
            PrintItemLableStatic.Data = null;

            return new ViewAsPdf(responsePrintLableINF)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.Letter,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = new Rotativa.AspNetCore.Options.Margins(0, 0, 1, 0),
                PageWidth = 120,
                PageHeight = 90
            };
        }

        public IActionResult PrintListForTransfer()
        {
            ResponsePrintLableINF responsePrintLableINF = new ResponsePrintLableINF();
            responsePrintLableINF.Data = PrintLableINFStatic.Data;
            PrintItemLableStatic.Data = null;

            return new ViewAsPdf(responsePrintLableINF)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = new Rotativa.AspNetCore.Options.Margins(0, 0, 1, 0)
            };
        }

        public IActionResult PrintBinCounted()
        {
            ResponsePrintBinC responsePrintBinC = new ResponsePrintBinC();
            responsePrintBinC.Data = PrintBinCStatic.Data;
            PrintBinCStatic.Data = null;

            return new ViewAsPdf(responsePrintBinC)
            {
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait,
                PageMargins = new Rotativa.AspNetCore.Options.Margins(0, 0, 1, 0)
            };
        }        

        [HttpPost]
        public IActionResult PostInventoryTransfer(SendInventoryTransfer sendInventoryTransfer)
        {
            var a = API.PostWithReturn<ResponseInventoryTransfer>("api/InventoryTransfer/SendInventoryTransfer", sendInventoryTransfer);
            return Ok(a);
        }
        
        [HttpPost]
        public IActionResult PostInventoryTransferCV(SendInventoryTransfer sendInventoryTransfer)
        {
            var a = API.PostWithReturn<ResponseInventoryTransfer>("api/InventoryTransfer/SendInventoryTransferCV", sendInventoryTransfer);
            return Ok(a);
        }

        [HttpPost]
        public IActionResult PostInventoryCounting(SendInventoryCounting sendInventoryCounting)
        {
            var a = API.PostWithReturn<ResponseInventoryCounting>("api/InventoryCounting/SendInventoryCounting", sendInventoryCounting);
            return Ok(a);
        }

        [HttpPost]
        public IActionResult PostSalesOrderIM(SendSalesOrderForIM sendSalesOrderForIM)
        {
            var a = API.PostWithReturn<ResponseSalesOrder>("api/SalesOrderForIM/SendSalesOrderForIM", sendSalesOrderForIM);
            return Ok(a);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}