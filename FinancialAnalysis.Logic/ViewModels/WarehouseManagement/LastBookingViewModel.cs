using DevExpress.Mvvm;

using FinancialAnalysis.Models.General;
using FinancialAnalysis.Models.WarehouseManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            get { return _RefStockyardId; }
            set
            {
                _RefStockyardId = value;
                LoadData();
            }
        }

        public int RefProductId
        {
            get { return _RefProductId; }
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
                WarehouseStockingHistoryList = WarehouseStockingHistories.GetLast10(_RefProductId, _RefStockyardId).ToSvenTechCollection();
            else
            {
                WarehouseStockingHistoryList = null;
            }
        }
    }
}
