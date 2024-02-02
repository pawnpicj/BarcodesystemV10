using System;
using System.Collections.Generic;

namespace BarCodeLibrary.Request.SAP
{
    public class SendDelivery
    {
        public string CardCode { get; set; }
        public string NumAtCard { get; set; }
        public DateTime DocDate { get; set; }
        public int ContactPersion { get; set; }
        public string Series { get; set; }
        public string CurrencyCode { get; set; }
        public string Remark { get; set; }
        public string SlpCode { get; set; }
        public double VatSum { get; set; }
        public double DiscountPercent { get; set; }
        public double DocTotal { get; set; }
        public string Sq_Remark { get; set; }
        public List<SendDeliveryLine> Lines { get; set; }
    }

    public class SendDeliveryLine
    {
        public string ItemCode { get; set; }
        public double Quantity { get; set; }
        public double Discount { get; set; }
        public string ManageItem { get; set; }
        public double PriceBeforeDis { get; set; }
        public double PriceAfterVAT { get; set; }
        public string Whs { get; set; }
        public int BinEntry { get; set; }
        public string UomName { get; set; }
        public string TaxCode { get; set; }
        public int DocEntry { get; set; }
        public string Patient { get; set; }
        public string TranferNo { get; set; }
        public int LineNum { get; set; }
        public string YesNo { get; set; }
        public List<GetSerialDelivery> Serial { get; set; }
        public List<GetBatchDelivery> Batches { get; set; }
    }
    public class GetSerialDelivery
    {
        public string ItemCode { get; set; }
        public string SerialNumber { get; set; }
        public double Qty { get; set; }
        public int BinEntry { get; set; }
    }
    public class GetBatchDelivery
    {
        public string ItemCode { get; set; }
        public string BatchNumber { get; set; }
        public string ExpDate { get; set; }
        //public double InputQty { get; set; }
        public double Qty { get; set; }
        //Quantity
        public int BinEntry { get; set; }
    }
}