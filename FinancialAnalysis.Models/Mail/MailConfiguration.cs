using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Mail
{
    [Serializable()]
    public class MailConfiguration
    {
        public string Server { get; set; }
        public string Address { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
    }
}
