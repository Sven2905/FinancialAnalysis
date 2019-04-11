using DevExpress.Mvvm;
using FinancialAnalysis.Logic.Messages;
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
            CancelBookingCommand = new DelegateCommand(CancelSelectedBooking);
            Messenger.Default.Register<YesNoDialogResult>(this, GetDialogResult);
        }

        private void GetData()
        {
            CostAccountList = CostAccounts.GetAll().ToSvenTechCollection();
        }

        private void SearchForData()
        {
            ResultList = Bookings.GetByParameter(StartDate, EndDate, CostAccountCreditorId, CostAccountDebitorId).ToSvenTechCollection();
        }

        private void CancelSelectedBooking()
        {
            Messenger.Default.Send(new OpenYesNoDialogWindowMessage("Stornieren", "Möchten Sie die ausgewählte Buchung wirklich stornieren?"));
            if (CancelConfirmation)
            {
                var test = 42;
            }
        }

        private void GetDialogResult(YesNoDialogResult YesNoDialogResult)
        {
            CancelConfirmation = YesNoDialogResult.Result;
        }

        public SvenTechCollection<CostAccount> CostAccountList { get; set; } = new SvenTechCollection<CostAccount>();
        public SvenTechCollection<Booking> ResultList { get; set; } = new SvenTechCollection<Booking>();
        public DateTime StartDate { get; set; } = DateTime.Now.AddDays(-7);
        public DateTime EndDate { get; set; } = DateTime.Now;
        public int? CostAccountCreditorId { get; set; }
        public int? CostAccountDebitorId { get; set; }
        public Booking SelectedBooking { get; set; }
        public DelegateCommand SearchCommand { get; set; }
        public DelegateCommand CancelBookingCommand { get; set; }
        public bool CancelConfirmation { get; set; } = false;
    }
}