using BarCodeAPIService.Connection;
using SAPbobsCOM;
using System;
using System.Data;
using System.Data.Odbc;
using System.Security.Cryptography;

namespace BarCodeAPIService.Models
{
    public class LoginOnlyDatabase
    {
        public LoginOnlyDatabase(Type type)
        {
            Login(type);
        }

        public OdbcDataAdapter AD { get; set; }

        public OdbcConnection CN { get; set; }

        public OdbcCommand CMD { get; set; }

        public string sErrMsg { get; private set; }

        public int lErrCode { get; private set; }

        public Company Company { get; internal set; }
        public enum Type
        {
            SapHana,SqlHana
        }

        private void Login(Type type)
        {
            //string Server = "";
            //string DbUserName = "";
            //string DbPassword = "";
            //string CompanyDB = "";
            try
            {
                //HDBODBC
                string connectionstr = "";
                if (type == Type.SapHana)
                    connectionstr = "Driver={HDBODBC};UID=SYSTEM;PWD=SAPB1Admin;SERVERNODE=172.20.150.240:30115;[DATABASE=UDOM_TRD]";
                //connectionstr = ConnectionString.ConnectionStringHANA1!;
                //connectionstr = ConnectionString.ConnectionStringSAP!;
                //connectionstr = $"Driver={{HDBODBC}};UID={ConnectionString.DbUserName};" +
                //$"PWD={ConnectionString.DbPassword};SERVERNODE={ConnectionString.ServerGET};[DATABASE={ConnectionString.CompanyDB}];";
                else if (type == Type.SqlHana)
                {
                    connectionstr = "Driver={HDBODBC};UID=SYSTEM;PWD=SAPB1Admin;SERVERNODE=172.20.150.240:30115;[DATABASE=BARCODESYSTEMDB]";
                    //connectionstr = $"Driver={{HDBODBC}};UID={ConnectionString.DbUserName};" +
                    //$"PWD={ConnectionString.DbPassword};SERVERNODE={ConnectionString.ServerGET};[DATABASE={ConnectionString.CompanyDB}];";
                }

                CN = new OdbcConnection(connectionstr);

                if (CN.State == ConnectionState.Closed) CN.Open();
                lErrCode = CN.State == ConnectionState.Open ? 0 : 9999;
            }
            catch (Exception ex)
            {
                lErrCode = ex.GetHashCode();
                sErrMsg = ex.Message;
            }
        }
    }
}