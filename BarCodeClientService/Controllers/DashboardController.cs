using Microsoft.AspNetCore.Mvc;

namespace BarCodeClientService.Controllers
{
    public class DashboardController : Controller
    {
        public IActionResult DashboardSaleAR()
        {
            return View();
        }

        public IActionResult DashboardInventory()
        {
            return View();
        }

        public IActionResult DashboardPurchasingAP()
        {
            return View();
        }

        public IActionResult DashboardNofity()
        {
            return View();
        }
    }
}