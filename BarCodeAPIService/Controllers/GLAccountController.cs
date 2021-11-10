﻿using BarCodeAPIService.Service;
using Barcodesystem.Contract.RouteApi;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCodeAPIService.Controllers
{
    [ApiController]
    [Route(APIRoute.Root)]
    public class GLAccountController : Controller
    {
        private readonly IGLAccountService gLAccount;

        public  GLAccountController(IGLAccountService gLAccount) {
            this.gLAccount = gLAccount;
        }
        [HttpGet]
        public async Task<IActionResult> GetGLAccountAsync() {
            var a = await gLAccount.ResponseOACTGetGLAccount();
            if (a.ErrorCode == 0) {
                return Ok(a);
            }
            else
            {
                return BadRequest(a);
            }
        }
    }
}
