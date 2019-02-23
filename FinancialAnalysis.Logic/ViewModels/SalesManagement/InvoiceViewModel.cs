using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.Interfaces;
using FinancialAnalysis.Models.SalesManagement;
using System.Linq;
using System.Windows.Input;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class InvoiceViewModel : ViewModelBase
    {
        #region Fields

        private int _Quantity = 0;
        private SalesOrder _SalesOrder;

        #endregion Fields

        #region Constructor

        public InvoiceViewModel()
        {
            RemoveFromInvoiceDropCommand = new DelegateCommand<IDropEventArgs>(RemoveFromInvoiceDrop);
            AddToInvoiceCommand = new DelegateCommand<IDropEventArgs>(AddToInvoiceDrop);
            Messenger.Default.Register<SelectedQuantity>(this, GetSelectedQuantity);
            GetData();
        }

        #endregion Constructor

        #region Methods

        private void GetData()
        {
            InvoiceTypes = DataContext.Instance.InvoiceTypes.GetAll().ToSvenTechCollection();
            PaymentConditions = DataContext.Instance.PaymentConditions.GetAll().ToSvenTechCollection();
        }

        public void RemoveFromInvoiceDrop(IDropEventArgs e)
        {
            if (e.Items?.Count > 0 && e.Items[0] is SalesOrderPosition)
            {
                var item = (SalesOrderPosition)e.Items[0];
                if (item.Quantity > 1)
                {
                    Messenger.Default.Send(new OpenQuantityWindowMessage(((int)item.Quantity)));
                }
                else
                {
                    _Quantity = 1;
                }

                TransferPosition(OrderedProducts, ProductsOnInvoice, item, _Quantity);

                e.Handled = true;
            }
        }

        public void AddToInvoiceDrop(IDropEventArgs e)
        {
            if (e.Items?.Count > 0 && e.Items[0] is SalesOrderPosition)
            {
                var item = (SalesOrderPosition)e.Items[0];
                if (item.Quantity > 1)
                {
                    Messenger.Default.Send(new OpenQuantityWindowMessage(((int)item.Quantity)));
                }
                else
                {
                    _Quantity = 1;
                }
                TransferPosition(ProductsOnInvoice, OrderedProducts, item, _Quantity);
                e.Handled = true;
            }
        }

        private void GetSelectedQuantity(SelectedQuantity SelectedQuantity)
        {
            _Quantity = SelectedQuantity.Quantity;
        }

        private void TransferPosition(SvenTechCollection<SalesOrderPosition> target, SvenTechCollection<SalesOrderPosition> source, SalesOrderPosition item, int Quantity)
        {
            if (target.IsNull() || source.IsNull() || item.IsNull() || Quantity <= 0)
            {
                return;
            }

            if (Quantity == item.Quantity)
            {
                target.Add(item);
                source.Remove(item);
            }
            else
            {
                var itemClone = item.Clone();
                itemClone.Quantity = Quantity;
                target.Add(itemClone);
                source.SingleOrDefault(x => x.SalesOrderPositionId == item.SalesOrderPositionId).Quantity -= Quantity;
            }
        }

        private void SaveInvoice()
        {
            Invoice.InvoiceId = DataContext.Instance.Invoices.Insert(Invoice);

            SvenTechCollection<InvoicePosition> itemsToSave = new SvenTechCollection<InvoicePosition>();
            foreach (var item in ProductsOnInvoice)
            {
                itemsToSave.Add(new InvoicePosition(Invoice.InvoiceId, item.SalesOrderPositionId, (int)item.Quantity));
            }

            DataContext.Instance.InvoicePositions.Insert(itemsToSave);
        }

        #endregion Methods

        #region Properties

        public SalesOrder SalesOrder
        {
            get { return _SalesOrder; }
            set { _SalesOrder = value; ProductsOnInvoice = _SalesOrder.SalesOrderPositions; }
        }

        public Invoice Invoice { get; set; } = new Invoice();
        public ICommand RemoveFromInvoiceDropCommand { get; }
        public ICommand AddToInvoiceCommand { get; }
        public SvenTechCollection<PaymentCondition> PaymentConditions { get; set; } = new SvenTechCollection<PaymentCondition>();
        public SvenTechCollection<InvoiceType> InvoiceTypes { get; set; } = new SvenTechCollection<InvoiceType>();
        public SvenTechCollection<SalesOrderPosition> OrderedProducts { get; set; } = new SvenTechCollection<SalesOrderPosition>();
        public SvenTechCollection<SalesOrderPosition> ProductsOnInvoice { get; set; } = new SvenTechCollection<SalesOrderPosition>();

        #endregion Properties
    }
}