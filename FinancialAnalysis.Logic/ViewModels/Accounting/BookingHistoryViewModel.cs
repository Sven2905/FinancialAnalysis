using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using System;

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

                var bookings =
                    DataContext.Instance.Bookings.GetByConditions(new DateTime(2018, 11, 24), DateTime.Now, 2);
        }
    }
}