﻿using System.Threading.Tasks;
using BarCodeAPIService.Service;
using Microsoft.AspNetCore.Mvc;

namespace BarCodeAPIService.Controllers
{
    public class InventoryTransferRequestLineController : ControllerBase
    {
        private readonly IInventoryTransferRequestService inventoryTransferRequestService;

        public InventoryTransferRequestLineController(IInventoryTransferRequestService inventoryTransferRequestService)
        {
            this.inventoryTransferRequestService = inventoryTransferRequestService;
        }

        [HttpGet("GetWTQLine/{DocEntry}")]
        public async Task<IActionResult> GetWTQLineAsync(int DocEntry)
        {
            var a = await inventoryTransferRequestService.responseGetWTQLine(DocEntry);
            if (a.ErrorCode == 0)
                return Ok(a);
            return BadRequest(a);
        }
    }
}