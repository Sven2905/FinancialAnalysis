using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models.SalesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class InvoiceDetailsViewModel : ViewModelBase
    {
        #region Constructor

        public InvoiceDetailsViewModel()
        {
            SaveCommand = new DelegateCommand(SaveInvoice, () => Invoice != null);
            CreateReminderCommand = new DelegateCommand(CreateReminder, () => Invoice != null);
        }

        #endregion Constructor

        #region Fields

        private Invoice _Invoice;

        #endregion Fields

        #region Methods

        private void SaveInvoice()
        {
            if (Invoice?.InvoiceId > 0)
            {
                DataContext.Instance.Invoices.Update(Invoice);
            }
        }

        private void CreateReminder()
        {
            // TODO
        }

        #endregion Methods

        #region Properties

        public Invoice Invoice { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand CreateReminderCommand { get; set; }
        public decimal OutstandingAmount { get; set; }

        #endregion Properties
    }
}
