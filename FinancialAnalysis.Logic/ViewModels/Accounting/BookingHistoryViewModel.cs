using DevExpress.Mvvm;
using System;
using WebApiWrapper.Accounting;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class BookingHistoryViewModel : ViewModelBase
    {
        public BookingHistoryViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }
            var bookings = Bookings.GetByParameter(new DateTime(2018, 11, 24), DateTime.Now, 2);
        }
    }
}