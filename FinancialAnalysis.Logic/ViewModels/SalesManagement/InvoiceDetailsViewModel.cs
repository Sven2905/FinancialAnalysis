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
            // TODO Report

            var reminder = new InvoiceReminder()
            {
                Date = DateTime.Now,
                IsLastReminder = false,
                RefInvoiceId = Invoice.InvoiceId,
                ReminderType = Models.Enums.ReminderType.Mail,
                Username = Globals.ActiveUser.Name
            };

            DataContext.Instance.InvoiceReminders.Insert(reminder);
            Invoice.InvoiceReminders.Add(reminder);
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
