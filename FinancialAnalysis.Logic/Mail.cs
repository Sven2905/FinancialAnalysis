using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using FinancialAnalysis.Models.Mail;

namespace FinancialAnalysis.Logic
{
    public static class Mail
    {
        public static void Send(MailData mailData, MailConfiguration mailConfiguration)
        {
            var user = "m044ad26";
            var password = "YrktvGZZu2M4eGpP";
            var server = "w011d665.kasserver.com";

            var message = new MailMessage(mailConfiguration.Address, mailData.To)
            {
                Subject = mailData.Subject,
                Body = mailData.Body
            };

            if (!string.IsNullOrEmpty(mailData.AttachmentPath))
            {
                Attachment attachment;
                attachment = new Attachment(mailData.AttachmentPath);
                message.Attachments.Add(attachment);
            }

            try
            {
                var client = new SmtpClient(server);
                client.Credentials = new NetworkCredential(user, password);
                client.Send(message);
                message.Attachments.Dispose();

                if (!string.IsNullOrEmpty(mailData.AttachmentPath)) File.Delete(mailData.AttachmentPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}