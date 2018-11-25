using DevExpress.Mvvm;
using DevExpress.Xpf.Dialogs;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class BookingViewModel : ViewModelBase
    {
        #region Fields

        private int _CostAccountCreditorId;
        private int _CostAccountDebitorId;
        private decimal _Amount;
        private CostAccount _CostAccountCreditor;

        #endregion Fields

        #region Constructor

        public BookingViewModel()
        {

            GetCreditorCommand = new DelegateCommand(() =>
                {
                    Messenger.Default.Send(new OpenKontenrahmenWindowMessage() { AccountingType = AccountingType.Credit });
                });
            GetDebitorCommand = new DelegateCommand(() =>
            {
                Messenger.Default.Send(new OpenKontenrahmenWindowMessage() { AccountingType = AccountingType.Debit });
            });
            OpenFileCommand = new DelegateCommand(() =>
            {
                DXOpenFileDialog fileDialog = new DXOpenFileDialog();
                if (fileDialog.ShowDialog().Value)
                {
                    SaveFileToDatabase(fileDialog.FileName);
                }
            });
            DoubleClickListBoxCommand = new DelegateCommand(() =>
            {
                if (SelectedScannedDocument != null)
                {
                    Messenger.Default.Send(new OpenPDFViewerWindowMessage(SelectedScannedDocument.Path));
                }
            });

            AddToStackCommand = new DelegateCommand(() =>
            {
                AddToStack(CreateBookingItem());
                ClearForm();
            });

            SaveStackToDBCommand = new DelegateCommand(() =>
            {
                SaveStackToDb();
            });

            SaveBookingCommand = new DelegateCommand(() =>
            {
                SaveBookingToDB(CreateBookingItem());
                ClearForm();
            });

            CancelCommand = new DelegateCommand(() =>
            {
                ClearForm();
            });

            Messenger.Default.Register<SelectedCostAccount>(this, ChangeSelectedCostAccount);

            DataLayer db = new DataLayer();
            TaxTypes = db.TaxTypes.GetAll().ToList();
            CostAccounts = db.CostAccounts.GetAllVisible().ToList();
            ScannedDocuments = db.ScannedDocuments.GetAll().ToSvenTechCollection();
        }

        #endregion Constructor

        #region Methods

        private void ClearForm()
        {
            Amount = 0;
            Description = "";
            ScannedDocuments.Clear();
        }

        private void SaveFileToDatabase(string path)
        {
            var file = File.ReadAllBytes(path);
            FileInfo fileInfo = new FileInfo(path);

            var temp = path.Split('\\');
            var fileName = temp[temp.Length - 1].Replace(".pdf", "").Replace(".PDF", "");

            ScannedDocument scannedDocument = new ScannedDocument()
            {
                Content = file,
                Date = DateTime.Now,
                FileName = fileName,
                RefBookingId = 1,
                Path = path
            };

            ScannedDocuments.Add(scannedDocument);
        }

        public void ChangeSelectedCostAccount(SelectedCostAccount SelectedCostAccount)
        {
            switch (SelectedCostAccount.AccountingType)
            {
                case AccountingType.Credit:
                    CostAccountCreditor = SelectedCostAccount.CostAccount; CostAccountCreditorId = SelectedCostAccount.CostAccount.CostAccountId; break;
                case AccountingType.Debit:
                    CostAccountDebitor = SelectedCostAccount.CostAccount; CostAccountDebitorId = SelectedCostAccount.CostAccount.CostAccountId; break;
                default:
                    break;
            }
        }

        private bool ValidateBooking()
        {
            var result = false;

            if (CostAccountCreditorId != 0 && CostAccountDebitorId != 0 && SelectedTax != null)
            {
                result = true;
            }

            result = true;

            return result;
        }

        private Booking CreateBookingItem()
        {
            if (!ValidateBooking())
            {
                return null;
            }

            Booking booking = new Booking(Amount, Date, Description);

            decimal tax = 0;
            decimal amountWithoutTax = 0;
            if (SelectedTax.AmountOfTax > 0)
            {
                if (GrossNetType == GrossNetType.Brutto)
                {
                    tax = (Amount / (100 + SelectedTax.AmountOfTax)) * SelectedTax.AmountOfTax;
                    amountWithoutTax = Amount - tax;
                }
                else
                {
                    tax = Amount / 100 * SelectedTax.AmountOfTax;
                    amountWithoutTax = Amount;
                }
            }

            Credit credit = new Credit(Amount, CostAccountCreditorId, booking.BookingId);
            Debit debit = new Debit(amountWithoutTax, CostAccountDebitorId, booking.BookingId);
            booking.Credits.Add(credit);
            booking.Debits.Add(debit);
            booking.ScannedDocuments = ScannedDocuments;

            return booking;
        }

        private void SaveBookingToDB(Booking booking)
        {
            if (booking == null)
            {
                return;
            }

            int bookingId = 0;

            using (DataLayer db = new DataLayer())
            {
                try
                {
                    bookingId = db.Bookings.Insert(booking);
                }
                catch (Exception ex)
                {
                    Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
                }
            }

            foreach (var item in booking.Credits)
            {
                item.RefBookingId = bookingId;
                using (DataLayer db = new DataLayer())
                {
                    try
                    {
                        db.Credits.Insert(item);
                    }
                    catch (Exception ex)
                    {
                        Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
                    }
                }
            }

            foreach (var item in booking.Debits)
            {
                item.RefBookingId = bookingId;
                using (DataLayer db = new DataLayer())
                {
                    try
                    {
                        db.Debits.Insert(item);
                    }
                    catch (Exception ex)
                    {
                        Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
                    }
                }
            }

            foreach (var item in booking.ScannedDocuments)
            {
                item.RefBookingId = bookingId;

                using (DataLayer db = new DataLayer())
                {
                    try
                    {
                        db.ScannedDocuments.Insert(item);
                    }
                    catch (Exception ex)
                    {
                        Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
                    }
                }
            }
        }

        private void AddToStack(Booking booking)
        {
            Booking bookingItem = CreateBookingItem();

            if (bookingItem != null)
            {
                BookingsOnStack.Add(bookingItem);
            }
        }

        private void SaveStackToDb()
        {
            foreach (var item in BookingsOnStack)
            {
                SaveBookingToDB(item);
            }

            BookingsOnStack.Clear();
        }

        #endregion Methods

        #region Properties

        public int CostAccountCreditorId
        {
            get { return _CostAccountCreditorId; }
            set { _CostAccountCreditorId = value; CostAccountCreditor = CostAccounts.Single(x => x.CostAccountId == value); }
        }

        public int CostAccountDebitorId
        {
            get { return _CostAccountDebitorId; }
            set { _CostAccountDebitorId = value; CostAccountDebitor = CostAccounts.Single(x => x.CostAccountId == value); }
        }

        public DelegateCommand AddToStackCommand { get; }
        public DelegateCommand SaveStackToDBCommand { get; }
        public DelegateCommand SaveBookingCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand GetCreditorCommand { get; }
        public DelegateCommand GetDebitorCommand { get; }
        public DelegateCommand OpenFileCommand { get; }
        public DelegateCommand DoubleClickListBoxCommand { get; set; }

        public CostAccount CostAccountCreditor
        {
            get { return _CostAccountCreditor; }
            set { _CostAccountCreditor = value; SelectedTax = TaxTypes.Single(x => x.TaxTypeId == _CostAccountCreditor.RefTaxTypeId); }
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
        public SvenTechCollection<ScannedDocument> ScannedDocuments { get; set; }

        public decimal Amount
        {
            get { return Math.Round(_Amount, 2); }
            set { _Amount = value; RaisePropertyChanged(); }
        }

        #endregion Properties
    }
}
