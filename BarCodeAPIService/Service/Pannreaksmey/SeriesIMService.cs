﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Threading.Tasks;
using BarCodeAPIService.Connection;
using BarCodeAPIService.Models;
using BarCodeLibrary.Respones.SAP;

namespace BarCodeAPIService.Service.Pannreaksmey
{
    public class SeriesIMService : ISeriesIMService
    {
        public Task<ResponseNNM1_IM> responseNNM1_IM()
        {
            var nNM1IM = new List<NNM1IM>();
            var dt = new DataTable();
            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {
                    var Query = "CALL \"" + ConnectionString.CompanyDB +
                                "\"._USP_CALLTRANS_BANK ('NNM1IM','','','','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                        nNM1IM.Add(new NNM1IM
                        {
                            ObjectCode = row[0].ToString(),
                            Series = Convert.ToInt32(row[1].ToString()),
                            SeriesName = row[2].ToString(),
                            Indicator = row[3].ToString(),
                            BeginStr = row[4].ToString()
                        });
                    return Task.FromResult(new ResponseNNM1_IM
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = nNM1IM.ToList()
                    });
                }

                return Task.FromResult(new ResponseNNM1_IM
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }

            catch (Exception ex)
            {
                return Task.FromResult(new ResponseNNM1_IM
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }
    }
}