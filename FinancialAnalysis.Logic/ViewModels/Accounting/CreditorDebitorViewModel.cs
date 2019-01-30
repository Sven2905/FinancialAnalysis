using System.ComponentModel;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Logic.ViewModels.Accounting;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.ClientManagement;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class CreditorDebitorViewModel : ViewModelBase
    {
        #region Constructor

        public CreditorDebitorViewModel()
        {
            if (IsInDesignMode)
                return;

            Creditor = new Creditor();
            Debitor = new Debitor();

            RefreshData();
            InitializeButtonCommands();
        }

        #endregion Constructor;

        #region Fields

        private Creditor _creditor = new Creditor();
        private Debitor _debitor = new Debitor();
        private Client _selectedClient = new Client();
        private bool _saveCreditorButtonEnabled;
        private bool _saveDebitorButtonEnabled;
        private bool _deleteCreditorButtonEnabled;
        private bool _deleteDebitorButtonEnabled;

        private IDocumentManagerService SingleObjectDocumentManagerService =>
            GetService<IDocumentManagerService>("SingleObjectDocumentManagerService");

        #endregion Fields

        #region Methods

        private void RefreshData()
        {
            try
            {
                Creditors = DataLayer.Instance.Creditors.GetAll().ToSvenTechCollection();
                Debitors = DataLayer.Instance.Debitors.GetAll().ToSvenTechCollection();
                Clients = DataLayer.Instance.Clients.GetAll().ToSvenTechCollection();
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        #region Creditor

        private void NewCreditor()
        {
            Creditor = new Creditor();
            CreditorViewModel.Client = new Client();
            Creditors.Add(Creditor);
        }

        private void SaveCreditor()
        {
            if (Creditor.CreditorId == 0)
            {
                if (CreditorViewModel.SelectedClientType == ClientType.Business)
                    Creditor.Client.IsCompany = true;
                else
                    Creditor.Client.IsCompany = false;
                if (CreditorViewModel.Client.ClientId == 0)
                    CreditorViewModel.Client.ClientId = DataLayer.Instance.Clients.Insert(CreditorViewModel.Client);
                else
                    DataLayer.Instance.Clients.Update(CreditorViewModel.Client);
                if (CreditorViewModel.SelectedClientType == ClientType.Business)
                {
                    if (CreditorViewModel.Client.Company.CompanyId == 0)
                    {
                        CreditorViewModel.Client.Company.RefClientId = CreditorViewModel.Client.ClientId;
                        CreditorViewModel.Client.Company.CompanyId = DataLayer.Instance.Companies.Insert(CreditorViewModel.Client.Company);
                    }
                    else
                        DataLayer.Instance.Companies.Update(CreditorViewModel.Client.Company);
                }
                var CreditorNumber = DataLayer.Instance.CostAccounts.GetNextCreditorNumber();
                Creditor.RefClientId = CreditorViewModel.Client.ClientId;
                Creditor.CostAccount.AccountNumber = CreditorNumber;
                Creditor.CostAccount.RefCostAccountCategoryId = DataLayer.Instance.CostAccountCategories.GetCreditorId();
                Creditor.CostAccount.Description = CreditorViewModel.Client.Name;
                Creditor.CostAccount.IsVisible = true;
                Creditor.RefCostAccountId = DataLayer.Instance.CostAccounts.Insert(Creditor.CostAccount);
                DataLayer.Instance.Creditors.Insert(Creditor);
            }
            else
            {
                DataLayer.Instance.Clients.Update(CreditorViewModel.Client);
                if (CreditorViewModel.SelectedClientType == ClientType.Business)
                {
                    DataLayer.Instance.Companies.Update(CreditorViewModel.Client.Company);
                }
                DataLayer.Instance.CostAccounts.Update(Creditor.CostAccount);
                DataLayer.Instance.Creditors.Update(Creditor);
            }

            if (Clients.Contains(CreditorViewModel.Client))
                Clients.Add(CreditorViewModel.Client);
        }

        private void DeleteCreditor()
        {
            if (Creditor.CreditorId != 0)
            {
                DataLayer.Instance.CostAccounts.Delete(Creditor.RefCostAccountId);
                DataLayer.Instance.Creditors.Delete(Creditor.CreditorId);
            }
            Creditors.Remove(Creditor);
        }

        #endregion Creditor

        #region Debitor

        private void NewDebitor()
        {
            Debitor = new Debitor();
            DebitorViewModel.Client = new Client();
            Debitors.Add(Debitor);
        }

        private void SaveDebitor()
        {
            if (Debitor.DebitorId == 0)
            {
                if (DebitorViewModel.SelectedClientType == ClientType.Business)
                    Debitor.Client.IsCompany = true;
                else
                    Debitor.Client.IsCompany = false;
                if (DebitorViewModel.Client.ClientId == 0)
                    DebitorViewModel.Client.ClientId = DataLayer.Instance.Clients.Insert(DebitorViewModel.Client);
                else
                    DataLayer.Instance.Clients.Update(DebitorViewModel.Client);
                if (DebitorViewModel.SelectedClientType == ClientType.Business)
                {
                    if (DebitorViewModel.Client.Company.CompanyId == 0)
                    {
                        DebitorViewModel.Client.Company.RefClientId = DebitorViewModel.Client.ClientId;
                        DebitorViewModel.Client.Company.CompanyId = DataLayer.Instance.Companies.Insert(DebitorViewModel.Client.Company);
                    }
                    else
                        DataLayer.Instance.Companies.Update(DebitorViewModel.Client.Company);
                }
                var DebitorNumber = DataLayer.Instance.CostAccounts.GetNextDebitorNumber();
                Debitor.RefClientId = DebitorViewModel.Client.ClientId;
                Debitor.CostAccount.AccountNumber = DebitorNumber;
                Debitor.CostAccount.RefCostAccountCategoryId = DataLayer.Instance.CostAccountCategories.GetDebitorId();
                Debitor.CostAccount.Description = DebitorViewModel.Client.Name;
                Debitor.CostAccount.IsVisible = true;
                Debitor.RefCostAccountId = DataLayer.Instance.CostAccounts.Insert(Debitor.CostAccount);
                DataLayer.Instance.Debitors.Insert(Debitor);
            }
            else
            {
                DataLayer.Instance.Clients.Update(DebitorViewModel.Client);
                if (DebitorViewModel.SelectedClientType == ClientType.Business)
                {
                    DataLayer.Instance.Companies.Update(DebitorViewModel.Client.Company);
                }
                DataLayer.Instance.CostAccounts.Update(Debitor.CostAccount);
                DataLayer.Instance.Debitors.Update(Debitor);
            }

            if (Clients.Contains(DebitorViewModel.Client))
                Clients.Add(DebitorViewModel.Client);
        }

        private void DeleteDebitor()
        {
            if (Debitor.DebitorId != 0)
            {
                DataLayer.Instance.CostAccounts.Delete(Debitor.RefCostAccountId);
                DataLayer.Instance.Debitors.Delete(Debitor.DebitorId);
            }
            Debitors.Remove(Debitor);
        }

        #endregion Debitor

        private void InitializeButtonCommands()
        {
            SaveCreditorCommand = new DelegateCommand(SaveCreditor, ValidateSaveCreditor());
            DeleteCreditorCommand = new DelegateCommand(DeleteCreditor);
            SaveDebitorCommand = new DelegateCommand(SaveDebitor);
            DeleteDebitorCommand = new DelegateCommand(DeleteDebitor);

            NewCreditorCommand = new DelegateCommand(() => { Creditor = new Creditor(); });
            NewDebitorCommand = new DelegateCommand(() => { Debitor = new Debitor(); });
            OpenClientWindowCommand = new DelegateCommand(() =>
            {
                Messenger.Default.Send(new OpenClientWindowMessage());
            });
        }

        private void UseExistingClient()
        {
            if (SelectedTab == 0)
            {
                Creditor.Client = SelectedClient;
                CreditorViewModel.Client = SelectedClient;
                CreditorViewModel.SelectedTaxType = Globals.CoreData.GetTaxTypeById(Creditor.CostAccount.RefTaxTypeId);
                if (Creditor.Client.IsNull())
                    Creditor.Client = new Client();

                ValidateCreditor();
            }
            else if (SelectedTab == 1)
            {
                Debitor.Client = SelectedClient;
                DebitorViewModel.Client = SelectedClient;
                DebitorViewModel.SelectedTaxType = Debitor.CostAccount.TaxType;
                if (Debitor.Client.IsNull())
                    Debitor.Client = new Client();

                ValidateSaveDebitor();
            }
        }

        #region Validation Methods

        private void ValidateCreditor()
        {
            ValidateSaveCreditor();
            ValidateDeleteCreditor();
        }

        private void ValidateDebitor()
        {
            ValidateSaveDebitor();
            ValidateDeleteDebitor();
        }

        private bool ValidateSaveCreditor()
        {
            if (Creditor.IsNull()) Creditor = new Creditor();

            if (!string.IsNullOrEmpty(Creditor.Client.Name) && !string.IsNullOrEmpty(Creditor.Client.Street) &&
                Creditor.Client.Postcode != 0 && !string.IsNullOrEmpty(Creditor.Client.City))
            {
                SaveCreditorButtonEnabled = true;
                return true;
            }

            SaveCreditorButtonEnabled = false;
            return false;
        }

        private void ValidateDeleteCreditor()
        {
            if (Creditor.IsNull()) Creditor = new Creditor();

            if (Creditor.CreditorId != 0)
                DeleteCreditorButtonEnabled = !DataLayer.Instance.Creditors.IsCreditorInUse(Creditor.CreditorId);
            else
                DeleteCreditorButtonEnabled = false;
        }

        private void ValidateSaveDebitor()
        {
            if (Debitor.IsNull()) Debitor = new Debitor();

            if (!string.IsNullOrEmpty(Debitor.Client.Name) && !string.IsNullOrEmpty(Debitor.Client.Street) &&
                Debitor.Client.Postcode != 0 && !string.IsNullOrEmpty(Debitor.Client.City))
            {
                SaveDebitorButtonEnabled = true;
                return;
            }

            SaveDebitorButtonEnabled = false;
        }

        private void ValidateDeleteDebitor()
        {
            if (Debitor.DebitorId != 0)
                DeleteDebitorButtonEnabled = !DataLayer.Instance.Debitors.IsDebitorInUse(Debitor.DebitorId);
            else
                DeleteDebitorButtonEnabled = false;
        }

        #endregion Validation Methods

        #endregion Methods

        #region Properties

        public DelegateCommand SaveCreditorCommand { get; set; }
        public DelegateCommand SaveDebitorCommand { get; set; }
        public DelegateCommand DeleteCreditorCommand { get; set; }
        public DelegateCommand DeleteDebitorCommand { get; set; }
        public DelegateCommand NewCreditorCommand { get; set; }
        public DelegateCommand NewDebitorCommand { get; set; }
        public DelegateCommand OpenClientWindowCommand { get; set; }

        public SvenTechCollection<Client> Clients { get; set; } = new SvenTechCollection<Client>();
        public SvenTechCollection<Creditor> Creditors { get; set; } = new SvenTechCollection<Creditor>();
        public SvenTechCollection<Debitor> Debitors { get; set; } = new SvenTechCollection<Debitor>();
        public CompanyViewModel CreditorViewModel { get; set; } = new CompanyViewModel();
        public CompanyViewModel DebitorViewModel { get; set; } = new CompanyViewModel();

        public int SelectedTab { get; set; } = 0;

        private Client _SelectedClient;

        public Client SelectedClient
        {
            get { return _SelectedClient; }
            set { _SelectedClient = value; UseExistingClient(); }
        }


        public Creditor Creditor
        {
            get => _creditor;
            set
            {
                _creditor = value;
                CreditorViewModel.Client = _creditor.Client;
                if (_creditor.Client.IsCompany)
                    CreditorViewModel.SelectedClientType = ClientType.Business;
                else
                    CreditorViewModel.SelectedClientType = ClientType.Private;
                ValidateCreditor();
            }
        }

        public Debitor Debitor
        {
            get { DebitorViewModel.Client = _debitor.Client; return _debitor; }
            set
            {
                _debitor = value;
                DebitorViewModel.Client = _debitor.Client;
                if (_debitor.Client.IsCompany)
                    DebitorViewModel.SelectedClientType = ClientType.Business;
                else
                    DebitorViewModel.SelectedClientType = ClientType.Private;
                ValidateDebitor();
            }
        }

        #region Validation Properties

        public bool SaveCreditorButtonEnabled
        {
            get => _saveCreditorButtonEnabled;
            set
            {
                _saveCreditorButtonEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool SaveDebitorButtonEnabled
        {
            get => _saveDebitorButtonEnabled;
            set
            {
                _saveDebitorButtonEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool DeleteCreditorButtonEnabled
        {
            get => _deleteCreditorButtonEnabled;
            set
            {
                _deleteCreditorButtonEnabled = value;
                RaisePropertyChanged();
            }
        }

        public bool DeleteDebitorButtonEnabled
        {
            get => _deleteDebitorButtonEnabled;
            set
            {
                _deleteDebitorButtonEnabled = value;
                RaisePropertyChanged();
            }
        }

        #endregion Validation Properties

        #endregion Properties
    }
}