using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BarCodeAPIService.Connection;
using BarCodeAPIService.Models;
using BarCodeLibrary.Request.SAP.Pannreaksmey;
using BarCodeLibrary.Respones.SAP;
using BarCodeLibrary.Respones.SAP.Bank;
using BarCodeLibrary.Respones.SAP.Pannreaksmey;

namespace BarCodeAPIService.Service
{
    public class UserService : IUserService
    {
        public Task<ResponseOUSRGetUser> ResponseOUSRGetUser()
        {
            var oUSR = new List<OUSR>();
            var dt = new DataTable();
            try
            {
                var login = new LoginOnlyDatabase(LoginOnlyDatabase.Type.SapHana);
                if (login.lErrCode == 0)
                {
                    var Query = "CALL \"" + ConnectionString.CompanyDB +
                                "\"._USP_CALLTRANS_Smey ('OUSR','','','','','')";
                    login.AD = new OdbcDataAdapter(Query, login.CN);
                    login.AD.Fill(dt);
                    foreach (DataRow row in dt.Rows)
                        oUSR.Add(new OUSR
                        {
                            UserCode = row[0].ToString(),
                            UserName = row[1].ToString()
                        });
                    return Task.FromResult(new ResponseOUSRGetUser
                    {
                        ErrorCode = 0,
                        ErrorMessage = "",
                        Data = oUSR.ToList()
                    });
                }

                return Task.FromResult(new ResponseOUSRGetUser
                {
                    ErrorCode = login.lErrCode,
                    ErrorMessage = login.sErrMsg,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return Task.FromResult(new ResponseOUSRGetUser
                {
                    ErrorCode = ex.HResult,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
        }

        public Task<ResponsePostUser> ResponsePostUserAsync(SendUser send)
        {
            var clsCRUD = new ClsCRUD();
            var dt = clsCRUD.GetDataWeb(
                "INSERT INTO \"" + ConnectionString.BarcodeDb +
                "\".TBUSER(USERCODE,USERNAME,PHONE,EMAIL,FAX,DEPARTMENT,PASSWORD,ADMINISTRATOR,CREATEDATE,UPDATEDATE,ACTIVE) VALUES( '" +
                send.UserCode + "','" + send.UserName + "','" + send.Phone + "','" + send.Email + "','" + send.Fax +
                "','" + send.Department + "','" + send.Password + "','" + send.Admin +
                "',(SELECT CURRENT_DATE  FROM DUMMY),'','" + send.Active + "');", "WebDb");
            if (dt != null)
                return Task.FromResult(new ResponsePostUser
                {
                    ErrorCode = 0,
                    ErrorMsg = "",
                    UserCode = send.UserName
                });
            return Task.FromResult(new ResponsePostUser
            {
                ErrorCode = 0,
                ErrorMsg = "",
                UserCode = null
            });
        }

        public Task<ResponseGetUser> ResponseGetuser()
        {
            var clsCRUD = new ClsCRUD();
            var tbUSER = new List<TBUSER>();
            var dt = clsCRUD.GetDataWeb(
                "SELECT USERID,USERCODE,USERNAME,PHONE,EMAIL,FAX,DEPARTMENT,PASSWORD,ADMINISTRATOR,TO_DATE(CREATEDATE, 'YYYY-MM-DD'),TO_DATE(UPDATEDATE, 'YYYY-MM-DD'),ACTIVE FROM \"" +
                ConnectionString.BarcodeDb + "\".TBUSER", "WebDb");
            var responseGetUsers = new List<ResponseGetUser>();
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                    try
                    {
                        tbUSER.Add(new TBUSER
                        {
                            UserID = row[0].ToString(),
                            UserCode = row[1].ToString(),
                            UserName = row[2].ToString(),
                            Phone = row[3].ToString(),
                            Email = row[4].ToString(),
                            Fax = row[5].ToString(),
                            Department = row[6].ToString(),
                            Password = row[7].ToString(),
                            Admin = row[8].ToString(),
                            CreateDate = row[9].ToString(),
                            UpdateDate = row[10].ToString(),
                            Active = row[11].ToString()
                        });
                    }
                    catch (Exception ex)
                    {
                        var e1 = ex.Message;
                    }

                return Task.FromResult(new ResponseGetUser
                {
                    Data = tbUSER.ToList()
                });
            }

            return Task.FromResult(new ResponseGetUser
            {
                Data = null
            });
        }

        public Task<ResponseGetDataConfig> ResponseGetDataConfig()
        {
            var listData = new List<ListData>();

            
            listData.Add(new ListData
            {
                CompanyDB = ConnectionString.CompanyDB,
                UserNameSAP = ConnectionString.UserName
            });


            return Task.FromResult(new ResponseGetDataConfig
            {
                ErrorCode = 0,
                ErrorMessage = "",
                Data = listData.ToList()
            });
        }

        public Task<ResponsePostUser> ResponseUpdatePasswordAsync(SendUser send)
        {
            var clsCRUD = new ClsCRUD();
            var dt = clsCRUD.GetDataWeb(
                "UPDATE \"" + ConnectionString.BarcodeDb + "\".TBUSER SET PASSWORD = '"+send.Password+"' " +
                " WHERE USERNAME = '"+send.UserName+"' AND PASSWORD = '"+send.OldPassword+"' " +
                "", "WebDb");
            if (dt != null)
                return Task.FromResult(new ResponsePostUser
                {
                    ErrorCode = 0,
                    ErrorMsg = "",
                    UserCode = send.UserName
                });
            return Task.FromResult(new ResponsePostUser
            {
                ErrorCode = 0,
                ErrorMsg = "",
                UserCode = null
            });
        }
    }
}