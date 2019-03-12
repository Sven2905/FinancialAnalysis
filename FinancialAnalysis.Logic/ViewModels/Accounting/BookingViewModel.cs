using DevExpress.Mvvm;
using DevExpress.Xpf.Dialogs;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.ProjectManagement;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class BookingViewModel : ViewModelBase
    {
        #region Constructor

        public BookingViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }

            SetCommands();

            Messenger.Default.Register<SelectedCostAccount>(this, ChangeSelectedCostAccount);

            LoadLists();
            SelectedTax = FilteredTaxTypes.SingleOrDefault(x => x.TaxTypeId == 1);
        }

        #endregion Constructor

        #region Fields

        private int _costAccountCreditorId;
        private int _costAccountDebitorId;
        private decimal _amount;
        private CostAccount _costAccountCreditor;

        #endregion Fields

        #region Methods

        private void LoadLists()
        {
            FilteredTaxTypes = new SvenTechCollection<TaxType>(Globals.CoreData.TaxTypes);
            CostAccounts = DataContext.Instance.CostAccounts.GetAllVisible().ToList();
            CostCenterCategories = DataContext.Instance.CostCenterCategories.GetAll().ToSvenTechCollection();
            Projects = DataContext.Instance.Projects.GetAll().ToSvenTechCollection();
        }

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
                Messenger.Default.Send(new OpenKontenrahmenWindowMessage { AccountingType = AccountingType.Credit });
            });

            GetDebitorCommand = new DelegateCommand(() =>
            {
                Messenger.Default.Send(new OpenKontenrahmenWindowMessage { AccountingType = AccountingType.Debit });
            });

            OpenFileCommand = new DelegateCommand(() =>
            {
                var fileDialog = new DXOpenFileDialog();
                if (fileDialog.ShowDialog().Value)
                {
                    CreateScannedDocumentItem(fileDialog.FileName);
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
            }, () => ValidateBooking());

            SaveStackToDbCommand = new DelegateCommand(SaveStackToDb, () => BookingsOnStack.Count > 0);

            SaveBookingCommand = new DelegateCommand(() =>
            {
                SaveBookingToDB(CreateBookingItem());
                ClearForm();
            }, () => ValidateBooking());

            DeleteSelectedScannedDocumentCommand =
                new DelegateCommand(DeleteSelectedScannedDocument, () => ScannedDocuments.Count > 0);

            CancelCommand = new DelegateCommand(ClearForm);
        }

        private void DeleteSelectedScannedDocument()
        {
            if (SelectedScannedDocument != null)
            {
                ScannedDocuments.Remove(SelectedScannedDocument);
            }
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
            return CostAccountCreditorId != 0 && CostAccountDebitorId != 0 && SelectedTax != null && SelectedCostCenter != null && !string.IsNullOrEmpty(Description);
        }

        private Booking CreateBookingItem()
        {
            if (!ValidateBooking())
            {
                return null;
            }

            var booking = new Booking(Amount, Date, SelectedCostCenter.CostCenterId, Description);

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

            var debit = new Debit();
            var credit = new Credit();

            if (SelectedBookingType == BookingType.Invoice)
            {
                if (SelectedTax.Description.ToLower().Contains("Vorsteuer"))
                {
                    credit = new Credit(amountWithoutTax, CostAccountDebitorId, booking.BookingId);
                    debit = new Debit(amountWithoutTax + tax, CostAccountDebitorId, booking.BookingId);
                    var creditTax = new Credit(tax, SelectedTax.RefCostAccount, booking.BookingId);
                    booking.Credits.Add(creditTax);
                }
                else if (SelectedTax.Description.ToLower().Contains("Umsatzsteuer"))
                {
                    credit = new Credit(amountWithoutTax + tax, CostAccountDebitorId, booking.BookingId);
                    debit = new Debit(amountWithoutTax + tax, CostAccountDebitorId, booking.BookingId);
                    var debitTax = new Debit(tax, SelectedTax.RefCostAccount, booking.BookingId);
                    booking.Debits.Add(debitTax);
                }
                else
                {
                    credit = new Credit(Amount, CostAccountDebitorId, booking.BookingId);
                    debit = new Debit(Amount, CostAccountDebitorId, booking.BookingId);
                }
            }
            else if (SelectedBookingType == BookingType.CreditAdvice)
            {
                if (SelectedTax.Description.ToLower().Contains("Vorsteuer"))
                {
                    credit = new Credit(amountWithoutTax + tax, CostAccountDebitorId, booking.BookingId);
                    debit = new Debit(amountWithoutTax, CostAccountDebitorId, booking.BookingId);
                    var debitTax = new Debit(tax, SelectedTax.RefCostAccount, booking.BookingId);
                    booking.Debits.Add(debitTax);
                }
                else if (SelectedTax.Description.ToLower().Contains("Umsatzsteuer"))
                {
                    credit = new Credit(amountWithoutTax + tax, CostAccountDebitorId, booking.BookingId);
                    debit = new Debit(amountWithoutTax + tax, CostAccountDebitorId, booking.BookingId);
                    var creditTax = new Credit(tax, SelectedTax.RefCostAccount, booking.BookingId);
                    booking.Credits.Add(creditTax);
                }
                else
                {
                    credit = new Credit(Amount, CostAccountDebitorId, booking.BookingId);
                    debit = new Debit(Amount, CostAccountDebitorId, booking.BookingId);
                }
            }

            booking.Debits.Add(debit);
            booking.Credits.Add(credit);
            booking.ScannedDocuments = ScannedDocuments.ToList();

            return booking;
        }

        private void SaveBookingToDB(Booking booking)
        {
            if (booking == null)
            {
                return;
            }

            var bookingId = 0;

            bookingId = DataContext.Instance.Bookings.Insert(booking);
            foreach (var item in booking.Credits)
            {
                item.RefBookingId = bookingId;
                DataContext.Instance.Credits.Insert(item);
            }

            foreach (var item in booking.Debits)
            {
                item.RefBookingId = bookingId;
                DataContext.Instance.Debits.Insert(item);
            }

            foreach (var item in booking.ScannedDocuments)
            {
                item.RefBookingId = bookingId;
                DataContext.Instance.ScannedDocuments.Insert(item);
            }
        }

        private void AddToStack(Booking booking)
        {
            var bookingItem = CreateBookingItem();

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

        private void FilterTaxType()
        {
            FilteredTaxTypes = new SvenTechCollection<TaxType>();
            if (CostAccountCreditor != null && CostAccountCreditor.AccountNumber > 69999)
            {
                FilteredTaxTypes.Add(Globals.CoreData.TaxTypes[0]);
                FilteredTaxTypes.AddRange(
                    Globals.CoreData.TaxTypes.Where(x => x.Description.ToLower().Contains("vorsteuer")));
            }
            else if (CostAccountDebitor != null && CostAccountDebitor.AccountNumber > 9999)
            {
                FilteredTaxTypes.Add(Globals.CoreData.TaxTypes[0]);
                FilteredTaxTypes.AddRange(
                    Globals.CoreData.TaxTypes.Where(x => x.Description.ToLower().Contains("umsatzsteuer")));
            }
            else
            {
                FilteredTaxTypes = Globals.CoreData.TaxTypes;
            }
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
        public User ActualUser => Globals.ActiveUser;

        public CostAccount CostAccountCreditor
        {
            get => _costAccountCreditor;
            set
            {
                _costAccountCreditor = value;
                FilterTaxType();
                SelectedTax = FilteredTaxTypes.Single(x => x.TaxTypeId == _costAccountCreditor.RefTaxTypeId);
            }
        }

        public SvenTechCollection<Booking> BookingsOnStack { get; set; } = new SvenTechCollection<Booking>();
        public string Description { get; set; }
        public CostAccount CostAccountDebitor { get; set; }
        public TaxType SelectedTax { get; set; }
        public List<CostAccount> CostAccounts { get; set; }
        public SvenTechCollection<CostCenterCategory> CostCenterCategories { get; set; } = new SvenTechCollection<CostCenterCategory>();
        public SvenTechCollection<Project> Projects { get; set; } = new SvenTechCollection<Project>();
        public SvenTechCollection<TaxType> FilteredTaxTypes { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public GrossNetType GrossNetType { get; set; }
        public ScannedDocument SelectedScannedDocument { get; set; }
        public BookingType SelectedBookingType { get; set; } = BookingType.Invoice;
        public CostCenter SelectedCostCenter { get; set; }
        public CostCenterCategory SelectedCostCenterCategory { get; set; }

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