﻿using System.Collections.Generic;

namespace BarCodeLibrary.Request.SAP
{
    //Head
    public class SendInventoryCounting
    {
        public int Series { get; set; }

        public string CountingDate { get; set; } // Posting Date (YYYY-MM-DD)
        public string CountingTime { get; set; } // 15:37 

        public string CountingType { get; } = "1"; // 1 = Single Counter, 2 = Multiple Counters

        public string CounterType { get; } = "12"; // 12 = User Type, 171 = Employee Type
        //public int CounterCode { get; set; }
        //public string CounterName { get; set; }

        public string Ref2 { get; set; }
        public string Comments { get; set; }

        public string Udf_BinLocation { get; set; }

        public List<SendInventoryCountingLine> Line { get; set; }
    }


    //Line
    public class SendInventoryCountingLine
    {
        //public string BarCode { get; set; }
        public string ItemCode { get; set; }
        public string WhsCode { get; set; }
        public int BinEntry { get; set; }
        public string Freeze { get; } = "N"; // Y/N
        public string UomCode { get; set; } // Required Field if Item has UOM Group
        public long CountedQuantity { get; set; }
        public string Counted { get; } = "Y";
        //public string BatchNo { get; set; }
        //public string SerialNo { get; set; }
        public long Quantity { get; set; }
        public long TotalQuantity { get; set; }
        public string ProductType { get; set; }
        public List<GetBatchNumber> BatchLine { get; set; }
        public List<GetSerialNumber> SerialLine { get; set; }
    }

    public class GetBatchNumber
    {
        public string ItemCode { get; set; }
        public string BatchNumber { get; set; }
        public double Quantity { get; set; }
    }

    public class GetSerialNumber
    {
        public string ItemCode { get; set; }
        public string SerialNumber { get; set; }
        public double Quantity { get; set; }
    }
}