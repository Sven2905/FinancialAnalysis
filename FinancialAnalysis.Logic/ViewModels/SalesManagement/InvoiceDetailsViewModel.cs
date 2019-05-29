using DevExpress.Mvvm;

using FinancialAnalysis.Models.SalesManagement;
using System;
using System.Windows.Input;
using WebApiWrapper.SalesManagement;

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
                Invoices.Update(Invoice);
            }
        }

        private void CreateReminder()
        {
            // TODO Report

            InvoiceReminder reminder = new InvoiceReminder()
            {
                Date = DateTime.Now,
                IsLastReminder = false,
                RefInvoiceId = Invoice.InvoiceId,
                ReminderType = Models.Enums.ReminderType.Mail,
                Username = Globals.ActiveUser.Name
            };

            InvoiceReminders.Insert(reminder);
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
