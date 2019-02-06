using DevExpress.Mvvm;
using FinancialAnalysis.Models.WarehouseManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
