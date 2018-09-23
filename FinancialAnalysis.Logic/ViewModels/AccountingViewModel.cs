using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Mvvm;
using FinancialAnalysis.Models.Accounting;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class AccountingViewModel : ViewModelBase
    {
        public ObservableCollection<Booking> Bookings { get; set; }
        public ObservableCollection<Kontenrahmen> Kontenrahmen { get; set; }
    }
}
