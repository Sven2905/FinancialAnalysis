using DevExpress.Mvvm;
using FinancialAnalysis.Models.Accounting;
using System;
using System.Linq;
using Utilities;
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

            GetData();
            SearchCommand = new DelegateCommand(SearchForData);
        }

        private void GetData()
        {
            CostAccountList = CostAccounts.GetAll().ToSvenTechCollection();
        }

        private void SearchForData()
        {
            ResultList = Bookings.GetByParameter(StartDate, EndDate, CostAccountCreditorId, CostAccountDebitorId).ToSvenTechCollection();
        }

        public SvenTechCollection<CostAccount> CostAccountList { get; set; } = new SvenTechCollection<CostAccount>();
        public SvenTechCollection<Booking> ResultList { get; set; } = new SvenTechCollection<Booking>();
        public DateTime StartDate { get; set; } = DateTime.Now.AddDays(-7);
        public DateTime EndDate { get; set; } = DateTime.Now;
        public int? CostAccountCreditorId { get; set; }
        public int? CostAccountDebitorId { get; set; }
        public Booking SelectedBooking { get; set; }
        public DelegateCommand SearchCommand { get; set; }
    }
}