using BarCodeAPIService.Service;
using BarCodeLibrary.APICall;
using BarCodeLibrary.Request.SAP.Pannreaksmey;
using BarCodeLibrary.Respones.SAP.Pannreaksmey;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;

namespace BarCodeClientService.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserService user;

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UserSetup()
        {
            return View();
        }
        public IActionResult Authorize()
        {
            return View();
        }

        public IActionResult ProgramAndDriver()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PostUserAsync(SendUser send)
        {
            var a = API.PostWithReturn<ResponsePostUser>("api/User/PostUser", send);
            return Ok(a);
        }

        [HttpGet]
        public IActionResult GetUserLogin()
        {
            var a = API.Read<ResponseGetUser>("api/User/GetUserLogin");
            return Ok(a);
        }

        [HttpPost]
        public IActionResult PostUpdatePasswordAsync(SendUser send)
        {
            var a = API.PostWithReturn<ResponseGetUserX>("api/User/UpdatePasswordUser", send);
            return Ok(a);
        }

    }
}