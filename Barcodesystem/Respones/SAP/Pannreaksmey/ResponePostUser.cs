﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarCodeLibrary.Respones.SAP.Pannreaksmey
{
    public class ResponsePostUser
    {
        public int ErrorCode { get; set; }
        public string? ErrorMsg { get; set; }
        public string UserCode { get; set; }
    }
}
