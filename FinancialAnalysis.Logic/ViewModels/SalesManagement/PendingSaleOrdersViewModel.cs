using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Logic.SalesManagement;
using FinancialAnalysis.Models.SalesManagement;
using System.Linq;
using System.Windows.Input;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class PendingSaleOrdersViewModel : ViewModelBase
    {
        #region Contructor

        public PendingSaleOrdersViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }

            CloseOrderCommand = new DelegateCommand(CloseOrder, () => SelectedSalesOrder != null);
            OpenInvoiceWindowCommand = new DelegateCommand(CreateInvoice, () => SelectedSalesOrder != null);
            ShowPDFOrderReportCommand = new DelegateCommand(ShowPDFOrderReport, () => SelectedSalesOrder != null);
            RefreshData();
        }

        #endregion Contructor

        #region Methods

        private void CloseOrder()
        {
            SelectedSalesOrder.IsClosed = true;
            DataContext.Instance.SalesOrders.Update(SelectedSalesOrder);
            RefreshData();
            SelectedSalesOrder = null;
        }

        private void ShowPDFOrderReport()
        {
            var salesOrderReportData = new SalesOrderReportData
            {
                MyCompany = Globals.CoreData.MyCompany,
                SalesOrder = SelectedSalesOrder,
                Employee = SelectedSalesOrder.Employee
            };

            SalesReportPDFCreator.CreateAndShowOrderReport(salesOrderReportData, false);
        }

        private decimal CalculateOutstandingInvoiceAmount()
        {
            if (SelectedSalesOrder != null)
            {
                return SelectedSalesOrder.SumTotal - SelectedSalesOrder.Invoices.Sum(x => x.PaidAmount);
            }
            else
            {
                return 0;
            }
        }

        private void RefreshData()
        {
            SalesOrders = DataContext.Instance.SalesOrders.GetOpenedSalesOrders().ToSvenTechCollection();
            CheckOrdersStatus();
        }

        private void CheckOrdersStatus()
        {
            foreach (var order in SalesOrders)
            {
                order.CheckStatus();
            }
        }

        private void CreateInvoice()
        {

            var invoiceSalesOrder = SelectedSalesOrder.Clone();

            foreach (var invoice in SelectedSalesOrder.Invoices)
            {
                foreach (var invoicePosition in invoice.InvoicePositions)
                {
                    invoiceSalesOrder.SalesOrderPositions.SingleOrDefault(x => x.SalesOrderPositionId == invoicePosition.RefSalesOrderPositionId).Quantity -= invoicePosition.Quantity;
                }
            }

            for (int i = invoiceSalesOrder.SalesOrderPositions.Count - 1; i >= 0; i--)
            {
                if (invoiceSalesOrder.SalesOrderPositions[i].Quantity == 0)
                {
                    invoiceSalesOrder.SalesOrderPositions.RemoveAt(i);
                }
            }

            Messenger.Default.Send(new OpenInvoiceWindowMessage(invoiceSalesOrder));
        }

        #endregion Methods

        #region Properties

        public SvenTechCollection<SalesOrder> SalesOrders { get; set; }
        public DelegateCommand CloseOrderCommand { get; set; }
        public DelegateCommand OpenInvoiceWindowCommand { get; set; }
        public ICommand ShowPDFOrderReportCommand { get; }
        public decimal OutstandingInvoiceAmount => CalculateOutstandingInvoiceAmount();

        public SvenTechCollection<Invoice> OpenInvoices => SelectedSalesOrder.Invoices.Where(x => x.IsPaid).ToSvenTechCollection();

        public SalesOrder SelectedSalesOrder { get; set; }

        #endregion Properties
    }
}