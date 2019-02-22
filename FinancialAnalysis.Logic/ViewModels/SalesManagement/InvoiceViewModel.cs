using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class InvoiceViewModel : ViewModelBase
    {
        private int Quantity = 0;

        public InvoiceViewModel()
        {
            OrderedProducts = DataContext.Instance.TaxTypes.GetAll().ToSvenTechCollection();
            RemoveFromInvoiceDropCommand = new DelegateCommand<IDropEventArgs>(RemoveFromInvoiceDrop);
            AddToInvoiceCommand = new DelegateCommand<IDropEventArgs>(AddToInvoiceDrop);
            Messenger.Default.Register<SelectedQuantity>(this, GetSelectedQuantity);
        }

        public void RemoveFromInvoiceDrop(IDropEventArgs e)
        {
            if (e.Items?.Count > 0 && e.Items[0] is TaxType)
            {
                OrderedProducts.Add((TaxType)e.Items[0]);
                ProductsOnInvoice.Remove((TaxType)e.Items[0]);
                e.Handled = true;
            }
        }

        public void AddToInvoiceDrop(IDropEventArgs e)
        {
            if (e.Items?.Count > 0 && e.Items[0] is TaxType)
            {
                Messenger.Default.Send(new OpenQuantityWindowMessage(((TaxType)e.Items[0]).TaxTypeId));
                ProductsOnInvoice.Add((TaxType)e.Items[0]);
                OrderedProducts.Remove((TaxType)e.Items[0]);
                e.Handled = true;
            }
        }

        private void GetSelectedQuantity(SelectedQuantity SelectedQuantity)
        {
            Quantity = SelectedQuantity.Quantity;
        }

        public ICommand RemoveFromInvoiceDropCommand { get; }
        public ICommand AddToInvoiceCommand { get; }
        public SvenTechCollection<TaxType> OrderedProducts { get; set; } = new SvenTechCollection<TaxType>();
        public SvenTechCollection<TaxType> ProductsOnInvoice { get; set; } = new SvenTechCollection<TaxType>();
    }
}
