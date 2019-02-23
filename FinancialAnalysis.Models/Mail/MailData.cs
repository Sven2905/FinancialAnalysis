using System.IO;

namespace FinancialAnalysis.Models.Mail
{
    /// <summary>
    /// Daten zum Mailversand
    /// </summary>
    public class MailData
    {
        /// <summary>
        /// Empfänger
        /// </summary>
        public string To { get; set; }

        /// <summary>
        /// Betreff
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Inhalt
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Anhang
        /// </summary>
        public Stream AttachmentStream { get; set; }
    }
}