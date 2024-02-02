using System.Collections.Generic;

namespace BarCodeLibrary.Respones.SAP.Bank
{
    public class ResponseGetDataConfig
    {
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public List<ListData> Data { get; set; }
    }

    public class ListData
    {
        public string CompanyDB { get; set; }
        public string UserNameSAP { get; set; }
        public string Password { get; set; }
        public string UserNameSAPX { get; set; }
        public string PasswordX { get; set; }
    }
}
