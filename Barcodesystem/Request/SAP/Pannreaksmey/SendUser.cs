using System;

namespace BarCodeLibrary.Request.SAP.Pannreaksmey
{
    public class SendUser
    {
        public int UserID { get; set; }
        public string UserCode { get; set; }
        public string UserName { get; set; }
        public string UserNameSAP { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Fax { get; set; }
        public string Department { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }
        public string Admin { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Active { get; set; }
    }
}