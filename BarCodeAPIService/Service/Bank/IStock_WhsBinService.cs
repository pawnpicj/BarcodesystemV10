using BarCodeLibrary.Respones.SAP;
using BarCodeLibrary.Respones.SAP.Bank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarCodeAPIService.Service.Bank
{
    public interface IStock_WhsBinService
    {
        Task<ResponseGetStockByWhsBin> responseGetStockByWhsBin(string whsCode, string binCode);

        Task<ResponseGetStockItemBatchAndSerial> responseGetStockItemBatch(string itemCode, string batchNumber);
        Task<ResponseGetStockItemBatchAndSerial> responseGetStockItemSerial(string itemCode, string serialNumber);

        Task<ResponseGetStockItemBatchAndSerial> responseGetStockItemBatchBin(string itemCode, string batchNumber, string binEntry);
        Task<ResponseGetStockItemBatchAndSerial> responseGetStockItemSerialBin(string itemCode, string serialNumber, string binEntry);

        Task<ResponseGetStockItemBatchAndSerial> responseGetStockItemBatchW(string itemCode, string batchNumber, string whsCode);
        Task<ResponseGetStockItemBatchAndSerial> responseGetStockItemSerialW(string itemCode, string serialNumber, string whsCode);

        Task<ResponseGetStockItemBatchAndSerial> responseGetStockItemBatchWCounting(string itemCode, string batchNumber, string whsCode);
        Task<ResponseGetStockItemBatchAndSerial> responseGetStockItemSerialWCounting(string itemCode, string serialNumber, string whsCode);

        Task<ResponseScanItemsInIM> responseScanItemsInIM(string docEntry, string itemCode, string batchSerialNo);

        Task<ResponseGetListItemInIM> responseGetListItemInIM(string docEntry);

        Task<ResponseGetStockItemBatchAndSerial> responseGetItemByBarcode(string barCode, string itemCode);

        Task<ResponseGetStockItemBatchAndSerial> responseGetItemNoBatchSerial(string itemCode);
        
        Task<ResponseGetStockItemBatchAndSerial> responseGetItemByBinCode(string itemCode, string binCode);

        Task<ResponseGetStockItemBatchAndSerial> responseGetItemByWhs(string itemCode, string whsCode);

        Task<ResponseGetOUOM> responseGetOUOM();

        Task<ResponseGetOUOM> responseGetOUOM2();

        Task<ResponseGetListItemMaster> responseGetListItemMaster();
    }
}
