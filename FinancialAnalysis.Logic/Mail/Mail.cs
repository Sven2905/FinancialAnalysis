using FinancialAnalysis.Models.MailManagement;
using Serilog;
using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;

namespace FinancialAnalysis.Logic
{
    public static class Mail
    {
        public static void Send(MailData mailData, MailConfiguration mailConfiguration)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs\\Mail.txt", rollingInterval: RollingInterval.Month)
                .CreateLogger();

            MailMessage message = new MailMessage(mailConfiguration.Address, mailData.To)
            {
                Subject = mailData.Subject,
                Body = mailData.Body
            };

            if (mailData.AttachmentStream != null)
            {
                Attachment attachment = new Attachment(mailData.AttachmentStream, MediaTypeNames.Application.Octet);
                message.Attachments.Add(attachment);
            }

            try
            {
                SmtpClient client = new SmtpClient(mailConfiguration.Server)
                {
                    Credentials = new NetworkCredential(mailConfiguration.LoginUser, mailConfiguration.GetPasswordDecrypted())
                };
                client.Send(message);
                message.Attachments.Dispose();
            }
            catch (Exception ex)
            {
                Log.Error($"Exception occured while sending mail.", ex);
            }
        }
    }
}