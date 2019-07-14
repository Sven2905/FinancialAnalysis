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

        private int costAccountCreditorId;
        private int costAccountDebitorId;
        private decimal amount;
        private CostAccount costAccountCreditor;
        private GrossNetType grossNetType;

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

        #region Clear & Delete

        private void ClearForm()
        {
            Amount = 0;
            Description = "";
            ScannedDocumentList.Clear();
            Credits.Clear();
            Debits.Clear();
        }

        private void ClearCredits()
        {
            Credits.Clear();
            IsEnabledCreditorDropDown = true;
            RaisePropertyChanged("CreditsDisplay");
            if (IsEnabledCreditorDropDown && IsEnabledDebitorDropDown)
                IsEnabledTaxDropDown = true;
        }

        private void ClearDebits()
        {
            Debits.Clear();
            IsEnabledDebitorDropDown = true;
            RaisePropertyChanged("DebitsDisplay");
            if (IsEnabledCreditorDropDown && IsEnabledDebitorDropDown)
                IsEnabledTaxDropDown = true;
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

        #endregion Clear & Delete

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
            }, () => Amount != 0);
            OpenCreditSplitWindowCommand = new DelegateCommand(() =>
            {
                Messenger.Default.Send(new OpenCreditSplitWindowMessage(SelectedBookingType, Amount));
            }, () => Amount != 0);

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
            ClearCreditsCommand = new DelegateCommand(ClearCredits, () => Credits.Count > 0);
            ClearDebitsCommand = new DelegateCommand(ClearDebits, () => Debits.Count > 0);
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
            RaisePropertyChanged("ScannedDocuments");
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
            CostAccountDebitorId = 0;
            Debits.AddRange(debitSplitList.Debits);
            RaisePropertyChanged("DebitsDisplay");
            IsEnabledDebitorDropDown = false;
            IsEnabledTaxDropDown = false;
            SelectedTax = FilteredTaxTypes.First();
        }

        private void AddCreditSplitsToList(CreditSplitList creditSplitList)
        {
            CostAccountCreditorId = 0;
            Credits.AddRange(creditSplitList.Credits);
            RaisePropertyChanged("CreditsDisplay");
            IsEnabledCreditorDropDown = false;
            IsEnabledTaxDropDown = false;
            SelectedTax = FilteredTaxTypes.First();
        }

        #endregion Methods

        #region Properties

        #region Commands

        public DelegateCommand AddToStackCommand { get; set; }
        public DelegateCommand SaveStackToDbCommand { get; set; }
        public DelegateCommand CancelCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand GetCreditorCommand { get; set; }
        public DelegateCommand GetDebitorCommand { get; set; }
        public DelegateCommand ClearDebitsCommand { get; set; }
        public DelegateCommand ClearCreditsCommand { get; set; }
        public DelegateCommand OpenFileCommand { get; set; }
        public DelegateCommand DoubleClickListBoxCommand { get; set; }
        public DelegateCommand DeleteSelectedScannedDocumentCommand { get; set; }
        public DelegateCommand OpenCreditSplitWindowCommand { get; set; }
        public DelegateCommand OpenDebitSplitWindowCommand { get; set; }

        #endregion Commands

        #region Collections

        public ObservableCollection<Credit> CreditsDisplay
        {
            get
            {
                if (Credits != null && Credits.Count > 0)
                {
                    var tempCredits = new ObservableCollection<Credit>();
                    foreach (var item in Credits.OrderBy(x => x.CostAccount.AccountNumber))
                    {
                        var exists = tempCredits.SingleOrDefault(x => x.RefCostAccountId == item.RefCostAccountId);
                        if (exists != null)
                            exists.Amount += item.Amount;
                        else
                            tempCredits.Add(item);
                    }
                    return tempCredits;
                }
                else if (CostAccountCreditorId != 0 && costAccountDebitorId != 0)
                {
                    AccountBookingManager.Instance.CreateCreditDebit(GrossNetType, SelectedBookingType, Amount, CostAccountCreditor, CostAccountDebitor, SelectedTax, out List<Credit> creditList, out List<Debit> debitList);
                    var tempCredits = new ObservableCollection<Credit>();
                    foreach (var item in creditList.OrderBy(x => x.CostAccount.AccountNumber))
                    {
                        var exists = tempCredits.SingleOrDefault(x => x.RefCostAccountId == item.RefCostAccountId);
                        if (exists != null)
                            exists.Amount += item.Amount;
                        else
                            tempCredits.Add(item);
                    }
                    return tempCredits;
                }
                else if (CostAccountCreditorId != 0)
                    return AccountBookingManager.Instance.CreateCredits(GrossNetType, SelectedTax, Amount, CostAccountList.Single(x => x.CostAccountId == CostAccountCreditorId)).OrderBy(x => x.CostAccount.AccountNumber).ToOberservableCollection();
                else return new ObservableCollection<Credit>();
            }
        }
        public ObservableCollection<Debit> DebitsDisplay
        {
            get
            {
                if (Debits != null && Debits.Count > 0)
                {
                    var tempDebits = new ObservableCollection<Debit>();
                    foreach (var item in Debits.OrderBy(x => x.CostAccount.AccountNumber))
                    {
                        var exists = tempDebits.SingleOrDefault(x => x.RefCostAccountId == item.RefCostAccountId);
                        if (exists != null)
                            exists.Amount += item.Amount;
                        else
                            tempDebits.Add(item);
                    }
                    return tempDebits;
                }
                else if (CostAccountCreditorId != 0 && costAccountDebitorId != 0)
                {
                    AccountBookingManager.Instance.CreateCreditDebit(GrossNetType, SelectedBookingType, Amount, CostAccountCreditor, CostAccountDebitor, SelectedTax, out List<Credit> creditList, out List<Debit> debitList);
                    var tempDebits = new ObservableCollection<Debit>();
                    foreach (var item in debitList.OrderBy(x => x.CostAccount.AccountNumber))
                    {
                        var exists = tempDebits.SingleOrDefault(x => x.RefCostAccountId == item.RefCostAccountId);
                        if (exists != null)
                            exists.Amount += item.Amount;
                        else
                            tempDebits.Add(item);
                    }
                    return tempDebits;
                }
                else if (CostAccountDebitorId != 0)
                    return AccountBookingManager.Instance.CreateDebits(GrossNetType, SelectedTax, Amount, CostAccountList.Single(x => x.CostAccountId == CostAccountDebitorId)).OrderBy(x => x.CostAccount.AccountNumber).ToOberservableCollection();
                else return new ObservableCollection<Debit>();
            }
        }
        public SvenTechCollection<CostCenterCategory> CostCenterCategoryList { get; set; } = new SvenTechCollection<CostCenterCategory>();
        public SvenTechCollection<Project> ProjectList { get; set; } = new SvenTechCollection<Project>();
        public SvenTechCollection<TaxType> FilteredTaxTypes { get; set; }
        public SvenTechCollection<FixedCostAllocation> FixedCostAllocationList { get; set; }
        public ObservableCollection<Credit> Credits { get; set; } = new ObservableCollection<Credit>();
        public ObservableCollection<Debit> Debits { get; set; } = new ObservableCollection<Debit>();
        public ObservableCollection<ScannedDocument> ScannedDocumentList { get; set; } = new ObservableCollection<ScannedDocument>();

        #endregion Collections

        public int CostAccountCreditorId
        {
            get => costAccountCreditorId;
            set
            {
                costAccountCreditorId = value;
                if (value != 0)
                    CostAccountCreditor = CostAccountList.Single(x => x.CostAccountId == value);
                else
                    CostAccountCreditor = null;

                Credits.Clear();
                RaisePropertyChanged("CreditsDisplay");
            }
        }
        public int CostAccountDebitorId
        {
            get => costAccountDebitorId;
            set
            {
                costAccountDebitorId = value;
                if (value != 0)
                    CostAccountDebitor = CostAccountList.Single(x => x.CostAccountId == value);
                else
                    CostAccountDebitor = null;
                Debits.Clear();
                RaisePropertyChanged("DebitsDisplay");
            }
        }
        public User ActualUser => Globals.ActiveUser;
        public CostAccount CostAccountCreditor
        {
            get => costAccountCreditor;
            set
            {
                costAccountCreditor = value;
                FilterTaxType();
                if (costAccountCreditor != null)
                    SelectedTax = FilteredTaxTypes.Single(x => x.TaxTypeId == costAccountCreditor.RefTaxTypeId);
            }
        }
        public SvenTechCollection<Booking> BookingsOnStack { get; set; } = new SvenTechCollection<Booking>();
        public CostAccount CostAccountDebitor { get; set; }
        public TaxType SelectedTax { get; set; }
        public List<CostAccount> CostAccountList { get; set; }

        public GrossNetType GrossNetType
        {
            get { return grossNetType; }
            set
            {
                grossNetType = value;
                RaisePropertyChanged("CreditsDisplay");
                RaisePropertyChanged("DebitsDisplay");
            }
        }

        public ScannedDocument SelectedScannedDocument { get; set; }
        public BookingType SelectedBookingType { get; set; }
        public CostCenter SelectedCostCenter { get; set; }
        public CostCenterCategory SelectedCostCenterCategory { get; set; }
        public bool IsFixedCostAllocationActive { get; set; }
        public FixedCostAllocation SelectedFixedCostAllocation { get; set; }
        public Booking SelectedBooking { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
        public string Description { get; set; }
        public bool IsEnabledCreditorDropDown { get; set; } = true;
        public bool IsEnabledDebitorDropDown { get; set; } = true;
        public bool IsEnabledTaxDropDown { get; set; } = true;
        public decimal Amount
        {
            get => Math.Round(amount, 2);
            set
            {
                amount = value;
                RaisePropertyChanged();
                RaisePropertyChanged("CreditsDisplay");
                RaisePropertyChanged("DebitsDisplay");
            }
        }

        #endregion Properties
    }
}