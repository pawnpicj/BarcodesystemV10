﻿using System;
using System.Collections.Generic;

namespace BarCodeLibrary.Respones.SAP.Tengkimleang
{
    public class ResponseGetGenerateBatchSerial
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public List<GetGenerateBatchSerial> Data { get; set; }
    }

    public class GetGenerateBatchSerial
    {
        public string ItemCode { get; set; }
        public int Qty { get; set; }
        public string SerialAndBatch { get; set; }
        public string Script { get; set; }
        public string MfrDate { get; set; }
        public string ExpirationDate { get; set; }
        public string AdmissionDate { get; set; }
    }
}