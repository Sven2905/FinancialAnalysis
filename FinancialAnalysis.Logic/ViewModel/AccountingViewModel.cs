using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinancialAnalysis.Models.Accounting;

namespace FinancialAnalysis.Logic.ViewModel
{
    public class AccountingViewModel
    {
        public ObservableCollection<Booking> Bookings { get; set; }
        public ObservableCollection<Kontenrahmen> Kontenrahmen { get; set; }
    }
}
