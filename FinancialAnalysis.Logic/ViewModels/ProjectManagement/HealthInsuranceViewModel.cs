using System;
using System.Windows;
using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.ProjectManagement;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class HealthInsuranceViewModel : ViewModelBase
    {
        public HealthInsuranceViewModel()
        {
            if (IsInDesignMode) return;

            HealthInsurances = LoadAllHealthInsurances();
            NewHealthInsuranceCommand = new DelegateCommand(NewHealthInsurance);
            SaveHealthInsuranceCommand = new DelegateCommand(SaveUser, () => Validation());
            DeleteHealthInsuranceCommand =
                new DelegateCommand(DeleteHealthInsurance, () => SelectedHealthInsurance != null);
        }

        public SvenTechCollection<HealthInsurance> HealthInsurances { get; set; } =
            new SvenTechCollection<HealthInsurance>();

        public DelegateCommand NewHealthInsuranceCommand { get; set; }
        public DelegateCommand SaveHealthInsuranceCommand { get; set; }
        public DelegateCommand DeleteHealthInsuranceCommand { get; set; }
        public string Password { get; set; } = string.Empty;
        public string PasswordRepeat { get; set; } = string.Empty;

        public HealthInsurance SelectedHealthInsurance { get; set; }

        private SvenTechCollection<HealthInsurance> LoadAllHealthInsurances()
        {
            var allHealthInsurances = new SvenTechCollection<HealthInsurance>();
            return DataContext.Instance.HealthInsurances.GetAll().ToSvenTechCollection();
        }

        private void NewHealthInsurance()
        {
            SelectedHealthInsurance = new HealthInsurance();
            HealthInsurances.Add(SelectedHealthInsurance);
        }

        private void DeleteHealthInsurance()
        {
            if (SelectedHealthInsurance == null) return;

            if (SelectedHealthInsurance.HealthInsuranceId == 0)
            {
                HealthInsurances.Remove(SelectedHealthInsurance);
                SelectedHealthInsurance = null;
                return;
            }

            DataContext.Instance.HealthInsurances.Delete(SelectedHealthInsurance.HealthInsuranceId);
            HealthInsurances.Remove(SelectedHealthInsurance);
            SelectedHealthInsurance = null;
        }

        private void SaveUser()
        {
            if (SelectedHealthInsurance.HealthInsuranceId != 0)
                DataContext.Instance.HealthInsurances.Update(SelectedHealthInsurance);
            else
                SelectedHealthInsurance.HealthInsuranceId =
                    DataContext.Instance.HealthInsurances.Insert(SelectedHealthInsurance);
        }

        private bool Validation()
        {
            if (SelectedHealthInsurance == null) return false;
            if (string.IsNullOrEmpty(SelectedHealthInsurance.Name)) return false;
            if (SelectedHealthInsurance.HealthInsuranceId == 0)
                if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(PasswordRepeat))
                    return false;
            return true;
        }
    }
}