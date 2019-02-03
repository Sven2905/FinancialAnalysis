using System.Threading.Tasks;
using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models.WarehouseManagement;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels.WarehouseManagement
{
    public class StockingViewModel : ViewModelBase
    {
        #region Constructur

        public StockingViewModel()
        {
            if (IsInDesignMode) return;

            Task.Run(() => GetWarehouses());
        }

        #endregion Constructur

        #region Properties

        public SvenTechCollection<Warehouse> Warehouses { get; set; }

        #endregion Properties

        #region Methods

        private void GetWarehouses()
        {
            Warehouses = DataContext.Instance.Warehouses.GetAll().ToSvenTechCollection();
        }

        #endregion Methods
    }
}