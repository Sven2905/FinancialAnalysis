using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Logic.ViewModels.Accounting;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Accounting;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class CreditorDebitorViewModel : ViewModelBase
    {
        #region Constructor

        public CreditorDebitorViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }

            CompanyViewModel.SelectedCustomerTypeChanged += CompanyViewModel_SelectedCustomerTypeChanged;
            NewCommand = new DelegateCommand(NewItem);
            SaveCommand = new DelegateCommand(SaveItem, () => ValidateSave());
            DeleteCommand = new DelegateCommand(DeleteItem, () => ValidateDelete());

            RefreshData();
        }

        private void CompanyViewModel_SelectedCustomerTypeChanged(CustomerType customerType)
        {
            _SelectedCustomerType = customerType;
        }

        #endregion Constructor;

        #region Fields

        private CustomerType _SelectedCustomerType;
        private Creditor _SelectedCreditor;
        private Debitor _SelectedDebitor;

        #endregion Fields

        #region Methods

        private void DeleteItem()
        {
            if (SelectedCreditor != null)
            {
                DeleteCreditor();
            }
            else
            {
                DeleteDebitor();
            }
        }

        private void DeleteCreditor()
        {
            if (SelectedCreditor.CreditorId != 0)
            {
                DataLayer.Instance.Creditors.Delete(SelectedCreditor.CreditorId);
            }
            Creditors.Remove(SelectedCreditor);
        }

        private void DeleteDebitor()
        {
            if (SelectedDebitor.DebitorId != 0)
            {
                DataLayer.Instance.Debitors.Delete(SelectedDebitor.DebitorId);
            }
            Debitors.Remove(SelectedDebitor);
        }

        private void SaveItem()
        {
            var clientId = SaveClient();

            if (SelectedCreditor != null)
            {
                SaveCreditor(clientId);
            }
            else
            {
                SaveDebitor(clientId);
            }
        }

        private int SaveClient()
        {
            if (CompanyViewModel.SelectedClientType == ClientType.Business)
            {
                CompanyViewModel.Client.IsCompany = true;
            }
            else
            {
                CompanyViewModel.Client.IsCompany = false;
            }

            if (CompanyViewModel.Client.ClientId == 0)
            {
                CompanyViewModel.Client.ClientId = DataLayer.Instance.Clients.Insert(CompanyViewModel.Client);
            }
            else
            {
                DataLayer.Instance.Clients.Update(CompanyViewModel.Client);
            }

            if (CompanyViewModel.SelectedClientType == ClientType.Business)
            {
                if (CompanyViewModel.Client.Company.CompanyId == 0)
                {
                    CompanyViewModel.Client.Company.RefClientId = CompanyViewModel.Client.ClientId;
                    CompanyViewModel.Client.Company.CompanyId = DataLayer.Instance.Companies.Insert(CompanyViewModel.Client.Company);
                }
                else
                {
                    DataLayer.Instance.Companies.Update(CompanyViewModel.Client.Company);
                }
            }

            return CompanyViewModel.Client.ClientId;
        }

        private void SaveCreditor(int ClientId)
        {
            if (SelectedCreditor.CreditorId == 0)
            {

                var CreditorNumber = DataLayer.Instance.CostAccounts.GetNextCreditorNumber();
                SelectedCreditor.RefClientId = CompanyViewModel.Client.ClientId;
                SelectedCreditor.CostAccount.RefTaxTypeId = CompanyViewModel.SelectedTaxTypeId;
                SelectedCreditor.CostAccount.AccountNumber = CreditorNumber;
                SelectedCreditor.CostAccount.RefCostAccountCategoryId = DataLayer.Instance.CostAccountCategories.GetCreditorId();
                SelectedCreditor.CostAccount.Description = CompanyViewModel.Client.Name;
                SelectedCreditor.CostAccount.IsVisible = true;
                SelectedCreditor.RefCostAccountId = DataLayer.Instance.CostAccounts.Insert(SelectedCreditor.CostAccount);
                DataLayer.Instance.Creditors.Insert(SelectedCreditor);
            }
            else
            {
                DataLayer.Instance.Clients.Update(CompanyViewModel.Client);
                if (CompanyViewModel.SelectedClientType == ClientType.Business)
                {
                    DataLayer.Instance.Companies.Update(CompanyViewModel.Client.Company);
                }
                DataLayer.Instance.CostAccounts.Update(SelectedCreditor.CostAccount);
                DataLayer.Instance.Creditors.Update(SelectedCreditor);
            }
        }

        private void SaveDebitor(int ClientId)
        {
            if (SelectedDebitor.DebitorId == 0)
            {
                var DebitorNumber = DataLayer.Instance.CostAccounts.GetNextDebitorNumber();
                SelectedDebitor.RefClientId = CompanyViewModel.Client.ClientId;
                SelectedDebitor.CostAccount.RefTaxTypeId = CompanyViewModel.SelectedTaxTypeId;
                SelectedDebitor.CostAccount.AccountNumber = DebitorNumber;
                SelectedDebitor.CostAccount.RefCostAccountCategoryId = DataLayer.Instance.CostAccountCategories.GetDebitorId();
                SelectedDebitor.CostAccount.Description = CompanyViewModel.Client.Name;
                SelectedDebitor.CostAccount.IsVisible = true;
                SelectedDebitor.RefCostAccountId = DataLayer.Instance.CostAccounts.Insert(SelectedDebitor.CostAccount);
                DataLayer.Instance.Debitors.Insert(SelectedDebitor);
            }
            else
            {
                DataLayer.Instance.Clients.Update(CompanyViewModel.Client);
                if (CompanyViewModel.SelectedClientType == ClientType.Business)
                {
                    DataLayer.Instance.Companies.Update(CompanyViewModel.Client.Company);
                }
                DataLayer.Instance.CostAccounts.Update(SelectedDebitor.CostAccount);
                DataLayer.Instance.Debitors.Update(SelectedDebitor);
            }
        }

        private void NewItem()
        {
            CompanyViewModel.Clear();

            if (_SelectedCustomerType == CustomerType.Creditor)
            {
                SelectedCreditor = new Creditor
                {
                    Client = CompanyViewModel.Client
                };
                Creditors.Add(SelectedCreditor);
                CompanyViewModel.Client = SelectedCreditor.Client;
            }
            else
            {
                SelectedDebitor = new Debitor();
                SelectedDebitor.Client = CompanyViewModel.Client;
                Debitors.Add(SelectedDebitor);
            }
        }

        private void RefreshData()
        {
            try
            {
                Creditors = DataLayer.Instance.Creditors.GetAll().ToSvenTechCollection();
                Debitors = DataLayer.Instance.Debitors.GetAll().ToSvenTechCollection();
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void InitializeButtonCommands()
        {

            OpenClientWindowCommand = new DelegateCommand(() =>
            {
                Messenger.Default.Send(new OpenClientWindowMessage());
            });
        }

        #region Validation Methods

        private bool ValidateSave()
        {
            if (CompanyViewModel.Client == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(CompanyViewModel.Client.Name) && !string.IsNullOrEmpty(CompanyViewModel.Client.Street) &&
           CompanyViewModel.Client.Postcode != 0 && !string.IsNullOrEmpty(CompanyViewModel.Client.City))
            {
                return true;
            }
            return false;
        }

        private bool ValidateDelete()
        {
            if (SelectedCreditor == null && SelectedDebitor == null)
            {
                return false;
            }

            if (SelectedCreditor != null)
            {
                return !DataLayer.Instance.Creditors.IsCreditorInUse(SelectedCreditor.CreditorId);
            }

            if (SelectedDebitor != null)
            {
                return !DataLayer.Instance.Debitors.IsDebitorInUse(SelectedDebitor.DebitorId);
            }
            return false;
        }

        #endregion Validation Methods

        #endregion Methods

        #region Properties

        public DelegateCommand NewCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand DeleteCommand { get; set; }
        public DelegateCommand OpenClientWindowCommand { get; set; }
        public SvenTechCollection<Creditor> Creditors { get; set; } = new SvenTechCollection<Creditor>();
        public SvenTechCollection<Debitor> Debitors { get; set; } = new SvenTechCollection<Debitor>();
        public CompanyViewModel CompanyViewModel { get; set; } = new CompanyViewModel();


        public Creditor SelectedCreditor
        {
            get { return _SelectedCreditor; }
            set
            {
                _SelectedCreditor = value;
                if (value != null)
                {
                    CompanyViewModel.Client = value.Client; SelectedDebitor = null;
                    CompanyViewModel.SelectedTaxTypeId = SelectedCreditor.CostAccount.RefTaxTypeId;
                }
            }
        }

        public Debitor SelectedDebitor
        {
            get { return _SelectedDebitor; }
            set
            {
                _SelectedDebitor = value;
                if (value != null)
                {
                    CompanyViewModel.Client = value.Client; SelectedCreditor = null;
                    CompanyViewModel.SelectedTaxTypeId = SelectedDebitor.CostAccount.RefTaxTypeId;
                }
            }
        }

        public int SelectedTab { get; set; } = 0;

        #endregion Properties
    }
}