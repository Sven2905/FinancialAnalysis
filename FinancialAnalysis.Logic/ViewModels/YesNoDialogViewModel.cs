using DevExpress.Mvvm;
using System;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class YesNoDialogViewModel : ViewModelBase
    {
        public YesNoDialogViewModel()
        {
            YesCommand = new DelegateCommand(() => { SendToParent(true); CloseAction(); });
            NoCommand = new DelegateCommand(() => { SendToParent(false); CloseAction(); });
        }

        public string Title { get; set; }
        public string Message { get; set; }
        public DelegateCommand YesCommand { get; set; }
        public DelegateCommand NoCommand { get; set; }
        public Action CloseAction { get; set; }

        private void SendToParent(bool result)
        {
            Messenger.Default.Send(new YesNoDialogResult(result));
        }
    }
}
