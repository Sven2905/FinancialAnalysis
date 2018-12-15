using System;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using FinancialAnalysis.Models.Mail;

namespace FinancialAnalysis.Logic.Mail
{
    public static class Mail
    {
        public static void Send(MailData mailData, MailConfiguration mailConfiguration)
        {
            var message = new MailMessage(mailConfiguration.Address, mailData.To)
            {
                Subject = mailData.Subject,
                Body = mailData.Body
            };

            if (mailData.AttachmentStream != null)
            {
                var attachment = new Attachment(mailData.AttachmentStream, MediaTypeNames.Application.Octet);
                message.Attachments.Add(attachment);
            }

            try
            {
                var client = new SmtpClient(mailConfiguration.Server);
                client.Credentials = new NetworkCredential(mailConfiguration.LoginUser, mailConfiguration.Password);
                client.Send(message);
                message.Attachments.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}