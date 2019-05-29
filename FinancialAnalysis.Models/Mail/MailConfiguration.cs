using Newtonsoft.Json;
using Utilities;

namespace FinancialAnalysis.Models.MailManagement
{
    /// <summary>
    /// Maileinstellungen
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class MailConfiguration
    {
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
        public string Password { get; set; }

        public void SetPassword(string newPassword)
        {
            Password = Encryption.EncryptText(newPassword, @"G*ZCx[WD;d<k3*Gc");
        }

        public string GetPasswordDecrypted()
        {
            if (string.IsNullOrEmpty(Password))
            {
                return "";
            }
            return Encryption.DecryptText(Password, @"G*ZCx[WD;d<k3*Gc");
        }
    }
}