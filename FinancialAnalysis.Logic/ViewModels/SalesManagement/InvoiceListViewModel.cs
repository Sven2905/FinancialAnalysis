using DevExpress.Mvvm;

using FinancialAnalysis.Models.SalesManagement;
using System;
using System.Linq;
using Utilities;
using WebApiWrapper.SalesManagement;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class InvoiceListViewModel : ViewModelBase
    {
        public InvoiceListViewModel()
        {
            FilteredInvoices = InvoiceList = Invoices.GetAll().ToSvenTechCollection();
        }

        public InvoiceListViewModel(SvenTechCollection<Invoice> Invoices)
        {
            FilteredInvoices = InvoiceList = Invoices;
        }

        private string _FilterText;
        private Invoice _SelectedInvoice;

        public string FilterText
        {
            get => _FilterText;
            set
            {
                _FilterText = value;
                FilteredInvoices = InvoiceList.Where(x => x.InvoiceId.ToString().Contains(_FilterText) || x.Debitor.Client.Name.IndexOf(_FilterText, StringComparison.OrdinalIgnoreCase) >= 0).ToSvenTechCollection();
            }
        }

        public SvenTechCollection<Invoice> FilteredInvoices { get; set; }
        public SvenTechCollection<Invoice> InvoiceList { get; set; } = new SvenTechCollection<Invoice>();
        public InvoiceDetailsViewModel InvoiceDetailsViewModel { get; set; } = new InvoiceDetailsViewModel();
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Invoice SelectedInvoice
        {
            get => _SelectedInvoice;
            set { _SelectedInvoice = value; InvoiceDetailsViewModel.Invoice = _SelectedInvoice; }
        }
    }
}