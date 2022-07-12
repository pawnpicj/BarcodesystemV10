﻿using System.Threading.Tasks;
using BarCodeAPIService.Service;
using BarCodeLibrary.Request.SAP;
using Barcodesystem.Contract.RouteApi;
using Microsoft.AspNetCore.Mvc;

namespace BarCodeAPIService.Controllers
{
    [ApiController]
    [Route(APIRoute.Root)]
    public class InventoryTransferController : ControllerBase
    {
        private readonly IInventoryTransferService inventoryTransfer;

        public InventoryTransferController(IInventoryTransferService inventoryTransfer)
        {
            this.inventoryTransfer = inventoryTransfer;
        }

        [HttpPost("SendInventoryTransfer")]
        public async Task<IActionResult> PostInventoryTransferAsync(SendInventoryTransfer sendinventoryTransfer)
        {
            var a = await inventoryTransfer.responseInventoryTransfer(sendinventoryTransfer);
            if (a.ErrorCode == 0)
                return Ok(a);
            return BadRequest(a);
        }
    }
}