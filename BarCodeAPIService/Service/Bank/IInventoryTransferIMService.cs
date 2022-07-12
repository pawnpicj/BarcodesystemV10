﻿using System.Threading.Tasks;
using BarCodeLibrary.Respones.SAP;

namespace BarCodeAPIService.Service
{
    public interface IInventoryTransferIMService
    {
        Task<ResponseGetOWTR> responseGetOWTR();
        Task<ResponseGetWTRLine> responseGetWTRLine(int DocEntry);
    }
}