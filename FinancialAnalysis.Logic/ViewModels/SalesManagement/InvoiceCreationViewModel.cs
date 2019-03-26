using DevExpress.Mvvm;

using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Logic.SalesManagement;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.Interfaces;
using FinancialAnalysis.Models.ProjectManagement;
using FinancialAnalysis.Models.SalesManagement;
using System.Linq;
using System.Windows.Input;
using Utilities;
using WebApiWrapper.Accounting;
using WebApiWrapper.ProjectManagement;
using WebApiWrapper.SalesManagement;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class InvoiceCreationViewModel : ViewModelBase
    {
        #region Fields

        private int _Quantity = 0;
        private SalesOrder _SalesOrder;

        #endregion Fields

        #region Constructor

        public InvoiceCreationViewModel()
        {
            SetCommands();
            Messenger.Default.Register<SelectedQuantity>(this, GetSelectedQuantity);
            GetData();
        }

        #endregion Constructor

        #region Methods

        private void SetCommands()
        {
            RemoveFromInvoiceDropCommand = new DelegateCommand<IDropEventArgs>(RemoveFromInvoiceDrop);
            AddToInvoiceCommand = new DelegateCommand<IDropEventArgs>(AddToInvoiceDrop);
            CreateInvoiceCommand = new DelegateCommand(SaveInvoice, () => ProductsOnInvoice.Count > 0);
        }

        private void GetData()
        {
            InvoiceTypeList = InvoiceTypes.GetAll().ToSvenTechCollection();
            PaymentConditionList = PaymentConditions.GetAll().ToSvenTechCollection();
            EmployeeList = Employees.GetAll().ToSvenTechCollection();
        }

        public void RemoveFromInvoiceDrop(IDropEventArgs e)
        {
            if (e.GridControl.Name != "OrderPositions")
            {
                e.Handled = true;
                return;
            }
            if (e.Items?.Count > 0 && e.Items[0] is SalesOrderPosition)
            {
                var item = (SalesOrderPosition)e.Items[0];
                if (item.Quantity > 1)
                    Messenger.Default.Send(new OpenQuantityWindowMessage((int)item.Quantity));
                else
                    _Quantity = 1;

                TransferPosition(OrderedProducts, ProductsOnInvoice, item, _Quantity);

                e.Handled = true;
            }
        }

        public void AddToInvoiceDrop(IDropEventArgs e)
        {
            if (e.GridControl.Name != "OrderPositions")
            {
                e.Handled = true;
                return;
            }
            if (e.Items?.Count > 0 && e.Items[0] is SalesOrderPosition)
            {
                var item = (SalesOrderPosition)e.Items[0];
                if (item.Quantity > 1)
                {
                    Messenger.Default.Send(new OpenQuantityWindowMessage((int)item.Quantity));
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
                if (target.SingleOrDefault(x => x.SalesOrderPositionId == item.SalesOrderPositionId) != null)
                {
                    target.SingleOrDefault(x => x.SalesOrderPositionId == item.SalesOrderPositionId).Quantity += Quantity;
                }
                else
                {
                    target.Add(item);
                }
                source.Remove(item);
            }
            else
            {
                var itemClone = item.Clone();
                itemClone.Quantity = Quantity;
                if (target.SingleOrDefault(x => x.SalesOrderPositionId == item.SalesOrderPositionId) != null)
                {
                    target.SingleOrDefault(x => x.SalesOrderPositionId == item.SalesOrderPositionId).Quantity += Quantity;
                }
                else
                {
                    target.Add(itemClone);
                }
                source.SingleOrDefault(x => x.SalesOrderPositionId == item.SalesOrderPositionId).Quantity -= Quantity;
            }
        }

        private void SaveInvoice()
        {
            Invoice.InvoiceId = Invoices.Insert(Invoice);
            Invoice.TotalAmount = ProductsOnInvoice.Sum(x => x.Total);
            Invoice.Employee = EmployeeList.Single(x => x.EmployeeId == Invoice.RefEmployeeId);
            Invoice.Debitor = SalesOrder.Debitor;

            SvenTechCollection<InvoicePosition> itemsToSave = new SvenTechCollection<InvoicePosition>();
            foreach (var item in ProductsOnInvoice)
            {
                itemsToSave.Add(new InvoicePosition(Invoice.InvoiceId, item.SalesOrderPositionId, (int)item.Quantity));
            }

            InvoicePositions.Insert(itemsToSave);

            InvoiceReportData invoiceReportData = new InvoiceReportData()
            {
                Employee = Invoice.Employee,
                SalesOrder = SalesOrder,
                Invoice = Invoice,
                MyCompany = Globals.CoreData.MyCompany
            };

            SalesReportPDFCreator.CreateAndShowInvoiceReport(invoiceReportData, false);
        }

        #endregion Methods

        #region Properties

        public SalesOrder SalesOrder
        {
            get { return _SalesOrder; }
            set { _SalesOrder = value; ProductsOnInvoice = _SalesOrder.SalesOrderPositions; }
        }

        public Invoice Invoice { get; set; } = new Invoice();
        public ICommand RemoveFromInvoiceDropCommand { get; set; }
        public ICommand AddToInvoiceCommand { get; set; }
        public ICommand CreateInvoiceCommand { get; set; }
        public SvenTechCollection<PaymentCondition> PaymentConditionList { get; set; } = new SvenTechCollection<PaymentCondition>();
        public SvenTechCollection<InvoiceType> InvoiceTypeList { get; set; } = new SvenTechCollection<InvoiceType>();
        public SvenTechCollection<SalesOrderPosition> OrderedProducts { get; set; } = new SvenTechCollection<SalesOrderPosition>();
        public SvenTechCollection<SalesOrderPosition> ProductsOnInvoice { get; set; } = new SvenTechCollection<SalesOrderPosition>();
        public SvenTechCollection<Employee> EmployeeList { get; set; } = new SvenTechCollection<Employee>();

        #endregion Properties
    }
}