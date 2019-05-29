using DevExpress.Mvvm;
using FinancialAnalysis.Models.WarehouseManagement;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class StockyardStatusViewModel : ViewModelBase
    {
        public Stockyard Stockyard { get; set; }

        public void Refresh()
        {
            RaisePropertyChanged("Stockyard");
        }
    }
}
