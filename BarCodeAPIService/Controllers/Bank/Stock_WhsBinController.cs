using BarCodeAPIService.Service;
using BarCodeAPIService.Service.Pannreaksmey;
using BarCodeAPIService.Service.Bank;
using Barcodesystem.Contract.RouteApi;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCodeAPIService.Controllers.Bank
{
    public class Stock_WhsBinController : ControllerBase
    {
        private IStock_WhsBinService stockWhsBinService;

        public Stock_WhsBinController(IStock_WhsBinService stockWhsBinService)
        {
            this.stockWhsBinService = stockWhsBinService;
        }

        [HttpGet("GetStockWhsBin/{whsCode}/{binCode}")]
        public async Task<IActionResult> GetStockWhsBinAsync(string whsCode, string binCode)
        {
            var a = await stockWhsBinService.responseGetStockByWhsBin(whsCode, binCode);
            if (a.ErrorCode == 0)
            {
                return Ok(a);
            }
            else
            {
                return BadRequest(a);
            }
        }

        [HttpGet("GetStockItemBatchBin/{itemCode}/{batchNumber}/{binEntry}")]
        public async Task<IActionResult> GetStockItemBatchBinAsync(string itemCode, string batchNumber, string binEntry)
        {
            var a = await stockWhsBinService.responseGetStockItemBatchBin(itemCode, batchNumber, binEntry);
            if (a.ErrorCode == 0)
            {
                return Ok(a);
            }
            else
            {
                return BadRequest(a);
            }
        }

        [HttpGet("GetStockItemSerialBin/{itemCode}/{serialNumber}/{binEntry}")]
        public async Task<IActionResult> GetStockItemSerialBinAsync(string itemCode, string serialNumber, string binEntry)
        {
            var a = await stockWhsBinService.responseGetStockItemSerialBin(itemCode, serialNumber, binEntry);
            if (a.ErrorCode == 0)
            {
                return Ok(a);
            }
            else
            {
                return BadRequest(a);
            }
        }

        [HttpGet("GetStockItemBatch/{itemCode}/{batchNumber}")]
        public async Task<IActionResult> GetStockItemBatchAsync(string itemCode, string batchNumber)
        {
            var a = await stockWhsBinService.responseGetStockItemBatch(itemCode, batchNumber);
            if (a.ErrorCode == 0)
            {
                return Ok(a);
            }
            else
            {
                return BadRequest(a);
            }
        }

        [HttpGet("GetStockItemSerial/{itemCode}/{serialNumber}")]
        public async Task<IActionResult> GetStockItemSerialAsync(string itemCode, string serialNumber)
        {
            var a = await stockWhsBinService.responseGetStockItemSerial(itemCode, serialNumber);
            if (a.ErrorCode == 0)
            {
                return Ok(a);
            }
            else
            {
                return BadRequest(a);
            }
        }

        [HttpGet("GetStockItemBatchW/{itemCode}/{batchNumber}/{whsCode}")]
        public async Task<IActionResult> GetStockItemBatchWAsync(string itemCode, string batchNumber, string whsCode)
        {
            var a = await stockWhsBinService.responseGetStockItemBatchW(itemCode, batchNumber, whsCode);
            if (a.ErrorCode == 0)
            {
                return Ok(a);
            }
            else
            {
                return BadRequest(a);
            }
        }

        [HttpGet("GetStockItemBatchWCounting/{itemCode}/{batchNumber}/{whsCode}")]
        public async Task<IActionResult> GetStockItemBatchWCountingAsync(string itemCode, string batchNumber, string whsCode)
        {
            var a = await stockWhsBinService.responseGetStockItemBatchWCounting(itemCode, batchNumber, whsCode);
            if (a.ErrorCode == 0)
            {
                return Ok(a);
            }
            else
            {
                return BadRequest(a);
            }
        }

        [HttpGet("GetStockItemSerialW/{itemCode}/{serialNumber}/{whsCode}")]
        public async Task<IActionResult> GetStockItemSerialWAsync(string itemCode, string serialNumber, string whsCode)
        {
            var a = await stockWhsBinService.responseGetStockItemSerialW(itemCode, serialNumber, whsCode);
            if (a.ErrorCode == 0)
            {
                return Ok(a);
            }
            else
            {
                return BadRequest(a);
            }
        }

        [HttpGet("GetStockItemSerialWCounting/{itemCode}/{serialNumber}/{whsCode}")]
        public async Task<IActionResult> GetStockItemSerialWCountingAsync(string itemCode, string serialNumber, string whsCode)
        {
            var a = await stockWhsBinService.responseGetStockItemSerialWCounting(itemCode, serialNumber, whsCode);
            if (a.ErrorCode == 0)
            {
                return Ok(a);
            }
            else
            {
                return BadRequest(a);
            }
        }

        [HttpGet("GetStockItemx/{docEntry}/{itemCode}/{batchSerialNo}")]
        public async Task<IActionResult> GetStockItemxAsync(string docEntry, string itemCode, string batchSerialNo)
        {
            var a = await stockWhsBinService.responseScanItemsInIM(docEntry, itemCode, batchSerialNo);
            if (a.ErrorCode == 0)
            {
                return Ok(a);
            }
            else
            {
                return BadRequest(a);
            }
        }

        [HttpGet("GetListItemInIM/{docEntry}")]
        public async Task<IActionResult> GetListItemInIMAsync(string docEntry)
        {
            var a = await stockWhsBinService.responseGetListItemInIM(docEntry);
            if (a.ErrorCode == 0)
            {
                return Ok(a);
            }
            else
            {
                return BadRequest(a);
            }
        }

        [HttpGet("GetItemByBarcode/{barCode}/{itemCode}")]
        public async Task<IActionResult> GetItemByBarcodeAsync(string barCode, string itemCode)
        {
            var a = await stockWhsBinService.responseGetItemByBarcode(barCode, itemCode);
            if (a.ErrorCode == 0)
            {
                return Ok(a);
            }
            else
            {
                return BadRequest(a);
            }
        }

        //responseGetItemNoBatchSerial
        [HttpGet("GetItemNoBatchSerial/{itemCode}")]
        public async Task<IActionResult> GetItemNoBatchSerialAsync(string itemCode)
        {
            var a = await stockWhsBinService.responseGetItemNoBatchSerial(itemCode);
            if (a.ErrorCode == 0)
            {
                return Ok(a);
            }
            else
            {
                return BadRequest(a);
            }
        }

        [HttpGet("GetUOMList")]
        public async Task<IActionResult> GetUOMListAsync()
        {
            var a = await stockWhsBinService.responseGetOUOM();
            if (a.ErrorCode == 0)
            {
                return Ok(a);
            }
            else
            {
                return BadRequest(a);
            }
        }

        [HttpGet("GetUOMList2")]
        public async Task<IActionResult> GetUOMList2Async()
        {
            var a = await stockWhsBinService.responseGetOUOM2();
            if (a.ErrorCode == 0)
            {
                return Ok(a);
            }
            else
            {
                return BadRequest(a);
            }
        }

        [HttpGet("GetItemByBinCode/{itemCode}/{binCode}")]
        public async Task<IActionResult> GetItemByBinCodeAsync(string itemCode, string binCode)
        {
            var a = await stockWhsBinService.responseGetItemByBinCode(itemCode, binCode);
            if (a.ErrorCode == 0)
            {
                return Ok(a);
            }
            else
            {
                return BadRequest(a);
            }
        }

        [HttpGet("GetItemByWhs/{itemCode}/{whsCode}")]
        public async Task<IActionResult> GetItemByGetItemByWhsAsync(string itemCode, string whsCode)
        {
            var a = await stockWhsBinService.responseGetItemByWhs(itemCode, whsCode);
            if (a.ErrorCode == 0)
            {
                return Ok(a);
            }
            else
            {
                return BadRequest(a);
            }
        }

        [HttpGet("GetListItemMaster")]
        public async Task<IActionResult> GetListItemMasterAsync()
        {
            var a = await stockWhsBinService.responseGetListItemMaster();
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
