using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models.SalesManagement;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class PendingSaleOrdersViewModel : ViewModelBase
    {
        public PendingSaleOrdersViewModel()
        {
            if (IsInDesignMode) return;

            SalesOrders = GetSalesOrdersFromDb();
        }

        public SvenTechCollection<SalesOrder> SalesOrders { get; set; }
        public SalesOrder SelectedSalesOrder { get; set; }

        private SvenTechCollection<SalesOrder> GetSalesOrdersFromDb()
        {
            return DataContext.Instance.SalesOrders.GetOpenedSalesOrders().ToSvenTechCollection();
        }
    }
}