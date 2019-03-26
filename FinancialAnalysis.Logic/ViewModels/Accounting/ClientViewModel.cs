using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using FinancialAnalysis.Models.ClientManagement;
using System.ComponentModel;
using Utilities;
using WebApiWrapper.ClientManagement;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class ClientViewModel : ViewModelBase
    {
        private Client _SelectedClient = new Client();

        public ClientViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }

            InitializeButtonCommands();
            RefreshData();
        }

        private IDocumentManagerService SingleObjectDocumentManagerService =>
            GetService<IDocumentManagerService>("SignleObjectDocumentManagerService");

        public DelegateCommand NewClientCommand { get; set; }
        public DelegateCommand SaveClientCommand { get; set; }
        public DelegateCommand DeleteClientCommand { get; set; }

        public Client SelectedClient
        {
            get => _SelectedClient;
            set
            {
                _SelectedClient = value;
                UseExistingClient();
            }
        }

        public SvenTechCollection<Client> ClientList { get; set; }
        public bool SaveClientButtonEnabled { get; set; }
        public bool DeleteClientButtonEnabled { get; set; }

        private void RefreshData()
        {
            ClientList = Clients.GetAll().ToSvenTechCollection();
        }

        private void InitializeButtonCommands()
        {
            NewClientCommand = new DelegateCommand(() =>
            {
                SelectedClient = new Client();
                DeleteClientButtonEnabled = false;
            });
            SaveClientCommand = new DelegateCommand(() =>
            {
                SaveClient();
                RefreshData();
            });
            DeleteClientCommand = new DelegateCommand(() =>
            {
                DeleteClient();
                RefreshData();
                DeleteClientButtonEnabled = false;
            });
        }

        private void SaveClient()
        {
            if (SelectedClient.ClientId != 0)
            {
                Clients.Update(SelectedClient);
            }
            else
            {
                Clients.Insert(SelectedClient);
            }
            var notificationService = this.GetRequiredService<INotificationService>();
            INotification notification;
            if (SelectedClient.ClientId == 0)
            {
                notification = notificationService.CreatePredefinedNotification("Neue Firma",
                    $"Die Firma {SelectedClient.Name} wurde erfolgreich angelegt.", string.Empty);
            }
            else
            {
                notification = notificationService.CreatePredefinedNotification("Firma geändert",
                    $"Die Änderungen an der Firma {SelectedClient.Name} wurden erfolgreich durchgeführt.",
                    string.Empty);
            }

            notification.ShowAsync();
        }

        private void DeleteClient()
        {
            if (DeleteClientButtonEnabled)
            {
                Clients.Delete(SelectedClient.ClientId);
            }
        }

        private void UseExistingClient()
        {
            if (SelectedClient.IsNull())
            {
                SelectedClient = new Client();
            }

            SelectedClient.PropertyChanged += SelectedClient_PropertyChanged;
            ValidateClient();
            ValidateDeleteButton();
        }

        private void ValidateClient()
        {
            if (!string.IsNullOrEmpty(SelectedClient.Name) && !string.IsNullOrEmpty(SelectedClient.Street) &&
                SelectedClient.Postcode != 0 && !string.IsNullOrEmpty(SelectedClient.City))
            {
                SaveClientButtonEnabled = true;
                return;
            }

            SaveClientButtonEnabled = false;
        }

        private void ValidateDeleteButton()
        {
            if (!SelectedClient.IsNull() && SelectedClient.ClientId != 0)
            {
                DeleteClientButtonEnabled = !Clients.IsClientInUse(SelectedClient.ClientId);
            }
        }

        private void SelectedClient_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ValidateClient();
        }
    }
}