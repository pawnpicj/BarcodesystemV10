using System.Diagnostics;
using System.IO;
using BarCodeClientService.Models;
using BarCodeLibrary.APICall;
using BarCodeLibrary.Respones.SAP;
using BarCodeLibrary.Respones.SAP.Bank;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace BarCodeClientService.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult LoginAsync()
        {
            return View();
        }

        public IActionResult SaleARCreation()
        {
            return View();
        }

        public IActionResult InventoryCreation()
        {
            return View();
        }

        public IActionResult FrmCheckItem()
        {
            return View();
        }
        

        public IActionResult GetAPITesting()
        {
            var a = API.Read<ResponseOPDNGetGoodReceipt>("api/GoodsReturn/GetGoodsReceiptPO");
            return Ok(a);
        }

        public IActionResult GetContactPerson()
        {
            var a = API.Read<ResponseOCPRGetContactPerson>("api/ContactPerson");
            return Ok(a);
        }

        public IActionResult GetCustomerC()
        {
            var a = API.Read<ResponseOCPRGetContactPerson>("api/ContactPerson/GetCustomerC");
            return Ok(a);
        }

        public IActionResult GetSaleEmployee()
        {
            var a = API.Read<ResponseOSLPGetSalesEmployee>("api/SaleEmployee");
            return Ok(a);
        }

        public IActionResult GetWarehouse()
        {
            var a = API.Read<ResponseOWHSGetWarehouse>("api/Warehouse/GetWarehouse");
            return Ok(a);
        }

        public IActionResult GetBinLocation()
        {
            var a = API.Read<ResponseOBINGetBinCode>("api/BinCode/GetBinCode");
            return Ok(a);
        }

        public IActionResult hGetOWTQ()
        {
            var a = API.Read<ResponseGetOWTQ>("api/InventoryTransferRequest/GetIFR");
            return Ok(a);
        }

        public IActionResult GetDataConfig()
        {
            var a = API.Read<ResponseGetDataConfig>("/api/User/GetConfig");
            return Ok(a);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}