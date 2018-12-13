using System.IO;

namespace FinancialAnalysis.Models.Mail
{
    public class MailData
    {
        public string To { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public Stream AttachmentStream { get; set; }
    }
}