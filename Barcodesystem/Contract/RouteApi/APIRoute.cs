namespace Barcodesystem.Contract.RouteApi
{
    public static class APIRoute
    {
        public const string Root = "api/[controller]";

        public static class Token
        {
            public const string Login = "login";
            public const string RefreshToken = "refresh";
        }

        public static class GoodReceiptPO
        {
            public const string Controller = "api/GoodsReceiptPO/";
            public const string GetCustomer = "GetCustomer/";
            public const string GetPO = "GetPO/";
            public const string GetGoodReturn = "GoodReturn/";
            public const string SendGoodReceiptPO = "SendGoodReceiptPO";
            public const string GetSeries = "Series/";
            public const string GetSaleEmployee = "SaleEmployee";
            public const string GetCurrency = "GetCurrency/";
            public const string GetGenerate_Serial_Batch = "GetSerialOrBatch";
            public const string GetItemCode = "GetItem";
            public const string GetTaxCode = "GetTaxCode";
            public const string GetWarehouse = "GetWarehouse";
            public const string GetUnitOfMeasure = "GetUOM/";
            public const string GetBatchGenerator = "GetBatchGen";
            public const string GetBarCodeItem = "GetBarCode/";
        }
        public static class GoodReturn
        {
            public const string Controller = "api/GoodsReturn/";
            public const string GetGoodReceiptPO = "GetGoodsReceiptPO/";
            public const string GetGoodReceiptPOByDocNum = "GetGoodsReceiptPOByDocNum/";
            public const string SendGoodsReturn = "SendGoodsReturn";
            public const string GetUnitOfMeasure = "GetUOM";
        }
        public static class Delivery
        {
            public const string Controller = "api/Delivery/";
            public const string GetSO = "GetSO/";
            public const string GetSONofity = "GetSONofity/";
            public const string GetBatch = "GetBatch/";
            public const string GetSerial = "GetSerial/";
            public const string POSTDelivery = "POSTDelivery";
            public const string GetSaleOrderList = "GetSaleOrderList/";
        }
        public static class Return
        {
            public const string Controller = "api/Return/";
            public const string GetDelivery = "GetDelivery1/";
            public const string GetDeliveryByDocNum = "GetDeliveryByDocNum/";
            public const string SendReturn = "SendReturn";
        }

        public static class InventoryCounting
        {
            //public const string Controller = "api/Return/";
            //public const string GetDelivery = "GetDelivery1/";
            //public const string GetDeliveryByDocNum = "GetDeliveryByDocNum/";
            //public const string SendReturn = "SendReturn";
        }
    }
}