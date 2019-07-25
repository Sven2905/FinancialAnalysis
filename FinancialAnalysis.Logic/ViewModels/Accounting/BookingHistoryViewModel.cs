using DevExpress.Mvvm;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Accounting;
using System;
using System.IO;
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

            SetCommands();
            GetCostAccounts();
        }

        private void GetCostAccounts()
        {
            CostAccountList = CostAccounts.GetAll().ToSvenTechCollection();
        }

        private void SetCommands()
        {
            SearchCommand = new DelegateCommand(SearchForData);
            CancelBookingCommand = new DelegateCommand(() => CheckForCancelingSelectedBooking(), ValidationForCancelingButton);
            DoubleClickListBoxCommand = new DelegateCommand(() =>
            {
                if (SelectedScannedDocument != null)
                {
                    MemoryStream stream = new MemoryStream(SelectedScannedDocument.Content);
                    Messenger.Default.Send(new OpenPDFViewerWindowMessage(stream));
                }
            });
            Messenger.Default.Register<YesNoDialogResult>(this, GetDialogResult);
        }

        private bool ValidationForCancelingButton()
        {
            return !SelectedBooking.IsCanceled;
        }

        private void SearchForData()
        {
            EndDate = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day, 23, 59, 59);
            ResultList = Bookings.GetByParameter(StartDate, EndDate, CostAccountCreditorId, CostAccountDebitorId, OnlyCanceledBookings).ToSvenTechCollection();
        }

        private void CheckForCancelingSelectedBooking()
        {
            Messenger
                .Default
                .Send(new OpenYesNoDialogWindowMessage("Stornieren", "Möchten Sie die ausgewählte Buchung wirklich stornieren?"));
            if (CancelConfirmation)
            {
                CancelingSelectedBooking();
            }
        }

        private void CancelingSelectedBooking()
        {
            SelectedBooking.IsCanceled = true;
            Bookings.UpdateCancelingStatus(SelectedBooking);

            Booking cancelBooking = new Booking(DateTime.Now, "Stornierung von Buchung " + SelectedBooking.BookingId + " - " + SelectedBooking.Description)
            {
                RefFixedCostAllocationId = SelectedBooking.RefFixedCostAllocationId,
                FixedCostAllocation = SelectedBooking.FixedCostAllocation,
            };

            cancelBooking.BookingId = Bookings.Insert(cancelBooking);

            foreach (Credit item in SelectedBooking.Credits)
            {
                cancelBooking.Credits.Add(new Credit(item.Amount * (-1), item.RefCostAccountId, cancelBooking.BookingId));
            }

            foreach (Debit item in SelectedBooking.Debits)
            {
                cancelBooking.Debits.Add(new Debit(item.Amount * (-1), item.RefCostAccountId, cancelBooking.BookingId));
            }

            foreach (BookingCostCenterMapping item in SelectedBooking.BookingCostCenterMappingList)
            {
                cancelBooking.BookingCostCenterMappingList.Add(new BookingCostCenterMapping(cancelBooking.BookingId, item.RefCostCenterId, item.Amount * (-1)) { CostCenter = item.CostCenter });
            }

            Credits.Insert(cancelBooking.Credits);
            Debits.Insert(cancelBooking.Debits);
            BookingCostCenterMappings.Insert(cancelBooking.BookingCostCenterMappingList);

            SearchForData();
        }

        private void GetDialogResult(YesNoDialogResult YesNoDialogResult)
        {
            CancelConfirmation = YesNoDialogResult.Result;
        }

        public SvenTechCollection<CostAccount> CostAccountList { get; set; } = new SvenTechCollection<CostAccount>();
        public SvenTechCollection<Booking> ResultList { get; set; } = new SvenTechCollection<Booking>();
        public DateTime StartDate { get; set; } = DateTime.Now.AddDays(-7).AddHours(-DateTime.Now.Hour).AddMinutes(-DateTime.Now.Minute).AddSeconds(-DateTime.Now.Second);
        public DateTime EndDate { get; set; } = DateTime.Now;
        public int? CostAccountCreditorId { get; set; }
        public int? CostAccountDebitorId { get; set; }
        public bool OnlyCanceledBookings { get; set; }
        public bool CancelConfirmation { get; set; } = false;
        public Booking SelectedBooking { get; set; }
        public ScannedDocument SelectedScannedDocument { get; set; }
        public DelegateCommand SearchCommand { get; set; }
        public DelegateCommand CancelBookingCommand { get; set; }
        public DelegateCommand DoubleClickListBoxCommand { get; set; }
    }
}