using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models.General;
using FinancialAnalysis.Models.SalesManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class PendingSaleOrdersViewModel : ViewModelBase
    {
        public PendingSaleOrdersViewModel()
        {
            if (IsInDesignMode)
                return;

            SalesOrders = GetSalesOrdersFromDb();
        }

        private SvenTechCollection<SalesOrder> GetSalesOrdersFromDb()
        {
            return DataContext.Instance.SalesOrders.GetOpenedSalesOrders().ToSvenTechCollection();
        }

        public SvenTechCollection<SalesOrder> SalesOrders { get; set; }
        public SalesOrder SelectedSalesOrder { get; set; }
    }
}
