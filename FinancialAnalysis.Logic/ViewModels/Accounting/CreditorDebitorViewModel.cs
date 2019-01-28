using System.ComponentModel;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
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
            Creditor.Client.PropertyChanged += CreditorClient_PropertyChanged;
            Debitor.Client.PropertyChanged += DebitorClient_PropertyChanged;

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

        private void SaveCreditor()
        {
            if (Creditor.Client.ClientId != 0)
            {
                UpdateClient(Creditor.Client);
                if (Creditor.CreditorId != 0)
                    UpdateCreditor();
                else
                    CreateCreditor();
            }
            else
            {
                CreateCreditorWithClient();
            }

            RefreshData();
        }

        private void CreateCreditor()
        {
            try
            {
                var creditorNumber = DataLayer.Instance.CostAccounts.GetNextCreditorNumber();
                Creditor.CostAccount.AccountNumber = creditorNumber;
                Creditor.CostAccount.RefCostAccountCategoryId = DataLayer.Instance.CostAccountCategories.GetCreditorId();
                Creditor.CostAccount.Description = Creditor.Client.Name;
                Creditor.CostAccount.IsVisible = true;
                var costAccountId = DataLayer.Instance.CostAccounts.Insert(Creditor.CostAccount);
                var creditor = new Creditor
                { RefClientId = SelectedClient.ClientId, RefCostAccountId = costAccountId };
                DataLayer.Instance.Creditors.Insert(creditor);
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }

            var notificationService = this.GetRequiredService<INotificationService>();
            var notification = notificationService.CreatePredefinedNotification("Neuer Kreditor",
                $"Der Kreditor {Creditor.Client.Name} wurde erfolgreich angelegt.", string.Empty);
            notification.ShowAsync();
        }

        private void UpdateCreditor()
        {
            try
            {
                DataLayer.Instance.Creditors.Update(Creditor);
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
                Creditors = DataLayer.Instance.Creditors.GetAll().ToSvenTechCollection();
                Debitors = DataLayer.Instance.Debitors.GetAll().ToSvenTechCollection();
                TaxTypes = DataLayer.Instance.TaxTypes.GetAll().ToSvenTechCollection();
                Companies = DataLayer.Instance.Companies.GetAll().ToSvenTechCollection();
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void CreateCreditorWithClient()
        {
            try
            {
                var ClientId = DataLayer.Instance.Companies.Insert(Creditor.Client);
                var creditorNumber = DataLayer.Instance.CostAccounts.GetNextCreditorNumber();
                Creditor.CostAccount.AccountNumber = creditorNumber;
                Creditor.CostAccount.RefCostAccountCategoryId = DataLayer.Instance.CostAccountCategories.GetCreditorId();
                Creditor.CostAccount.Description = Creditor.Client.Name;
                Creditor.CostAccount.IsVisible = true;
                var costAccountId = DataLayer.Instance.CostAccounts.Insert(Creditor.CostAccount);
                var creditor = new Creditor { RefClientId = ClientId, RefCostAccountId = costAccountId };
                DataLayer.Instance.Creditors.Insert(creditor);
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }

            var notificationService = this.GetRequiredService<INotificationService>();
            var notification = notificationService.CreatePredefinedNotification("Neuer Kreditor",
                $"Der Kreditor {Creditor.Client.Name} wurde erfolgreich angelegt.", string.Empty);
            notification.ShowAsync();
        }

        private void SaveDebitor()
        {
            if (Debitor.Client.ClientId != 0)
            {
                UpdateClient(Debitor.Client);
                if (Debitor.DebitorId != 0)
                    UpdateDebitor();
                else
                    CreateDebitor();
            }
            else
            {
                CreateDebitorWithClient();
            }

            RefreshData();
        }

        private void CreateDebitor()
        {
            try
            {
                var debitorNumber = DataLayer.Instance.CostAccounts.GetNextDebitorNumber();
                Debitor.CostAccount.AccountNumber = debitorNumber;
                Debitor.CostAccount.RefCostAccountCategoryId = DataLayer.Instance.CostAccountCategories.GetDebitorId();
                Debitor.CostAccount.Description = Debitor.Client.Name;
                Debitor.CostAccount.IsVisible = true;
                var costAccountId = DataLayer.Instance.CostAccounts.Insert(Debitor.CostAccount);
                var debitor = new Debitor { RefClientId = SelectedClient.ClientId, RefCostAccountId = costAccountId };
                DataLayer.Instance.Debitors.Insert(debitor);
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }

            var notificationService = this.GetRequiredService<INotificationService>();
            var notification = notificationService.CreatePredefinedNotification("Neuer Debitor",
                $"Der Debitor {Debitor.Client.Name} wurde erfolgreich angelegt.", string.Empty);
            notification.ShowAsync();
        }

        private void UpdateDebitor()
        {
            try
            {
                DataLayer.Instance.Debitors.Update(Debitor);
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void CreateDebitorWithClient()
        {
            try
            {
                var ClientId = DataLayer.Instance.Companies.Insert(Debitor.Client);
                var debitorNumber = DataLayer.Instance.CostAccounts.GetNextDebitorNumber();
                Debitor.CostAccount.AccountNumber = debitorNumber;
                Debitor.CostAccount.RefCostAccountCategoryId = DataLayer.Instance.CostAccountCategories.GetDebitorId();
                Debitor.CostAccount.Description = Debitor.Client.Name;
                Debitor.CostAccount.IsVisible = true;
                var costAccountId = DataLayer.Instance.CostAccounts.Insert(Debitor.CostAccount);
                var creditor = new Creditor { RefClientId = ClientId, RefCostAccountId = costAccountId };
                DataLayer.Instance.Creditors.Insert(creditor);
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }

            var notificationService = this.GetRequiredService<INotificationService>();
            var notification = notificationService.CreatePredefinedNotification("Neuer Debitor",
                $"Der Debitor {Debitor.Client.Name} wurde erfolgreich angelegt.", string.Empty);
            notification.ShowAsync();
        }

        private void UpdateClient(Client Client)
        {
            try
            {
                DataLayer.Instance.Companies.Update(Client);
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }

            var notificationService = this.GetRequiredService<INotificationService>();
            var notification = notificationService.CreatePredefinedNotification("Änderungen",
                $"Die Änderungen am Debitor {Client.Name} wurde erfolgreich gespeichert.", string.Empty);
            notification.ShowAsync();
        }

        private void DeleteCreditor()
        {
            try
            {
                DataLayer.Instance.Creditors.Delete(Creditor.CreditorId);
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
                DataLayer.Instance.Debitors.Delete(Debitor.DebitorId);
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
            OpenClientWindowCommand = new DelegateCommand(() =>
            {
                Messenger.Default.Send(new OpenClientWindowMessage());
            });
        }

        private void CreditorClient_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ValidateCreditor();
        }

        private void DebitorClient_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ValidateDebitor();
        }

        private void UseExistingClient()
        {
            if (SelectedTab == 0)
            {
                Creditor.Client.PropertyChanged -= CreditorClient_PropertyChanged;
                Creditor.Client = SelectedClient;
                if (Creditor.Client.IsNull()) Creditor.Client = new Client();
                Creditor.Client.PropertyChanged += CreditorClient_PropertyChanged;

                ValidateCreditor();
            }
            else if (SelectedTab == 1)
            {
                Debitor.Client.PropertyChanged -= DebitorClient_PropertyChanged;
                Debitor.Client = SelectedClient;
                if (Debitor.Client.IsNull()) Debitor.Client = new Client();
                Debitor.Client.PropertyChanged += DebitorClient_PropertyChanged;

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

            if (!string.IsNullOrEmpty(Creditor.Client.Name) && !string.IsNullOrEmpty(Creditor.Client.Street) &&
                Creditor.Client.Postcode != 0 && !string.IsNullOrEmpty(Creditor.Client.City))
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
            if (Debitor.IsNull()) Debitor = new Debitor();

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

        public SvenTechCollection<TaxType> TaxTypes { get; set; }
        public SvenTechCollection<Client> Companies { get; set; } = new SvenTechCollection<Client>();
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

        public Client SelectedClient
        {
            get => _selectedClient;
            set
            {
                _selectedClient = value;
                UseExistingClient();
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