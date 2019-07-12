using DevExpress.Mvvm;
using DevExpress.Xpf.Dialogs;
using FinancialAnalysis.Logic.Manager;
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
using System.Windows.Forms;
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
            Messenger.Default.Register<CreditSplitList>(this, AddCreditSplitsToList);
            Messenger.Default.Register<DebitSplitList>(this, AddDebitSplitsToList);

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
            Credits.Clear();
            Debits.Clear();
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
            OpenDebitSplitWindowCommand = new DelegateCommand(() =>
            {
                Messenger.Default.Send(new OpenDebitSplitWindowMessage(SelectedBookingType, Amount));
            });
            OpenCreditSplitWindowCommand = new DelegateCommand(() =>
            {
                Messenger.Default.Send(new OpenCreditSplitWindowMessage(SelectedBookingType, Amount));
            });

            OpenFileCommand = new DelegateCommand(() =>
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "pdf Dateien (*.pdf)|*.pdf";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    CreateScannedDocumentItem(openFileDialog.FileName);
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
                AccountBookingManager.Instance.NewBookingItem(Date, Description);
                if (Credits.Count == 0 && Debits.Count == 0)
                    AccountBookingManager.Instance.CreateAndAddCreditDebit(GrossNetType, SelectedBookingType, Amount, CostAccountCreditor, CostAccountDebitor, SelectedTax);

                else if (Credits.Count > 0 && Debits.Count == 0)
                {
                    Debits.Add(new Debit(Amount, CostAccountDebitorId, 0));
                    AccountBookingManager.Instance.AddCreditsAndDebits(Credits.ToList(), Debits.ToList());
                }
                else if (Credits.Count == 0 && Debits.Count > 0)
                {
                    Credits.Add(new Credit(Amount, CostAccountCreditorId, 0));
                    AccountBookingManager.Instance.AddCreditsAndDebits(Credits.ToList(), Debits.ToList());
                }
                else
                {
                    AccountBookingManager.Instance.AddCreditsAndDebits(Credits.ToList(), Debits.ToList());
                }

                AccountBookingManager.Instance.AddScannedDocuments(ScannedDocumentList);

                if (IsFixedCostAllocationActive)
                    AccountBookingManager.Instance.AddFixedCostAllocation(SelectedFixedCostAllocation);
                else
                    AccountBookingManager.Instance.AddCostCenter(SelectedCostCenter);

                BookingsOnStack = AccountBookingManager.Instance.BookingList.ToSvenTechCollection();

                    ClearForm();
            }, () => ValidateBooking());


            SaveStackToDbCommand = new DelegateCommand(SaveStackToDb, () => BookingsOnStack.Count > 0);

            DeleteCommand = new DelegateCommand(DeleteBooking, () => SelectedBooking != null);

            DeleteSelectedScannedDocumentCommand =
                new DelegateCommand(DeleteSelectedScannedDocument, () => ScannedDocumentList.Count > 0);

            CancelCommand = new DelegateCommand(ClearForm);
        }

        private void DeleteBooking()
        {
            AccountBookingManager.Instance.RemoveBookingFromList(SelectedBooking.BookingId);
            BookingsOnStack = AccountBookingManager.Instance.BookingList.ToSvenTechCollection();
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
            byte[] file = File.ReadAllBytes(path);

            string[] temp = path.Split('\\');
            string fileName = temp[temp.Length - 1].Replace(".pdf", "").Replace(".PDF", "");

            ScannedDocument scannedDocument = new ScannedDocument
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
            return (CostAccountCreditorId != 0 || Credits.Count > 0) && (CostAccountDebitorId != 0 || Debits.Count > 0) && ValidateBookingMode() && !string.IsNullOrEmpty(Description);
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

        private void SaveStackToDb()
        {
            AccountBookingManager.Instance.SaveBookingsToDB();

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

        private void AddDebitSplitsToList(DebitSplitList debitSplitList)
        {
            Debits.AddRange(debitSplitList.Debits);
        }

        private void AddCreditSplitsToList(CreditSplitList creditSplitList)
        {
            Credits.AddRange(creditSplitList.Credits);
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
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand GetCreditorCommand { get; set; }
        public DelegateCommand GetDebitorCommand { get; set; }
        public DelegateCommand OpenFileCommand { get; set; }
        public DelegateCommand DoubleClickListBoxCommand { get; set; }
        public DelegateCommand DeleteSelectedScannedDocumentCommand { get; set; }
        public DelegateCommand OpenCreditSplitWindowCommand { get; set; }
        public DelegateCommand OpenDebitSplitWindowCommand { get; set; }
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
        public CostAccount CostAccountDebitor { get; set; }
        public TaxType SelectedTax { get; set; }
        public List<CostAccount> CostAccountList { get; set; }
        public SvenTechCollection<CostCenterCategory> CostCenterCategoryList { get; set; } = new SvenTechCollection<CostCenterCategory>();
        public SvenTechCollection<Project> ProjectList { get; set; } = new SvenTechCollection<Project>();
        public SvenTechCollection<TaxType> FilteredTaxTypes { get; set; }
        public SvenTechCollection<FixedCostAllocation> FixedCostAllocationList { get; set; }
        public GrossNetType GrossNetType { get; set; }
        public ScannedDocument SelectedScannedDocument { get; set; }
        public BookingType SelectedBookingType { get; set; }
        public CostCenter SelectedCostCenter { get; set; }
        public CostCenterCategory SelectedCostCenterCategory { get; set; }
        public bool IsFixedCostAllocationActive { get; set; }
        public FixedCostAllocation SelectedFixedCostAllocation { get; set; }
        public Booking SelectedBooking { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Description { get; set; }
        public ObservableCollection<Credit> Credits = new ObservableCollection<Credit>();
        public ObservableCollection<Debit> Debits = new ObservableCollection<Debit>();
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