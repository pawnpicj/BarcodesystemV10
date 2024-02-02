using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using BarCodeLibrary.Respones.SAP.Bank;
using Microsoft.Extensions.Options;
using BarCodeLibrary.Respones.SAP;
using Barcodesystem.Contract.RouteApi;

namespace BarCodeAPIService.Controllers.Bank
{
    [ApiController]
    [Route(APIRoute.Root)]
    public class GetDataConfigController : Controller
    {
        private IConfiguration configuration;

        public GetDataConfigController(IConfiguration iConfig)
        {
            this.configuration = iConfig;
        }

        private readonly IOptions<ResponseGetDataConfig> dataConfig;

    }
}
