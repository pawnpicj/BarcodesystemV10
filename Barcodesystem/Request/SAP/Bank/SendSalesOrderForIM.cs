using System;
using System.Collections.Generic;

namespace BarCodeLibrary.Request.SAP
{
    public class SendSalesOrderForIM
    {
        public string CardCode { get; set; }
        public string NumAtCard { get; set; }
        public DateTime DocDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public DateTime TaxDate { get; set; }
        public int ContactPersion { get; set; }
        public string SeriesCode { get; set; }
        public string CurrencyCode { get; set; }
        public string Remark { get; set; }
        public string SlpCode { get; set; }
        public string Patient { get; set; }
        public string TranferNo { get; set; }
        public string internal_remark { get; set; }
        public string SqRemark { get; set; }
        public string InvRemark { get; set; }
        public List<SendSalesOrderLine> Lines { get; set; }
        public List<SendSalesOrderLineX> LinesX { get; set; }
    }

    public class SendSalesOrderLine
    {
        public string ItemCode { get; set; }
        public string DocEntry { get; set; }
        public double Quantity { get; set; }
        public double Discount { get; set; }
        public string ManageItem { get; set; }
        public double Price { get; set; }
        public double PriceBeforeDis { get; set; }
        public string WhsCode { get; set; }
        public int BinEntry { get; set; }
        public string BinCode { get; set; }
        public string UomName { get; set; }
        public int LineNum { get; set; }
        public string U_TranferNo { get; set; }
        public string U_Patient { get; set; }
        public List<GetSerialSalesOrder> Serial { get; set; }
        public List<GetBatchSalesOrder> Batches { get; set; }
    }
    public class SendSalesOrderLineX
    {
        public string ItemCode { get; set; }
        public string DocEntry { get; set; }
        public double Quantity { get; set; }
        public double Discount { get; set; }
        public string ManageItem { get; set; }
        public double Price { get; set; }
        public double PriceBeforeDis { get; set; }
        public string WhsCode { get; set; }
        public int BinEntry { get; set; }
        public string BinCode { get; set; }
        public string UomName { get; set; }
        public int LineNum { get; set; }
        public int BaseLine { get; set; }
        public string U_TranferNo { get; set; }
        public string U_Patient { get; set; }
        public List<GetSerialSalesOrder> Serial { get; set; }
        public List<GetBatchSalesOrder> Batches { get; set; }
    }
    public class GetSerialSalesOrder
    {
        public string ItemCode { get; set; }
        public string SerialNumber { get; set; }
        public double Quantity { get; set; }
    }
    public class GetBatchSalesOrder
    {
        public string ItemCode { get; set; }
        public string BatchNumber { get; set; }
        public string ExpDate { get; set; }
        public double Quantity { get; set; }
    }
}