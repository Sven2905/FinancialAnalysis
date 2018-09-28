using DevExpress.Mvvm;
using DevExpress.Mvvm.POCO;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models;
using System;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class CompanyViewModel : ViewModelBase
    {
        private Company _SelectedCompany = new Company();
        private IDocumentManagerService SingleObjectDocumentManagerService { get { return GetService<IDocumentManagerService>("SignleObjectDocumentManagerService"); } }

        public CompanyViewModel()
        {
            InitializeButtonCommands();
            RefreshData();
        }

        private void RefreshData()
        {
            using (DataLayer db = new DataLayer())
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
            using (DataLayer db = new DataLayer())
            {
                db.Companies.UpdateOrInsert(SelectedCompany);
                var notificationService = this.GetRequiredService<INotificationService>();
                INotification notification;
                if (SelectedCompany.CompanyId == 0)
                {
                    notification = notificationService.CreatePredefinedNotification("Neue Firma", $"Die Firma {SelectedCompany.Name} wurde erfolgreich angelegt.", string.Empty);
                }
                else
                {
                    notification = notificationService.CreatePredefinedNotification("Firma geändert", $"Die Änderungen an der Firma {SelectedCompany.Name} wurden erfolgreich durchgeführt.", string.Empty);
                }
                notification.ShowAsync();
            }
        }

        private void DeleteCompany()
        {
            if (DeleteCompanyButtonEnabled)
            {
                using (DataLayer db = new DataLayer())
                {
                    db.Companies.Delete(SelectedCompany.CompanyId);
                }
            }
        }

        private void UseExistingCompany()
        {
            if (SelectedCompany.IsNull())
            {
                SelectedCompany = new Company();
            }
            SelectedCompany.PropertyChanged += SelectedCompany_PropertyChanged;
            ValidateCompany();
            ValidateDeleteButton();
        }

        private void ValidateCompany()
        {
            if (!string.IsNullOrEmpty(SelectedCompany.Name) && !string.IsNullOrEmpty(SelectedCompany.Street) && SelectedCompany.Postcode != 0 && !string.IsNullOrEmpty(SelectedCompany.City))
            {
                SaveCompanyButtonEnabled = true;
                return;
            }

            SaveCompanyButtonEnabled = false;
        }

        private void ValidateDeleteButton()
        {
            if (!SelectedCompany.IsNull() && SelectedCompany.CompanyId != 0)
            {
                using (DataLayer db = new DataLayer())
                {
                    DeleteCompanyButtonEnabled = !db.Companies.IsCompanyInUse(SelectedCompany.CompanyId);
                }
            }
        }

        private void SelectedCompany_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            ValidateCompany();
        }

        public DelegateCommand NewCompanyCommand { get; set; }
        public DelegateCommand SaveCompanyCommand { get; set; }
        public DelegateCommand DeleteCompanyCommand { get; set; }

        public Company SelectedCompany
        {
            get { return _SelectedCompany; }
            set { _SelectedCompany = value; UseExistingCompany(); }
        }

        public SvenTechCollection<Company> Companies { get; set; }
        public bool SaveCompanyButtonEnabled { get; set; } = false;
        public bool DeleteCompanyButtonEnabled { get; set; } = false;
    }
}
