using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using DevExpress.Mvvm;
using DevExpress.Xpf.Dialogs;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Accounting;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class BookingViewModel : ViewModelBase
    {
        #region Constructor

        public BookingViewModel()
        {
            SetCommands();

            Messenger.Default.Register<SelectedCostAccount>(this, ChangeSelectedCostAccount);

            using (var db = new DataLayer())
            {
                TaxTypes = db.TaxTypes.GetAll().ToList();
                CostAccounts = db.CostAccounts.GetAllVisible().ToList();
            }

            SelectedTax = TaxTypes.SingleOrDefault(x => x.TaxTypeId == 1);
        }

        #endregion Constructor

        #region Fields

        private int _costAccountCreditorId;
        private int _costAccountDebitorId;
        private decimal _amount;
        private CostAccount _costAccountCreditor;

        #endregion Fields

        #region Methods

        private void ClearForm()
        {
            Amount = 0;
            Description = "";
            ScannedDocuments.Clear();
        }

        private void SetCommands()
        {
            GetCreditorCommand = new DelegateCommand(() =>
            {
                Messenger.Default.Send(new OpenKontenrahmenWindowMessage {AccountingType = AccountingType.Credit});
            });

            GetDebitorCommand = new DelegateCommand(() =>
            {
                Messenger.Default.Send(new OpenKontenrahmenWindowMessage {AccountingType = AccountingType.Debit});
            });

            OpenFileCommand = new DelegateCommand(() =>
            {
                var fileDialog = new DXOpenFileDialog();
                if (fileDialog.ShowDialog().Value) CreateScannedDocumentItem(fileDialog.FileName);
            });

            DoubleClickListBoxCommand = new DelegateCommand(() =>
            {
                if (SelectedScannedDocument != null)
                    Messenger.Default.Send(new OpenPDFViewerWindowMessage(SelectedScannedDocument.Path));
            });

            AddToStackCommand = new DelegateCommand(() =>
            {
                AddToStack(CreateBookingItem());
                ClearForm();
            });

            SaveStackToDbCommand = new DelegateCommand(SaveStackToDb);

            SaveBookingCommand = new DelegateCommand(() =>
            {
                SaveBookingToDB(CreateBookingItem());
                ClearForm();
            });

            DeleteSelectedScannedDocumentCommand = new DelegateCommand(DeleteSelectedScannedDocument);

            CancelCommand = new DelegateCommand(ClearForm);
        }

        private void DeleteSelectedScannedDocument()
        {
            if (SelectedScannedDocument != null) ScannedDocuments.Remove(SelectedScannedDocument);
        }

        private void CreateScannedDocumentItem(string path)
        {
            var file = File.ReadAllBytes(path);

            var temp = path.Split('\\');
            var fileName = temp[temp.Length - 1].Replace(".pdf", "").Replace(".PDF", "");

            var scannedDocument = new ScannedDocument
            {
                Content = file,
                Date = DateTime.Now,
                FileName = fileName,
                RefBookingId = 1,
                Path = path
            };

            ScannedDocuments.Add(scannedDocument);
            RaisePropertiesChanged("ScannedDocuments");
        }

        public void ChangeSelectedCostAccount(SelectedCostAccount selectedCostAccount)
        {
            switch (selectedCostAccount.AccountingType)
            {
                case AccountingType.Credit:
                    CostAccountCreditor = selectedCostAccount.CostAccount;
                    CostAccountCreditorId = selectedCostAccount.CostAccount.CostAccountId;
                    break;
                case AccountingType.Debit:
                    CostAccountDebitor = selectedCostAccount.CostAccount;
                    CostAccountDebitorId = selectedCostAccount.CostAccount.CostAccountId;
                    break;
            }
        }

        private bool ValidateBooking()
        {
            var result = false;

            if (CostAccountCreditorId != 0 && CostAccountDebitorId != 0 && SelectedTax != null) result = true;

            result = true;

            return result;
        }

        private Booking CreateBookingItem()
        {
            if (!ValidateBooking()) return null;

            var booking = new Booking(Amount, Date, Description);

            decimal tax = 0;
            decimal amountWithoutTax = 0;
            if (SelectedTax.AmountOfTax > 0)
            {
                if (GrossNetType == GrossNetType.Brutto)
                {
                    tax = Amount / (100 + SelectedTax.AmountOfTax) * SelectedTax.AmountOfTax;
                    amountWithoutTax = Amount - tax;
                }
                else
                {
                    tax = Amount / 100 * SelectedTax.AmountOfTax;
                    amountWithoutTax = Amount;
                }
            }

            var credit = new Credit(Amount, CostAccountCreditorId, booking.BookingId);
            var debit = new Debit(amountWithoutTax, CostAccountDebitorId, booking.BookingId);
            booking.Credits.Add(credit);
            booking.Debits.Add(debit);
            booking.ScannedDocuments = ScannedDocuments.ToList();

            return booking;
        }

        private void SaveBookingToDB(Booking booking)
        {
            if (booking == null) return;

            var bookingId = 0;

            using (var db = new DataLayer())
            {
                bookingId = db.Bookings.Insert(booking);
            }

            foreach (var item in booking.Credits)
            {
                item.RefBookingId = bookingId;
                using (var db = new DataLayer())
                {
                    db.Credits.Insert(item);
                }
            }

            foreach (var item in booking.Debits)
            {
                item.RefBookingId = bookingId;
                using (var db = new DataLayer())
                {
                    db.Debits.Insert(item);
                }
            }

            foreach (var item in booking.ScannedDocuments)
            {
                item.RefBookingId = bookingId;

                using (var db = new DataLayer())
                {
                    db.ScannedDocuments.Insert(item);
                }
            }
        }

        private void AddToStack(Booking booking)
        {
            var bookingItem = CreateBookingItem();

            if (bookingItem != null) BookingsOnStack.Add(bookingItem);
        }

        private void SaveStackToDb()
        {
            foreach (var item in BookingsOnStack) SaveBookingToDB(item);

            BookingsOnStack.Clear();
        }

        #endregion Methods

        #region Properties

        public int CostAccountCreditorId
        {
            get => _costAccountCreditorId;
            set
            {
                _costAccountCreditorId = value;
                CostAccountCreditor = CostAccounts.Single(x => x.CostAccountId == value);
            }
        }

        public int CostAccountDebitorId
        {
            get => _costAccountDebitorId;
            set
            {
                _costAccountDebitorId = value;
                CostAccountDebitor = CostAccounts.Single(x => x.CostAccountId == value);
            }
        }

        public DelegateCommand AddToStackCommand { get; set; }
        public DelegateCommand SaveStackToDbCommand { get; set; }
        public DelegateCommand SaveBookingCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand GetCreditorCommand { get; set; }
        public DelegateCommand GetDebitorCommand { get; set; }
        public DelegateCommand OpenFileCommand { get; set; }
        public DelegateCommand DoubleClickListBoxCommand { get; set; }
        public DelegateCommand DeleteSelectedScannedDocumentCommand { get; set; }

        public CostAccount CostAccountCreditor
        {
            get => _costAccountCreditor;
            set
            {
                _costAccountCreditor = value;
                SelectedTax = TaxTypes.Single(x => x.TaxTypeId == _costAccountCreditor.RefTaxTypeId);
            }
        }

        public SvenTechCollection<Booking> BookingsOnStack { get; set; } = new SvenTechCollection<Booking>();
        public string Description { get; set; }
        public CostAccount CostAccountDebitor { get; set; }
        public TaxType SelectedTax { get; set; }
        public List<CostAccount> CostAccounts { get; set; }
        public List<TaxType> TaxTypes { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public GrossNetType GrossNetType { get; set; }
        public ScannedDocument SelectedScannedDocument { get; set; }

        public ObservableCollection<ScannedDocument> ScannedDocuments { get; set; } =
            new ObservableCollection<ScannedDocument>();

        public decimal Amount
        {
            get => Math.Round(_amount, 2);
            set
            {
                _amount = value;
                RaisePropertyChanged();
            }
        }

        #endregion Properties
    }
}