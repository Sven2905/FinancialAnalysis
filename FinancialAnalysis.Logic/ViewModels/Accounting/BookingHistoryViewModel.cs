using System;
using System.Windows;
using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Administration;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class BookingHistoryViewModel : ViewModelBase
    {
        public User ActualUser { get { return Globals.ActualUser; } }

        public BookingHistoryViewModel()
        {
            using (var db = new DataLayer())
            {
                try
                {
                    var bookings = db.Bookings.GetByConditions(new DateTime(2018, 11, 24), DateTime.Now, 2);
                }
                catch (Exception ex)
                {
                    Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, MessageBoxImage.Error));
                }
            }
        }
    }
}