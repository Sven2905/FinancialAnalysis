using DevExpress.Mvvm;
using FinancialAnalysis.Models.WarehouseManagement;
using Utilities;
using WebApiWrapper.WarehouseManagement;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class LastBookingViewModel : ViewModelBase
    {
        private int _RefProductId;
        private int _RefStockyardId;

        public int RefStockyardId
        {
            get => _RefStockyardId;
            set
            {
                _RefStockyardId = value;
                LoadData();
            }
        }

        public int RefProductId
        {
            get => _RefProductId;
            set
            {
                _RefProductId = value;
                LoadData();
            }
        }

        public SvenTechCollection<WarehouseStockingHistory> WarehouseStockingHistoryList { get; set; } = new SvenTechCollection<WarehouseStockingHistory>();

        public void LoadData()
        {
            if (_RefProductId > 0 && RefStockyardId > 0)
            {
                WarehouseStockingHistoryList = WarehouseStockingHistories.GetLast10(_RefProductId, _RefStockyardId).ToSvenTechCollection();
            }
            else
            {
                WarehouseStockingHistoryList = null;
            }
        }
    }
}