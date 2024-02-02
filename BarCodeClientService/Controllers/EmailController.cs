using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;
using BarCodeClientService.Models;

namespace BarCodeClientService.Controllers
{
    public class EmailController : Controller
    {
        public IActionResult SendFile()
        {
            return View();
        }

        public IActionResult FormEmail()
        {
            return View();
        }

        [HttpPost]
        public IActionResult index(Email em)
        {
            
            return View();
        }

    }
}
