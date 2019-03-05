using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
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
        public PaymentConditionViewModel()
        {
            PaymentConditions = DataContext.Instance.PaymentConditions.GetAll().ToSvenTechCollection();
            SetCommands();
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
        }

        private void SavePaymentCondition()
        {
            DataContext.Instance.PaymentConditions.UpdateOrInsert(SelectedPaymentCondition);
        }

        private void DeletePaymentCondition()
        {
            if (SelectedPaymentCondition.PaymentConditionId != 0)
            {
                DataContext.Instance.PaymentConditions.Delete(SelectedPaymentCondition.PaymentConditionId);
            }
        }

        public PaymentCondition SelectedPaymentCondition { get; set; }
        public SvenTechCollection<PaymentCondition> PaymentConditions { get; set; } = new SvenTechCollection<PaymentCondition>();
        public ICommand NewPaymentConditionCommand { get; set; }
        public ICommand SavePaymentConditionCommand { get; set; }
        public ICommand DeletePaymentConditionCommand { get; set; }
    }
}
