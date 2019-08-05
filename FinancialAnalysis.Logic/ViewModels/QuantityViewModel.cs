using DevExpress.Mvvm;
using System;
using System.Windows.Input;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class QuantityViewModel : ViewModelBase
    {
        private int _MaxQuantity;

        public QuantityViewModel()
        {
            CloseCommand = new DelegateCommand(() =>
            { SendToParent(); CloseAction(); });
        }

        public int MaxQuantity
        {
            get => _MaxQuantity;
            set { _MaxQuantity = value; Quantity = _MaxQuantity; }
        }

        public int Quantity { get; set; }
        public ICommand CloseCommand { get; set; }
        public Action CloseAction { get; set; }

        private void SendToParent()
        {
            Messenger.Default.Send(new SelectedQuantity { Quantity = Quantity });
        }
    }
}