using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models.SalesManagement;
using System;
using System.Linq;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class PendingSaleOrdersViewModel : ViewModelBase
    {
        #region Contructor

        public PendingSaleOrdersViewModel()
        {
            if (IsInDesignMode) return;

            CloseOrderCommand = new DelegateCommand(CloseOrder, SelectedSalesOrder != null);
            RefreshData();
        }

        #endregion Contructor

        #region Fields

        #endregion Fields

        #region Properties

        public SvenTechCollection<SalesOrder> SalesOrders { get; set; }
        public DelegateCommand CloseOrderCommand { get; set; }
        public decimal OutstandingInvoiceAmount => CalculateOutstandingInvoiceAmount();

        public SvenTechCollection<Invoice> OpenedInvoices => SelectedSalesOrder.Invoices.Where(x => x.IsPaid).ToSvenTechCollection();

        public SalesOrder SelectedSalesOrder { get; set; }

        #endregion Properties

        #region Methods

        private void CloseOrder()
        {
            SelectedSalesOrder.IsClosed = true;
            DataContext.Instance.SalesOrders.Update(SelectedSalesOrder);
            RefreshData();
            SelectedSalesOrder = null;
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

        public void CheckOrdersStatus()
        {
            foreach (var order in SalesOrders)
            {
                order.CheckStatus();
            }
        }

        #endregion Methods
    }
}