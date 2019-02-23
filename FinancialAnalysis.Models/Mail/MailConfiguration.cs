using System;
using Utilities;

namespace FinancialAnalysis.Models.Mail
{
    /// <summary>
    /// Maileinstellungen
    /// </summary>
    [Serializable]
    public class MailConfiguration
    {
        private string _Password = string.Empty;

        /// <summary>
        /// Id
        /// </summary>
        public int MailConfigurationId { get; set; }

        /// <summary>
        /// Adresse des Servers
        /// </summary>
        public string Server { get; set; }

        /// <summary>
        /// Mailadresse
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Login Benutzer
        /// </summary>
        public string LoginUser { get; set; }

        /// <summary>
        /// Passwort
        /// </summary>
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
                {
                    _Password = Encryption.EncryptText(value, @"G*ZCx[WD;d<k3*Gc");
                }
            }
        }
    }
}