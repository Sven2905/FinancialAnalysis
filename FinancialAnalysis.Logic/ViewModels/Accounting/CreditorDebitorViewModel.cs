using DevExpress.Mvvm;

using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Logic.ViewModels.Accounting;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Accounting;
using System.Linq;
using Utilities;
using WebApiWrapper.Accounting;
using WebApiWrapper.ClientManagement;

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

        #endregion Constructor

        #region Fields

        private CustomerType _SelectedCustomerType;
        private Creditor _SelectedCreditor;
        private Debitor _SelectedDebitor;
        private string _CreditorFilterText;
        private string _DebitorFilterText;

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
                Creditors.Delete(SelectedCreditor.CreditorId);
            }

            FilteredCreditors.Remove(SelectedCreditor);
        }

        private void DeleteDebitor()
        {
            if (SelectedDebitor.DebitorId != 0)
            {
                Debitors.Delete(SelectedDebitor.DebitorId);
            }

            FilteredDebitors.Remove(SelectedDebitor);
        }

        private void SaveItem()
        {
            int clientId = SaveClient();

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
                CompanyViewModel.Client.ClientId = Clients.Insert(CompanyViewModel.Client);
            }
            else
            {
                Clients.Update(CompanyViewModel.Client);
            }

            if (CompanyViewModel.SelectedClientType == ClientType.Business)
            {
                if (CompanyViewModel.Client.Company == null)
                {
                    return 0;
                }

                if (CompanyViewModel.Client.Company.CompanyId == 0)
                {
                    CompanyViewModel.Client.Company.RefClientId = CompanyViewModel.Client.ClientId;
                    CompanyViewModel.Client.Company.CompanyId =
                        Companies.Insert(CompanyViewModel.Client.Company);
                }
                else
                {
                    Companies.Update(CompanyViewModel.Client.Company);
                }
            }

            return CompanyViewModel.Client.ClientId;
        }

        private void SaveCreditor(int ClientId)
        {
            if (SelectedCreditor.CreditorId == 0)
            {
                int CreditorNumber = CostAccounts.GetNextCreditorNumber();
                SelectedCreditor.RefClientId = CompanyViewModel.Client.ClientId;
                SelectedCreditor.CostAccount.RefTaxTypeId = CompanyViewModel.SelectedTaxTypeId;
                SelectedCreditor.CostAccount.AccountNumber = CreditorNumber;
                SelectedCreditor.CostAccount.RefCostAccountCategoryId =
                    CostAccountCategories.GetCreditorId();
                SelectedCreditor.CostAccount.Description = CompanyViewModel.Client.Name;
                SelectedCreditor.CostAccount.IsVisible = true;
                SelectedCreditor.RefCostAccountId =
                    CostAccounts.Insert(SelectedCreditor.CostAccount);
                Creditors.Insert(SelectedCreditor);
            }
            else
            {
                Clients.Update(CompanyViewModel.Client);
                if (CompanyViewModel.SelectedClientType == ClientType.Business)
                {
                    Companies.Update(CompanyViewModel.Client.Company);
                }

                CostAccounts.Update(SelectedCreditor.CostAccount);
                Creditors.Update(SelectedCreditor);
            }
        }

        private void SaveDebitor(int ClientId)
        {
            if (SelectedDebitor.DebitorId == 0)
            {
                int DebitorNumber = CostAccounts.GetNextDebitorNumber();
                SelectedDebitor.RefClientId = CompanyViewModel.Client.ClientId;
                SelectedDebitor.CostAccount.RefTaxTypeId = CompanyViewModel.SelectedTaxTypeId;
                SelectedDebitor.CostAccount.AccountNumber = DebitorNumber;
                SelectedDebitor.CostAccount.RefCostAccountCategoryId =
                    CostAccountCategories.GetDebitorId();
                SelectedDebitor.CostAccount.Description = CompanyViewModel.Client.Name;
                SelectedDebitor.CostAccount.IsVisible = true;
                SelectedDebitor.RefCostAccountId =
                    CostAccounts.Insert(SelectedDebitor.CostAccount);
                Debitors.Insert(SelectedDebitor);
            }
            else
            {
                Clients.Update(CompanyViewModel.Client);
                if (CompanyViewModel.SelectedClientType == ClientType.Business)
                {
                    Companies.Update(CompanyViewModel.Client.Company);
                }

                CostAccounts.Update(SelectedDebitor.CostAccount);
                Debitors.Update(SelectedDebitor);
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
                FilteredCreditors.Add(SelectedCreditor);
                CompanyViewModel.Client = SelectedCreditor.Client;
            }
            else
            {
                SelectedDebitor = new Debitor
                {
                    Client = CompanyViewModel.Client
                };
                FilteredDebitors.Add(SelectedDebitor);
            }
        }

        private void RefreshData()
        {
            FilteredCreditors = CreditorList = Creditors.GetAll().ToSvenTechCollection();
            FilteredDebitors = DebitorList = Debitors.GetAll().ToSvenTechCollection();
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

            if (!string.IsNullOrEmpty(CompanyViewModel.Client.Name) &&
                !string.IsNullOrEmpty(CompanyViewModel.Client.Street) &&
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
                return !Creditors.IsCreditorInUse(SelectedCreditor.CreditorId);
            }

            if (SelectedDebitor != null)
            {
                return !Debitors.IsDebitorInUse(SelectedDebitor.DebitorId);
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
        public SvenTechCollection<Creditor> FilteredCreditors { get; set; } = new SvenTechCollection<Creditor>();
        public SvenTechCollection<Debitor> FilteredDebitors { get; set; } = new SvenTechCollection<Debitor>();
        public CompanyViewModel CompanyViewModel { get; set; } = new CompanyViewModel();


        public string CreditorFilterText
        {
            get => _CreditorFilterText;
            set
            {
                _CreditorFilterText = value;
                if (!string.IsNullOrEmpty(value))
                {
                    FilteredCreditors = new SvenTechCollection<Creditor>();
                    FilteredCreditors.AddRange(CreditorList.Where(x => x.Client.Name.ToLower().Contains(_CreditorFilterText.ToLower()) || x.CostAccount.AccountNumber.ToString().Contains(_CreditorFilterText)));
                }
                else
                {
                    FilteredCreditors = CreditorList;
                }
            }
        }


        public string DebitorFilterText
        {
            get => _DebitorFilterText;
            set
            {
                _DebitorFilterText = value;
                if (!string.IsNullOrEmpty(value))
                {
                    FilteredDebitors = new SvenTechCollection<Debitor>();
                    FilteredDebitors.AddRange(DebitorList.Where(x => x.Client.Name.ToLower().Contains(_DebitorFilterText.ToLower()) || x.CostAccount.AccountNumber.ToString().Contains(_DebitorFilterText)));
                }
                else
                {
                    FilteredDebitors = DebitorList;
                }
            }
        }

        public Creditor SelectedCreditor
        {
            get => _SelectedCreditor;
            set
            {
                _SelectedCreditor = value;
                if (value != null)
                {
                    CompanyViewModel.Client = _SelectedCreditor.Client;
                    SelectedDebitor = null;
                    CompanyViewModel.SelectedTaxTypeId = SelectedCreditor.CostAccount.RefTaxTypeId;
                }
                else
                {
                    CompanyViewModel.Client = null;
                }
            }
        }

        public Debitor SelectedDebitor
        {
            get => _SelectedDebitor;
            set
            {
                _SelectedDebitor = value;
                if (value != null)
                {
                    CompanyViewModel.Client = _SelectedDebitor.Client;
                    SelectedCreditor = null;
                    CompanyViewModel.SelectedTaxTypeId = SelectedDebitor.CostAccount.RefTaxTypeId;
                }
                else
                {
                    CompanyViewModel.Client = null;
                }
            }
        }

        public int SelectedTab { get; set; } = 0;
        public SvenTechCollection<Creditor> CreditorList { get; private set; }
        public SvenTechCollection<Debitor> DebitorList { get; private set; }

        #endregion Properties
    }
}