using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class PaymentConditionViewModel : ViewModelBase
    {
        private PaymentCondition _SelectedPaymentCondition;
        private PayType _PayType;

        public PaymentConditionViewModel()
        {
            PaymentConditions = DataContext.Instance.PaymentConditions.GetAll().ToSvenTechCollection();
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
            PaymentConditions.Add(SelectedPaymentCondition);
        }

        private void SavePaymentCondition()
        {
            SelectedPaymentCondition.PayType = PayType;
            DataContext.Instance.PaymentConditions.UpdateOrInsert(SelectedPaymentCondition);
        }

        private void DeletePaymentCondition()
        {
            if (SelectedPaymentCondition.PaymentConditionId != 0)
            {
                DataContext.Instance.PaymentConditions.Delete(SelectedPaymentCondition.PaymentConditionId);
            }
            PaymentConditions.Remove(SelectedPaymentCondition);
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
        public SvenTechCollection<PaymentCondition> PaymentConditions { get; set; } = new SvenTechCollection<PaymentCondition>();
        public ICommand NewPaymentConditionCommand { get; set; }
        public ICommand SavePaymentConditionCommand { get; set; }
        public ICommand DeletePaymentConditionCommand { get; set; }
    }
}
