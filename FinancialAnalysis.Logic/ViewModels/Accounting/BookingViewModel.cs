using DevExpress.Mvvm;
using DevExpress.Xpf.Dialogs;
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
using WebApiWrapper.Accounting;
using WebApiWrapper.ProjectManagement;

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

            GetData();
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

        private void GetData()
        {
            FilteredTaxTypes = new SvenTechCollection<TaxType>(Globals.CoreData.TaxTypeList);
            CostAccountList = CostAccounts.GetAllVisible().ToList();
            CostCenterCategoryList = CostCenterCategories.GetAll().ToSvenTechCollection();
            ProjectList = Projects.GetAll().ToSvenTechCollection();
            FixedCostAllocationList = FixedCostAllocations.GetAll().ToSvenTechCollection();
        }

        private void ClearForm()
        {
            Amount = 0;
            Description = "";
            ScannedDocumentList.Clear();
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
                new DelegateCommand(DeleteSelectedScannedDocument, () => ScannedDocumentList.Count > 0);

            CancelCommand = new DelegateCommand(ClearForm);
        }

        private void DeleteSelectedScannedDocument()
        {
            if (SelectedScannedDocument != null)
            {
                ScannedDocumentList.Remove(SelectedScannedDocument);
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

            ScannedDocumentList.Add(scannedDocument);
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
            return CostAccountCreditorId != 0 && CostAccountDebitorId != 0 && SelectedTax != null && ValidateBookingMode() && !string.IsNullOrEmpty(Description);
        }

        private bool ValidateBookingMode()
        {
            if (IsFixedCostAllocationActive)
            {
                if (SelectedFixedCostAllocation != null)
                {
                    return true;
                }
                return false;
            }
            else
            {
                if (SelectedCostCenter != null)
                {
                    return true;
                }
                return false;
            }
        }

        private Booking CreateBookingItem()
        {
            if (!ValidateBooking())
            {
                return null;
            }

            Booking booking = new Booking(Amount * (-1), Date, Description);

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
                if (SelectedTax.Description.IndexOf("Vorsteuer", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    credit = new Credit(amountWithoutTax, CostAccountCreditorId, booking.BookingId);
                    debit = new Debit((amountWithoutTax + tax) * (-1), CostAccountDebitorId, booking.BookingId);
                    var creditTax = new Credit(tax, SelectedTax.RefCostAccount, booking.BookingId);
                    booking.Credits.Add(creditTax);
                }
                else if (SelectedTax.Description.IndexOf("Umsatzsteuer", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    credit = new Credit(amountWithoutTax + tax, CostAccountCreditorId, booking.BookingId);
                    debit = new Debit((amountWithoutTax + tax) * (-1), CostAccountDebitorId, booking.BookingId);
                    var debitTax = new Debit(tax, SelectedTax.RefCostAccount, booking.BookingId);
                    booking.Debits.Add(debitTax);
                }
                else
                {
                    credit = new Credit(Amount, CostAccountCreditorId, booking.BookingId);
                    debit = new Debit(Amount, CostAccountDebitorId, booking.BookingId);
                }
            }
            else if (SelectedBookingType == BookingType.CreditAdvice)
            {
                if (SelectedTax.Description.IndexOf("Vorsteuer", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    credit = new Credit((amountWithoutTax + tax) * (-1), CostAccountDebitorId, booking.BookingId);
                    debit = new Debit(amountWithoutTax, CostAccountCreditorId, booking.BookingId);
                    var debitTax = new Debit(tax, SelectedTax.RefCostAccount, booking.BookingId);
                    booking.Debits.Add(debitTax);
                }
                else if (SelectedTax.Description.IndexOf("Umsatzsteuer", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    credit = new Credit(amountWithoutTax, CostAccountDebitorId, booking.BookingId);
                    debit = new Debit((amountWithoutTax + tax) * (-1), CostAccountCreditorId, booking.BookingId);
                    var creditTax = new Credit(tax, SelectedTax.RefCostAccount, booking.BookingId);
                    booking.Credits.Add(creditTax);
                }
                else
                {
                    credit = new Credit(Amount, CostAccountDebitorId, booking.BookingId);
                    debit = new Debit(Amount * (-1), CostAccountCreditorId, booking.BookingId);
                }
            }

            booking.Debits.Add(debit);
            booking.Credits.Add(credit);
            booking.ScannedDocuments = ScannedDocumentList.ToList();

            if (IsFixedCostAllocationActive)
            {
                booking.RefFixedCostAllocationId = SelectedFixedCostAllocation.FixedCostAllocationId;
                foreach (var item in SelectedFixedCostAllocation.FixedCostAllocationDetails)
                {
                    booking.BookingCostCenterMappingList.Add(new BookingCostCenterMapping(0, item.RefCostCenterId, booking.Amount * (decimal)(item.Shares / SelectedFixedCostAllocation.Shares.Sum())));
                }
            }
            else
            {
                booking.RefFixedCostAllocationId = 0;
                booking.BookingCostCenterMappingList.Add(new BookingCostCenterMapping(0, SelectedCostCenter.CostCenterId, booking.Amount));
            }

            return booking;
        }

        private void SaveBookingToDB(Booking booking)
        {
            if (booking == null)
            {
                return;
            }

            var bookingId = Bookings.Insert(booking);
            if (bookingId == 0)
            {
                return;
            }

            foreach (var item in booking.Credits)
            {
                item.RefBookingId = bookingId;
            }
            Credits.Insert(booking.Credits);

            foreach (var item in booking.Debits)
            {
                item.RefBookingId = bookingId;
            }
            Debits.Insert(booking.Debits);

            foreach (var item in booking.ScannedDocuments)
            {
                item.RefBookingId = bookingId;
            }
            ScannedDocuments.Insert(booking.ScannedDocuments);

            foreach (var item in booking.BookingCostCenterMappingList)
            {
                item.RefBookingId = bookingId;
            }
            BookingCostCenterMappings.Insert(booking.BookingCostCenterMappingList);
        }

        private void AddToStack(Booking booking)
        {
            if (booking != null)
            {
                BookingsOnStack.Add(booking);
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
            if (CostAccountCreditor?.AccountNumber > 69999)
            {
                FilteredTaxTypes.Add(Globals.CoreData.TaxTypeList[0]);
                FilteredTaxTypes.AddRange(
                    Globals.CoreData.TaxTypeList.Where(x => x.Description.IndexOf("vorsteuer", StringComparison.OrdinalIgnoreCase) >= 0));
            }
            else if (CostAccountDebitor?.AccountNumber > 9999)
            {
                FilteredTaxTypes.Add(Globals.CoreData.TaxTypeList[0]);
                FilteredTaxTypes.AddRange(
                    Globals.CoreData.TaxTypeList.Where(x => x.Description.IndexOf("umsatzsteuer", StringComparison.OrdinalIgnoreCase) >= 0));
            }
            else
            {
                FilteredTaxTypes = Globals.CoreData.TaxTypeList;
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
                CostAccountCreditor = CostAccountList.Single(x => x.CostAccountId == value);
            }
        }

        public int CostAccountDebitorId
        {
            get => _costAccountDebitorId;
            set
            {
                _costAccountDebitorId = value;
                CostAccountDebitor = CostAccountList.Single(x => x.CostAccountId == value);
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
        public List<CostAccount> CostAccountList { get; set; }
        public SvenTechCollection<CostCenterCategory> CostCenterCategoryList { get; set; } = new SvenTechCollection<CostCenterCategory>();
        public SvenTechCollection<Project> ProjectList { get; set; } = new SvenTechCollection<Project>();
        public SvenTechCollection<TaxType> FilteredTaxTypes { get; set; }
        public SvenTechCollection<FixedCostAllocation> FixedCostAllocationList { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public GrossNetType GrossNetType { get; set; }
        public ScannedDocument SelectedScannedDocument { get; set; }
        public BookingType SelectedBookingType { get; set; }
        public CostCenter SelectedCostCenter { get; set; }
        public CostCenterCategory SelectedCostCenterCategory { get; set; }
        public bool IsFixedCostAllocationActive { get; set; }
        public FixedCostAllocation SelectedFixedCostAllocation { get; set; }

        public ObservableCollection<ScannedDocument> ScannedDocumentList { get; set; } =
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