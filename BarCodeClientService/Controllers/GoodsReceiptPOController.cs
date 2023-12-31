﻿using BarCodeLibrary.APICall;
using BarCodeLibrary.Respones.SAP;
using BarCodeLibrary.Respones.SAP.Tengkimleang;
using Barcodesystem.Contract.RouteApi;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BarCodeLibrary.Request.SAP.Tengkimleang;
using BarCodeLibrary.Request.SAP.TengKimleang;

namespace BarCodeClientService.Controllers
{
    public class GoodsReceiptPOController : Controller
    {
        #region  View

        public IActionResult CreatePO()
        {
            return View();
        }
        #endregion

        #region Method

        [HttpGet]
        public IActionResult GetCustomerClientResult(string cusType)
        {
            var a = API.Read<ResponseCustomerGet>(APIRoute.GoodReceiptPO.Controller + APIRoute.GoodReceiptPO.GetCustomer+cusType);
            if (a.ErrorCode == 0)
            {
                return Ok(a.Data);
            }
            else
            {
                return BadRequest(a.ErrorMessage);
            }
        }
        [HttpGet]
        public IActionResult GetPurchaseOrderByCardCode(string cardCode)
        {
            var a = API.Read<ResponseOPORGetPO>(APIRoute.GoodReceiptPO.Controller + APIRoute.GoodReceiptPO.GetPO + cardCode);
            if (a.ErrorCode == 0)
            {
                return Ok(a.Data);
            }
            else
            {
                return BadRequest(a.ErrorMessage);
            }
        }

        [HttpGet]
        public IActionResult GetGoodReturnByCardCode(string cardCode)
        {
            var a = API.Read<ResponseOPDNGetGoodReceipt>(APIRoute.GoodReturn.Controller + APIRoute.GoodReturn.GetGoodReceiptPO+cardCode);
            if (a.ErrorCode == 0)
            {
                return Ok(a.Data);
            }
            else
            {
                return BadRequest(a.ErrorMessage);
            }
        }

        [HttpGet]
        public IActionResult GetSeries(string objectCode, string dateOfMonth)
        {
            var a = API.Read<ResponseGetSeries>(APIRoute.GoodReceiptPO.Controller + APIRoute.GoodReceiptPO.GetSeries + objectCode + "/" + dateOfMonth);
            if (a.ErrorCode == 0)
            {
                return Ok(a.Data);
            }
            else
            {
                return BadRequest(a.ErrorMessage);
            }
        }
        [HttpGet]
        public IActionResult GetSaleEmployeeResult()
        {
            var a = API.Read<ResponseGetSaleEmployee>(APIRoute.GoodReceiptPO.Controller + APIRoute.GoodReceiptPO.GetSaleEmployee);
            if (a.ErrorCode == 0)
            {
                return Ok(a.Data);
            }
            else
            {
                return BadRequest(a.ErrorMessage);
            }
        }
        [HttpGet]
        public IActionResult GetCurrency(string cardCode)
        {
            var a = API.Read<ResponseGetCurrency>(APIRoute.GoodReceiptPO.Controller + APIRoute.GoodReceiptPO.GetCurrency + cardCode);
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
        public IActionResult GetSerialBatchResult(GetGenerateSerialBatchRequest generateSerialBatchRequest)
        {
            var a = API.PostWithReturn<ResponseGetGenerateBatchSerial>(APIRoute.GoodReceiptPO.Controller + APIRoute.GoodReceiptPO.GetGenerate_Serial_Batch
                                                                        , generateSerialBatchRequest);
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
        public IActionResult CreateGoodsReceiptPoResult(SendGoodReceiptPO goodReceiptPo)
        {
            var a = API.PostWithReturn<ResponseGoodReceiptPO>(APIRoute.GoodReceiptPO.Controller + APIRoute.GoodReceiptPO.SendGoodReceiptPO
                , goodReceiptPo);
            if (a != null)
            {
                return Ok(a.DocEntry);
            }
            return BadRequest(API.ErrorMessage);
        }

        [HttpGet]
        public IActionResult GetItemActionResult()
        {
            var a = API.Read<ResponseGetItemCode>(
                APIRoute.GoodReceiptPO.Controller + APIRoute.GoodReceiptPO.GetItemCode);
            if (a != null)
            {
                return Ok(a);
            }
            return BadRequest(API.ErrorMessage);
        }

        [HttpGet]
        public IActionResult GetTaxCodeResult()
        {
            var a = API.Read<ResponseGetVatCode>(APIRoute.GoodReceiptPO.Controller + APIRoute.GoodReceiptPO.GetTaxCode);
            if (a != null)
            {
                return Ok(a.Data);
            }
            return BadRequest(API.ErrorMessage);
        }
        [HttpGet]
        public IActionResult GetWarehouseResult()
        {
            var a = API.Read<ResponseGetVatCode>(APIRoute.GoodReceiptPO.Controller + APIRoute.GoodReceiptPO.GetWarehouse);
            if (a != null)
            {
                return Ok(a.Data);
            }
            return BadRequest(API.ErrorMessage);
        }

        [HttpGet]
        public IActionResult GetUomCodeResult(string ItemCode,string UOMType)
        {
            var a = API.Read<ResponseGetVatCode>(APIRoute.GoodReceiptPO.Controller + APIRoute.GoodReceiptPO.GetUnitOfMeasure + ItemCode+"/"+ UOMType);
            if (a != null)
            {
                return Ok(a.Data);
            }
            return BadRequest(API.ErrorMessage);
        }

        [HttpPost]
        public IActionResult GenerateBatchResult(GetBatchGenRequest generateSerialBatchRequest)
        {
            var a = API.PostWithReturn<ResponseGetGenerateBatchSerial>(APIRoute.GoodReceiptPO.Controller + APIRoute.GoodReceiptPO.GetBatchGenerator
                , generateSerialBatchRequest);
            if (a.ErrorCode == 0)
            {
                return Ok(a.Data);
            }
            else
            {
                return BadRequest(a.ErrorMessage);
            }
        }

        [HttpGet]
        public IActionResult GetBarCodeActionResult(string BarCode)
        {
            var a = API.Read<ResponseGetBarCode>(APIRoute.GoodReceiptPO.Controller + APIRoute.GoodReceiptPO.GetBarCodeItem + BarCode);
            if (a != null)
            {
                return Ok(a.Data);
            }
            return BadRequest(API.ErrorMessage);
        }

        #endregion

    }
}
