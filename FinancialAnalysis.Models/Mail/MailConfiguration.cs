using System;

namespace FinancialAnalysis.Models.Mail
{
    [Serializable]
    public class MailConfiguration
    {
        public string Server { get; set; }
        public string Address { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}