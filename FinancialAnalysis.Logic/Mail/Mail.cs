using System;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Windows;
using DevExpress.Mvvm;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Mail;
using Serilog;

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
                var client = new SmtpClient(mailConfiguration.Server)
                {
                    Credentials = new NetworkCredential(mailConfiguration.LoginUser, mailConfiguration.Password)
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