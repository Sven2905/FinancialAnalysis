using System;
using Utilities;

namespace FinancialAnalysis.Models.Mail
{
    [Serializable]
    public class MailConfiguration
    {
        private string _Password = string.Empty;

        public int MailConfigurationId { get; set; }
        public string Server { get; set; }
        public string Address { get; set; }
        public string LoginUser { get; set; }

        public string Password
        {
            get
            {
                if (_Password != string.Empty)
                {
                    return Encryption.DecryptText(_Password, @"G*ZCx[WD;d<k3*Gc");
                }
                return "";
            }
            set
            {
                if (value != "")
                    _Password = Encryption.EncryptText(value, @"G*ZCx[WD;d<k3*Gc");
            }
        }
    }
}