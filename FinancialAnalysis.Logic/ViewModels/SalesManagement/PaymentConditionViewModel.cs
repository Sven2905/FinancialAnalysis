using DevExpress.Mvvm;

using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Accounting;
using System.Windows.Input;
using Utilities;
using WebApiWrapper.Accounting;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class PaymentConditionViewModel : ViewModelBase
    {
        private PaymentCondition _SelectedPaymentCondition;
        private PayType _PayType;

        public PaymentConditionViewModel()
        {
            PaymentConditionList = PaymentConditions.GetAll().ToSvenTechCollection();
            SetCommands();
            ValueLabel = "Dauer (in Tagen)";
        }

        private void SetCommands()
        {
            NewPaymentConditionCommand = new DelegateCommand(CreateNewPaymentCondition);
            SavePaymentConditionCommand = new DelegateCommand(SavePaymentCondition, () => SelectedPaymentCondition != null);
            DeletePaymentConditionCommand = new DelegateCommand(DeletePaymentCondition, () => SelectedPaymentCondition != null);
        }

        private void CreateNewPaymentCondition()
        {
            SelectedPaymentCondition = new PaymentCondition();
            PaymentConditionList.Add(SelectedPaymentCondition);
        }

        private void SavePaymentCondition()
        {
            SelectedPaymentCondition.PayType = PayType;
            if (SelectedPaymentCondition.PaymentConditionId != 0)
            {
                PaymentConditions.Update(SelectedPaymentCondition);
            }
            else
            {
                PaymentConditions.Insert(SelectedPaymentCondition);
            }
        }

        private void DeletePaymentCondition()
        {
            if (SelectedPaymentCondition.PaymentConditionId != 0)
            {
                PaymentConditions.Delete(SelectedPaymentCondition.PaymentConditionId);
            }
            PaymentConditionList.Remove(SelectedPaymentCondition);
        }

        public PayType PayType
        {
            get { return _PayType; }
            set
            {
                _PayType = value;
                if (_PayType == PayType.Intervall)
                {
                    ValueLabel = "Dauer (in Tagen)";
                }
                else
                {
                    ValueLabel = "Tag des Monats";
                }
            }
        }

        public PaymentCondition SelectedPaymentCondition
        {
            get { return _SelectedPaymentCondition; }
            set { _SelectedPaymentCondition = value; PayType = _SelectedPaymentCondition.PayType; }
        }

        public string ValueLabel { get; set; }
        public SvenTechCollection<PaymentCondition> PaymentConditionList { get; set; } = new SvenTechCollection<PaymentCondition>();
        public ICommand NewPaymentConditionCommand { get; set; }
        public ICommand SavePaymentConditionCommand { get; set; }
        public ICommand DeletePaymentConditionCommand { get; set; }
    }
}
