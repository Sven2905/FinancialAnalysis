using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Accounting;
using System;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class CreditorDebitorViewModel : ViewModelBase
    {
        #region Fields

        private Creditor _Creditor = new Creditor();
        private Debitor _Debitor = new Debitor();
        private Company _SelectedCompany = new Company();
        private bool _SaveCreditorButtonEnabled;
        private bool _SaveDebitorButtonEnabled;
        private bool _DeleteCreditorButtonEnabled;
        private bool _DeleteDebitorButtonEnabled;

        private IDocumentManagerService SingleObjectDocumentManagerService { get { return GetService<IDocumentManagerService>("SignleObjectDocumentManagerService"); } }

        #endregion Fields

        #region Constructor

        public CreditorDebitorViewModel()
        {
            Creditor = new Creditor();
            Debitor = new Debitor();
            Creditor.Company.PropertyChanged += CreditorCompany_PropertyChanged;
            Debitor.Company.PropertyChanged += DebitorCompany_PropertyChanged;

            RefreshData();
            InitializeButtonCommands();
        }



        #endregion Constructor;

        #region Methods

        private void SaveCreditor()
        {
            if (Creditor.Company.CompanyId != 0)
            {
                UpdateCompany(Creditor.Company);
                if (Creditor.CreditorId != 0)
                {
                    UpdateCreditor();
                }
                else
                {
                    CreateCreditor();
                }
            }
            else
            {
                CreateCreditorWithCompany();
            }

            RefreshData();
        }

        private void CreateCreditor()
        {
            using (DataLayer db = new DataLayer())
            {
                var creditorNumber = db.CostAccounts.GetNextCreditorNumber();
                Creditor.CostAccount.AccountNumber = creditorNumber;
                Creditor.CostAccount.RefCostAccountCategoryId = db.CostAccountCategories.GetCreditorId();
                Creditor.CostAccount.Description = Creditor.Company.Name;
                Creditor.CostAccount.IsVisible = true;
                var costAccountId = db.CostAccounts.Insert(Creditor.CostAccount);
                var creditor = new Creditor() { RefCompanyId = SelectedCompany.CompanyId, RefCostAccountId = costAccountId };
                db.Creditors.Insert(creditor);
            }

            var notificationService = this.GetRequiredService<INotificationService>();
            var notification = notificationService.CreatePredefinedNotification("Neuer Kreditor", $"Der Kreditor {Creditor.Company.Name} wurde erfolgreich angelegt.", string.Empty);
            notification.ShowAsync();
        }

        private void UpdateCreditor()
        {
            using (DataLayer db = new DataLayer())
            {
                db.Creditors.Update(Creditor);
            }
        }

        private void RefreshData()
        {
            using (DataLayer db = new DataLayer())
            {
                Creditors = db.Creditors.GetAll().ToSvenTechCollection();
                Debitors = db.Debitors.GetAll().ToSvenTechCollection();
                TaxTypes = db.TaxTypes.GetAll().ToSvenTechCollection();
                Companies = db.Companies.GetAll().ToSvenTechCollection();
            }
        }

        private void CreateCreditorWithCompany()
        {
            using (DataLayer db = new DataLayer())
            {
                var companyId = db.Companies.Insert(Creditor.Company);
                var creditorNumber = db.CostAccounts.GetNextCreditorNumber();
                Creditor.CostAccount.AccountNumber = creditorNumber;
                Creditor.CostAccount.RefCostAccountCategoryId = db.CostAccountCategories.GetCreditorId();
                Creditor.CostAccount.Description = Creditor.Company.Name;
                Creditor.CostAccount.IsVisible = true;
                var costAccountId = db.CostAccounts.Insert(Creditor.CostAccount);
                var creditor = new Creditor() { RefCompanyId = companyId, RefCostAccountId = costAccountId };
                db.Creditors.Insert(creditor);
            }

            var notificationService = this.GetRequiredService<INotificationService>();
            var notification = notificationService.CreatePredefinedNotification("Neuer Kreditor", $"Der Kreditor {Creditor.Company.Name} wurde erfolgreich angelegt.", string.Empty);
            notification.ShowAsync();
        }

        private void SaveDebitor()
        {
            if (Debitor.Company.CompanyId != 0)
            {
                UpdateCompany(Debitor.Company);
                if (Debitor.DebitorId != 0)
                {
                    UpdateDebitor();
                }
                else
                {
                    CreateDebitor();
                }
            }
            else
            {
                CreateDebitorWithCompany();
            }

            RefreshData();
        }

        private void CreateDebitor()
        {
            using (DataLayer db = new DataLayer())
            {
                var debitorNumber = db.CostAccounts.GetNextDebitorNumber();
                Debitor.CostAccount.AccountNumber = debitorNumber;
                Debitor.CostAccount.RefCostAccountCategoryId = db.CostAccountCategories.GetDebitorId();
                Debitor.CostAccount.Description = Debitor.Company.Name;
                Debitor.CostAccount.IsVisible = true;
                var costAccountId = db.CostAccounts.Insert(Debitor.CostAccount);
                var debitor = new Debitor() { RefCompanyId = SelectedCompany.CompanyId, RefCostAccountId = costAccountId };
                db.Debitors.Insert(debitor);
            }

            var notificationService = this.GetRequiredService<INotificationService>();
            var notification = notificationService.CreatePredefinedNotification("Neuer Debitor", $"Der Debitor {Debitor.Company.Name} wurde erfolgreich angelegt.", string.Empty);
            notification.ShowAsync();
        }

        private void UpdateDebitor()
        {
            using (DataLayer db = new DataLayer())
            {
                db.Debitors.Update(Debitor);
            }
        }

        private void CreateDebitorWithCompany()
        {
            using (DataLayer db = new DataLayer())
            {
                var companyId = db.Companies.Insert(Debitor.Company);
                var debitorNumber = db.CostAccounts.GetNextDebitorNumber();
                Debitor.CostAccount.AccountNumber = debitorNumber;
                Debitor.CostAccount.RefCostAccountCategoryId = db.CostAccountCategories.GetDebitorId();
                Debitor.CostAccount.Description = Debitor.Company.Name;
                Debitor.CostAccount.IsVisible = true;
                var costAccountId = db.CostAccounts.Insert(Debitor.CostAccount);
                var creditor = new Creditor() { RefCompanyId = companyId, RefCostAccountId = costAccountId };
                db.Creditors.Insert(creditor);
            }

            var notificationService = this.GetRequiredService<INotificationService>();
            var notification = notificationService.CreatePredefinedNotification("Neuer Debitor", $"Der Debitor {Debitor.Company.Name} wurde erfolgreich angelegt.", string.Empty);
            notification.ShowAsync();
        }

        private void UpdateCompany(Company company)
        {
            using (DataLayer db = new DataLayer())
            {
                db.Companies.Update(company);
            }
            var notificationService = this.GetRequiredService<INotificationService>();
            var notification = notificationService.CreatePredefinedNotification("Änderungen", $"Die Änderungen am Debitor {company.Name} wurde erfolgreich gespeichert.", string.Empty);
            notification.ShowAsync();
        }

        private void DeleteCreditor()
        {
            using (DataLayer db = new DataLayer())
            {
                db.Creditors.Delete(Creditor.CreditorId);
                db.Companies.Delete(Creditor.Company.CompanyId);
                db.CostAccounts.Delete(Creditor.CostAccount.CostAccountId);
            }
            RefreshData();
        }

        private void DeleteDebitor()
        {
            using (DataLayer db = new DataLayer())
            {
                db.Debitors.Delete(Debitor.DebitorId);
                db.Companies.Delete(Debitor.Company.CompanyId);
                db.CostAccounts.Delete(Debitor.CostAccount.CostAccountId);
            }
            RefreshData();
        }

        private void InitializeButtonCommands()
        {
            SaveCreditorCommand = new DelegateCommand(() =>
            {
                SaveCreditor();
            });
            DeleteCreditorCommand = new DelegateCommand(() =>
            {
                DeleteCreditor();
            });
            SaveDebitorCommand = new DelegateCommand(() =>
            {
                SaveDebitor();
            });
            DeleteDebitorCommand = new DelegateCommand(() =>
            {
                DeleteDebitor();
            });
            NewCreditorCommand = new DelegateCommand(() =>
            {
                Creditor = new Creditor();
            });
            NewDebitorCommand = new DelegateCommand(() =>
            {
                Debitor = new Debitor();
            });
            OpenCompanyWindowCommand = new DelegateCommand(() =>
            {
                Messenger.Default.Send(new OpenCompanyWindowMessage());
            });
        }

        private void CreditorCompany_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ValidateCreditor();
        }

        private void DebitorCompany_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ValidateDebitor();
        }

        private void UseExistingCompany()
        {
            if (SelectedTab == 0)
            {
                Creditor.Company.PropertyChanged -= CreditorCompany_PropertyChanged;
                Creditor.Company = SelectedCompany;
                if (Creditor.Company.IsNull())
                {
                    Creditor.Company = new Company();
                }
                Creditor.Company.PropertyChanged += CreditorCompany_PropertyChanged;
                ValidateCreditor();
            }
            else if (SelectedTab == 1)
            {
                Debitor.Company.PropertyChanged -= DebitorCompany_PropertyChanged;
                Debitor.Company = SelectedCompany;
                if (Debitor.Company.IsNull())
                {
                    Debitor.Company = new Company();
                }
                Debitor.Company.PropertyChanged += DebitorCompany_PropertyChanged;
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

        private void ValidateSaveCreditor()
        {
            if (Creditor.IsNull())
            {
                Creditor = new Creditor();
            }

            if (!string.IsNullOrEmpty(Creditor.Company.Name) && !string.IsNullOrEmpty(Creditor.Company.Street) && Creditor.Company.Postcode != 0 && !string.IsNullOrEmpty(Creditor.Company.City))
            {
                SaveCreditorButtonEnabled = true;
                return;
            }

            SaveCreditorButtonEnabled = false;
        }

        private void ValidateDeleteCreditor()
        {
            if (Creditor.IsNull())
            {
                Creditor = new Creditor();
            }

            if (Creditor.Company.CompanyId != 0)
            {
                DeleteCreditorButtonEnabled = true;
                return;
            }

            DeleteCreditorButtonEnabled = false;
        }

        private void ValidateSaveDebitor()
        {
            if (Debitor.IsNull())
            {
                Debitor = new Debitor();
            }

            if (!string.IsNullOrEmpty(Debitor.Company.Name) && !string.IsNullOrEmpty(Debitor.Company.Street) && Debitor.Company.Postcode != 0 && !string.IsNullOrEmpty(Debitor.Company.City))
            {
                SaveDebitorButtonEnabled = true;
                return;
            }

            SaveDebitorButtonEnabled = false;
        }

        private void ValidateDeleteDebitor()
        {
            if (Debitor.IsNull())
            {
                Debitor = new Debitor();
            }

            if (Debitor.Company.CompanyId != 0)
            {
                DeleteDebitorButtonEnabled = true;
                return;
            }

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
        public DelegateCommand OpenCompanyWindowCommand { get; set; }

        public SvenTechCollection<TaxType> TaxTypes { get; set; }
        public SvenTechCollection<Company> Companies { get; set; } = new SvenTechCollection<Company>();
        public SvenTechCollection<Creditor> Creditors { get; set; } = new SvenTechCollection<Creditor>();
        public SvenTechCollection<Debitor> Debitors { get; set; } = new SvenTechCollection<Debitor>();

        public int SelectedTab { get; set; } = 0;
        public Creditor Creditor
        {
            get { return _Creditor; }
            set { _Creditor = value; ValidateCreditor(); }
        }
        public Debitor Debitor
        {
            get { return _Debitor; }
            set { _Debitor = value; ValidateDebitor(); }
        }
        public Company SelectedCompany
        {
            get { return _SelectedCompany; }
            set { _SelectedCompany = value; UseExistingCompany(); }
        }


        #region Validation Properties
        public bool SaveCreditorButtonEnabled
        {
            get { return _SaveCreditorButtonEnabled; }
            set { _SaveCreditorButtonEnabled = value; RaisePropertyChanged(); }
        }
        public bool SaveDebitorButtonEnabled
        {
            get { return _SaveDebitorButtonEnabled; }
            set { _SaveDebitorButtonEnabled = value; RaisePropertyChanged(); }
        }
        public bool DeleteCreditorButtonEnabled
        {
            get { return _DeleteCreditorButtonEnabled; }
            set { _DeleteCreditorButtonEnabled = value; RaisePropertyChanged(); }
        }
        public bool DeleteDebitorButtonEnabled
        {
            get { return _DeleteDebitorButtonEnabled; }
            set { _DeleteDebitorButtonEnabled = value; RaisePropertyChanged(); }
        }
        #endregion Validation Properties

        #endregion Properties

    }
}
