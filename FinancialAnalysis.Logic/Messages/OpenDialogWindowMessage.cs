using DevExpress.Mvvm;
using System.Windows;

namespace FinancialAnalysis.Logic.Messages
{
    public class OpenDialogWindowMessage : ViewModelBase
    {
        public OpenDialogWindowMessage(string Title, string Message,
            MessageBoxImage MessageBoxImage = MessageBoxImage.None)
        {
            if (IsInDesignMode)
            {
                return;
            }

            this.Title = Title;
            this.Message = Message;
            this.MessageBoxImage = MessageBoxImage;
        }

        public string Title { get; set; }
        public string Message { get; set; }
        public MessageBoxImage MessageBoxImage { get; set; }
    }
}