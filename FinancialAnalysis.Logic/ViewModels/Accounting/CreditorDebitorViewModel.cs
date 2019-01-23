using System.ComponentModel;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.CompanyManagement;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class CreditorDebitorViewModel : ViewModelBase
    {
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

        #region Fields

        private Creditor _creditor = new Creditor();
        private Debitor _debitor = new Debitor();
        private Company _selectedCompany = new Company();
        private bool _saveCreditorButtonEnabled;
        private bool _saveDebitorButtonEnabled;
        private bool _deleteCreditorButtonEnabled;
        private bool _deleteDebitorButtonEnabled;

        private IDocumentManagerService SingleObjectDocumentManagerService =>
            GetService<IDocumentManagerService>("SingleObjectDocumentManagerService");

        #endregion Fields

        #region Methods

        private void SaveCreditor()
        {
            if (Creditor.Company.CompanyId != 0)
            {
                UpdateCompany(Creditor.Company);
                if (Creditor.CreditorId != 0)
                    UpdateCreditor();
                else
                    CreateCreditor();
            }
            else
            {
                CreateCreditorWithCompany();
            }

            RefreshData();
        }

        private void CreateCreditor()
        {
            try
            {
                using (var db = new DataLayer())
                {
                    var creditorNumber = db.CostAccounts.GetNextCreditorNumber();
                    Creditor.CostAccount.AccountNumber = creditorNumber;
                    Creditor.CostAccount.RefCostAccountCategoryId = db.CostAccountCategories.GetCreditorId();
                    Creditor.CostAccount.Description = Creditor.Company.Name;
                    Creditor.CostAccount.IsVisible = true;
                    var costAccountId = db.CostAccounts.Insert(Creditor.CostAccount);
                    var creditor = new Creditor
                    { RefCompanyId = SelectedCompany.CompanyId, RefCostAccountId = costAccountId };
                    db.Creditors.Insert(creditor);
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }

            var notificationService = this.GetRequiredService<INotificationService>();
            var notification = notificationService.CreatePredefinedNotification("Neuer Kreditor",
                $"Der Kreditor {Creditor.Company.Name} wurde erfolgreich angelegt.", string.Empty);
            notification.ShowAsync();
        }

        private void UpdateCreditor()
        {
            try
            {
                using (var db = new DataLayer())
                {
                    db.Creditors.Update(Creditor);
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void RefreshData()
        {
            try
            {
                using (var db = new DataLayer())
                {
                    Creditors = db.Creditors.GetAll().ToSvenTechCollection();
                    Debitors = db.Debitors.GetAll().ToSvenTechCollection();
                    TaxTypes = db.TaxTypes.GetAll().ToSvenTechCollection();
                    Companies = db.Companies.GetAll().ToSvenTechCollection();
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void CreateCreditorWithCompany()
        {
            try
            {
                using (var db = new DataLayer())
                {
                    var companyId = db.Companies.Insert(Creditor.Company);
                    var creditorNumber = db.CostAccounts.GetNextCreditorNumber();
                    Creditor.CostAccount.AccountNumber = creditorNumber;
                    Creditor.CostAccount.RefCostAccountCategoryId = db.CostAccountCategories.GetCreditorId();
                    Creditor.CostAccount.Description = Creditor.Company.Name;
                    Creditor.CostAccount.IsVisible = true;
                    var costAccountId = db.CostAccounts.Insert(Creditor.CostAccount);
                    var creditor = new Creditor { RefCompanyId = companyId, RefCostAccountId = costAccountId };
                    db.Creditors.Insert(creditor);
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }

            var notificationService = this.GetRequiredService<INotificationService>();
            var notification = notificationService.CreatePredefinedNotification("Neuer Kreditor",
                $"Der Kreditor {Creditor.Company.Name} wurde erfolgreich angelegt.", string.Empty);
            notification.ShowAsync();
        }

        private void SaveDebitor()
        {
            if (Debitor.Company.CompanyId != 0)
            {
                UpdateCompany(Debitor.Company);
                if (Debitor.DebitorId != 0)
                    UpdateDebitor();
                else
                    CreateDebitor();
            }
            else
            {
                CreateDebitorWithCompany();
            }

            RefreshData();
        }

        private void CreateDebitor()
        {
            try
            {
                using (var db = new DataLayer())
                {
                    var debitorNumber = db.CostAccounts.GetNextDebitorNumber();
                    Debitor.CostAccount.AccountNumber = debitorNumber;
                    Debitor.CostAccount.RefCostAccountCategoryId = db.CostAccountCategories.GetDebitorId();
                    Debitor.CostAccount.Description = Debitor.Company.Name;
                    Debitor.CostAccount.IsVisible = true;
                    var costAccountId = db.CostAccounts.Insert(Debitor.CostAccount);
                    var debitor = new Debitor { RefCompanyId = SelectedCompany.CompanyId, RefCostAccountId = costAccountId };
                    db.Debitors.Insert(debitor);
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }

            var notificationService = this.GetRequiredService<INotificationService>();
            var notification = notificationService.CreatePredefinedNotification("Neuer Debitor",
                $"Der Debitor {Debitor.Company.Name} wurde erfolgreich angelegt.", string.Empty);
            notification.ShowAsync();
        }

        private void UpdateDebitor()
        {
            try
            {
                using (var db = new DataLayer())
                {
                    db.Debitors.Update(Debitor);
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void CreateDebitorWithCompany()
        {
            try
            {
                using (var db = new DataLayer())
                {
                    var companyId = db.Companies.Insert(Debitor.Company);
                    var debitorNumber = db.CostAccounts.GetNextDebitorNumber();
                    Debitor.CostAccount.AccountNumber = debitorNumber;
                    Debitor.CostAccount.RefCostAccountCategoryId = db.CostAccountCategories.GetDebitorId();
                    Debitor.CostAccount.Description = Debitor.Company.Name;
                    Debitor.CostAccount.IsVisible = true;
                    var costAccountId = db.CostAccounts.Insert(Debitor.CostAccount);
                    var creditor = new Creditor { RefCompanyId = companyId, RefCostAccountId = costAccountId };
                    db.Creditors.Insert(creditor);
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }

            var notificationService = this.GetRequiredService<INotificationService>();
            var notification = notificationService.CreatePredefinedNotification("Neuer Debitor",
                $"Der Debitor {Debitor.Company.Name} wurde erfolgreich angelegt.", string.Empty);
            notification.ShowAsync();
        }

        private void UpdateCompany(Company company)
        {
            try
            {
                using (var db = new DataLayer())
                {
                    db.Companies.Update(company);
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }

            var notificationService = this.GetRequiredService<INotificationService>();
            var notification = notificationService.CreatePredefinedNotification("Änderungen",
                $"Die Änderungen am Debitor {company.Name} wurde erfolgreich gespeichert.", string.Empty);
            notification.ShowAsync();
        }

        private void DeleteCreditor()
        {
            try
            {
                using (var db = new DataLayer())
                {
                    db.Creditors.Delete(Creditor.CreditorId);
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }

            RefreshData();
        }

        private void DeleteDebitor()
        {
            try
            {
                using (var db = new DataLayer())
                {
                    db.Debitors.Delete(Debitor.DebitorId);
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }

            RefreshData();
        }

        private void InitializeButtonCommands()
        {
            SaveCreditorCommand = new DelegateCommand(SaveCreditor);
            DeleteCreditorCommand = new DelegateCommand(DeleteCreditor);
            SaveDebitorCommand = new DelegateCommand(SaveDebitor);
            DeleteDebitorCommand = new DelegateCommand(DeleteDebitor);

            NewCreditorCommand = new DelegateCommand(() => { Creditor = new Creditor(); });
            NewDebitorCommand = new DelegateCommand(() => { Debitor = new Debitor(); });
            OpenCompanyWindowCommand = new DelegateCommand(() =>
            {
                Messenger.Default.Send(new OpenCompanyWindowMessage());
            });
        }

        private void CreditorCompany_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ValidateCreditor();
        }

        private void DebitorCompany_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ValidateDebitor();
        }

        private void UseExistingCompany()
        {
            if (SelectedTab == 0)
            {
                Creditor.Company.PropertyChanged -= CreditorCompany_PropertyChanged;
                Creditor.Company = SelectedCompany;
                if (Creditor.Company.IsNull()) Creditor.Company = new Company();
                Creditor.Company.PropertyChanged += CreditorCompany_PropertyChanged;

                ValidateCreditor();
            }
            else if (SelectedTab == 1)
            {
                Debitor.Company.PropertyChanged -= DebitorCompany_PropertyChanged;
                Debitor.Company = SelectedCompany;
                if (Debitor.Company.IsNull()) Debitor.Company = new Company();
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
            if (Creditor.IsNull()) Creditor = new Creditor();

            if (!string.IsNullOrEmpty(Creditor.Company.Name) && !string.IsNullOrEmpty(Creditor.Company.Street) &&
                Creditor.Company.Postcode != 0 && !string.IsNullOrEmpty(Creditor.Company.City))
            {
                SaveCreditorButtonEnabled = true;
                return;
            }

            SaveCreditorButtonEnabled = false;
        }

        private void ValidateDeleteCreditor()
        {
            if (Creditor.IsNull()) Creditor = new Creditor();

            if (Creditor.CreditorId != 0)
                using (var db = new DataLayer())
                {
                    DeleteCreditorButtonEnabled = !db.Creditors.IsCreditorInUse(Creditor.CreditorId);
                }
            else
                DeleteCreditorButtonEnabled = false;
        }

        private void ValidateSaveDebitor()
        {
            if (Debitor.IsNull()) Debitor = new Debitor();

            if (!string.IsNullOrEmpty(Debitor.Company.Name) && !string.IsNullOrEmpty(Debitor.Company.Street) &&
                Debitor.Company.Postcode != 0 && !string.IsNullOrEmpty(Debitor.Company.City))
            {
                SaveDebitorButtonEnabled = true;
                return;
            }

            SaveDebitorButtonEnabled = false;
        }

        private void ValidateDeleteDebitor()
        {
            if (Debitor.IsNull()) Debitor = new Debitor();

            if (Debitor.DebitorId != 0)
                using (var db = new DataLayer())
                {
                    DeleteDebitorButtonEnabled = !db.Debitors.IsDebitorInUse(Debitor.DebitorId);
                }
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
        public DelegateCommand OpenCompanyWindowCommand { get; set; }

        public SvenTechCollection<TaxType> TaxTypes { get; set; }
        public SvenTechCollection<Company> Companies { get; set; } = new SvenTechCollection<Company>();
        public SvenTechCollection<Creditor> Creditors { get; set; } = new SvenTechCollection<Creditor>();
        public SvenTechCollection<Debitor> Debitors { get; set; } = new SvenTechCollection<Debitor>();

        public int SelectedTab { get; set; } = 0;

        public Creditor Creditor
        {
            get => _creditor;
            set
            {
                _creditor = value;
                ValidateCreditor();
            }
        }

        public Debitor Debitor
        {
            get => _debitor;
            set
            {
                _debitor = value;
                ValidateDebitor();
            }
        }

        public Company SelectedCompany
        {
            get => _selectedCompany;
            set
            {
                _selectedCompany = value;
                UseExistingCompany();
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