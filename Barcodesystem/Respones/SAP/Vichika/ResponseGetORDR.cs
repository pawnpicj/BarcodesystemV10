﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarCodeLibrary.Respones.SAP
{
    public class ResponseGetORDR
    {
        
        public string ErrorMessage { get; set; }
        public int ErrorCode { get; set; }
        public List<ORDR> Data { get; set; }
    }
    public class ORDR
    {
        public string DocNum { get; set; }
        public int DocEntry { get; set; }
        public string CardName { get; set; }
        public string CardCode { get; set; }
        public int CntctCode { get; set; }
        public string NumAtCard { get; set; }        
        public string DocStatus { get; set; }
        public string DocDate { get; set; }
        public DateTime DocDueDate { get; set; }
        public DateTime TaxDate { get; set; }
        public double DocTotal { get; set; }
        public double DiscPrcnt { get; set; }
        public int SlpCode { get; set; }
        public string SlpName { get; set; }
        public List<RDR1> Line { get; set; }
    }
    public class RDR1
    {
        public string ItemCode { get; set; }
        public string Description { get; set; }
        public double Quatity { get; set; }
        public double Price { get; set; }
        public double DiscPrcnt { get; set; }
        public string VatGroup { get; set; }
        public double LineTotal { get; set; }
        public string WhsCode { get; set; }
    }
}

