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
            CancelBookingCommand = new DelegateCommand(() => CheckForCancelingSelectedBooking(), ValidationForCancelingButton);
            Messenger.Default.Register<YesNoDialogResult>(this, GetDialogResult);
        }

        private void GetData()
        {
            CostAccountList = CostAccounts.GetAll().ToSvenTechCollection();
        }

        private bool ValidationForCancelingButton()
        {
            return !SelectedBooking.IsCanceled;
        }

        private void SearchForData()
        {
            ResultList = Bookings.GetByParameter(StartDate, EndDate, CostAccountCreditorId, CostAccountDebitorId, OnlyCanceledBookings).ToSvenTechCollection();
        }

        private void CheckForCancelingSelectedBooking()
        {
            Messenger.Default.Send(new OpenYesNoDialogWindowMessage("Stornieren", "Möchten Sie die ausgewählte Buchung wirklich stornieren?"));
            if (CancelConfirmation)
            {
                CancelingSelectedBooking();
            }
        }

        private void CancelingSelectedBooking()
        {
            SelectedBooking.IsCanceled = true;
            Bookings.UpdateCancelingStatus(SelectedBooking);

            Booking cancelBooking = new Booking(SelectedBooking.Amount, DateTime.Now, "Stornierung von " + SelectedBooking.Description);
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
        public bool OnlyCanceledBookings { get; set; }
        public Booking SelectedBooking { get; set; }
        public DelegateCommand SearchCommand { get; set; }
        public DelegateCommand CancelBookingCommand { get; set; }
        public bool CancelConfirmation { get; set; } = false;
    }
}