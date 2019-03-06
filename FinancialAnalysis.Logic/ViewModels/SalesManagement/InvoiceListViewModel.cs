using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models.SalesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class InvoiceListViewModel : ViewModelBase
    {
        public InvoiceListViewModel()
        {
            FilteredInvoices = Invoices = DataContext.Instance.Invoices.GetAll().ToSvenTechCollection();
        }

        public InvoiceListViewModel(SvenTechCollection<Invoice> Invoices)
        {
            FilteredInvoices = this.Invoices = Invoices;
        }

        private string _FilterText;
        private Invoice _SelectedInvoice;

        public string FilterText
        {
            get { return _FilterText; }
            set { _FilterText = value;
                FilteredInvoices = Invoices.Where(x => x.InvoiceId.ToString().Contains(_FilterText) || x.Debitor.Client.Name.IndexOf(_FilterText, StringComparison.OrdinalIgnoreCase) >= 0).ToSvenTechCollection();
            }
        }

        public SvenTechCollection<Invoice> FilteredInvoices { get; set; }
        public SvenTechCollection<Invoice> Invoices { get; set; } = new SvenTechCollection<Invoice>();
        public InvoiceDetailsViewModel InvoiceDetailsViewModel { get; set; } = new InvoiceDetailsViewModel();

        public Invoice SelectedInvoice
        {
            get { return _SelectedInvoice; }
            set { _SelectedInvoice = value; InvoiceDetailsViewModel.Invoice = _SelectedInvoice; }
        }

    }
}
