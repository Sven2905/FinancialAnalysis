using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.General
{
    public static class NotificationMessages
    {
        public static void ShowError(string title = "Fehler", string message = "Es ist ein Fehler aufgetreten.")
        {
            var notificationManager = new NotificationManager();
            notificationManager.Show(new NotificationContent
            {
                Title = title,
                Message = message,
                Type = NotificationType.Error
            });
        }

        public static void ShowSuccess(string title = "Erfolgreich", string message = "Der Vorgang wurde erfolgreich ausgeführt.")
        {
            var notificationManager = new NotificationManager();
            notificationManager.Show(new NotificationContent
            {
                Title = title,
                Message = message,
                Type = NotificationType.Success
            });
        }

        public static void ShowInformation(string title , string message)
        {
            var notificationManager = new NotificationManager();
            notificationManager.Show(new NotificationContent
            {
                Title = title,
                Message = message,
                Type = NotificationType.Information
            });
        }

        public static void ShowWarning(string title, string message)
        {
            var notificationManager = new NotificationManager();
            notificationManager.Show(new NotificationContent
            {
                Title = title,
                Message = message,
                Type = NotificationType.Warning
            });
        }
    }
}
