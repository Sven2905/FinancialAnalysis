using System.ComponentModel;
using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.CompanyManagement;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class CompanyViewModel : ViewModelBase
    {
        private Company _SelectedCompany = new Company();

        public CompanyViewModel()
        {
            InitializeButtonCommands();
            RefreshData();
        }

        private IDocumentManagerService SingleObjectDocumentManagerService =>
            GetService<IDocumentManagerService>("SignleObjectDocumentManagerService");

        public DelegateCommand NewCompanyCommand { get; set; }
        public DelegateCommand SaveCompanyCommand { get; set; }
        public DelegateCommand DeleteCompanyCommand { get; set; }

        public Company SelectedCompany
        {
            get => _SelectedCompany;
            set
            {
                _SelectedCompany = value;
                UseExistingCompany();
            }
        }

        public SvenTechCollection<Company> Companies { get; set; }
        public bool SaveCompanyButtonEnabled { get; set; }
        public bool DeleteCompanyButtonEnabled { get; set; }

        private void RefreshData()
        {
            using (var db = new DataLayer())
            {
                Companies = db.Companies.GetAll().ToSvenTechCollection();
            }
        }

        private void InitializeButtonCommands()
        {
            NewCompanyCommand = new DelegateCommand(() =>
            {
                SelectedCompany = new Company();
                DeleteCompanyButtonEnabled = false;
            });
            SaveCompanyCommand = new DelegateCommand(() =>
            {
                SaveCompany();
                RefreshData();
            });
            DeleteCompanyCommand = new DelegateCommand(() =>
            {
                DeleteCompany();
                RefreshData();
                DeleteCompanyButtonEnabled = false;
            });
        }

        private void SaveCompany()
        {
            using (var db = new DataLayer())
            {
                db.Companies.UpdateOrInsert(SelectedCompany);
                var notificationService = this.GetRequiredService<INotificationService>();
                INotification notification;
                if (SelectedCompany.CompanyId == 0)
                    notification = notificationService.CreatePredefinedNotification("Neue Firma",
                        $"Die Firma {SelectedCompany.Name} wurde erfolgreich angelegt.", string.Empty);
                else
                    notification = notificationService.CreatePredefinedNotification("Firma geändert",
                        $"Die Änderungen an der Firma {SelectedCompany.Name} wurden erfolgreich durchgeführt.",
                        string.Empty);
                notification.ShowAsync();
            }
        }

        private void DeleteCompany()
        {
            if (DeleteCompanyButtonEnabled)
            {
                try
                {
                    using (var db = new DataLayer())
                    {
                        db.Companies.Delete(SelectedCompany.CompanyId);
                    }
                }
                catch (System.Exception ex)
                {
                    Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
                }
            }
                
        }

        private void UseExistingCompany()
        {
            if (SelectedCompany.IsNull()) SelectedCompany = new Company();
            SelectedCompany.PropertyChanged += SelectedCompany_PropertyChanged;
            ValidateCompany();
            ValidateDeleteButton();
        }

        private void ValidateCompany()
        {
            if (!string.IsNullOrEmpty(SelectedCompany.Name) && !string.IsNullOrEmpty(SelectedCompany.Street) &&
                SelectedCompany.Postcode != 0 && !string.IsNullOrEmpty(SelectedCompany.City))
            {
                SaveCompanyButtonEnabled = true;
                return;
            }

            SaveCompanyButtonEnabled = false;
        }

        private void ValidateDeleteButton()
        {
            if (!SelectedCompany.IsNull() && SelectedCompany.CompanyId != 0)
                using (var db = new DataLayer())
                {
                    DeleteCompanyButtonEnabled = !db.Companies.IsCompanyInUse(SelectedCompany.CompanyId);
                }
        }

        private void SelectedCompany_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            ValidateCompany();
        }
    }
}