using System;
using System.Windows;
using DevExpress.Mvvm;

using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.ProjectManagement;
using Utilities;
using WebApiWrapper.ProjectManagement;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class HealthInsuranceViewModel : ViewModelBase
    {
        public HealthInsuranceViewModel()
        {
            if (IsInDesignMode) return;

            HealthInsuranceList = LoadAllHealthInsurances();
            NewHealthInsuranceCommand = new DelegateCommand(NewHealthInsurance);
            SaveHealthInsuranceCommand = new DelegateCommand(SaveUser, () => Validation());
            DeleteHealthInsuranceCommand =
                new DelegateCommand(DeleteHealthInsurance, () => SelectedHealthInsurance != null);
        }

        public SvenTechCollection<HealthInsurance> HealthInsuranceList { get; set; } =
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
            return HealthInsurances.GetAll().ToSvenTechCollection();
        }

        private void NewHealthInsurance()
        {
            SelectedHealthInsurance = new HealthInsurance();
            HealthInsuranceList.Add(SelectedHealthInsurance);
        }

        private void DeleteHealthInsurance()
        {
            if (SelectedHealthInsurance == null) return;

            if (SelectedHealthInsurance.HealthInsuranceId == 0)
            {
                HealthInsuranceList.Remove(SelectedHealthInsurance);
                SelectedHealthInsurance = null;
                return;
            }

            HealthInsurances.Delete(SelectedHealthInsurance.HealthInsuranceId);
            HealthInsuranceList.Remove(SelectedHealthInsurance);
            SelectedHealthInsurance = null;
        }

        private void SaveUser()
        {
            if (SelectedHealthInsurance.HealthInsuranceId != 0)
                HealthInsurances.Update(SelectedHealthInsurance);
            else
                SelectedHealthInsurance.HealthInsuranceId =
                    HealthInsurances.Insert(SelectedHealthInsurance);
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