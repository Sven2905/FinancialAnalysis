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
            try
            {
                allHealthInsurances = DataContext.Instance.HealthInsurances.GetAll().ToSvenTechCollection();
            }
            catch (Exception ex)
            {
                // TODO Exception
            }

            return allHealthInsurances;
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

            try
            {
                DataContext.Instance.HealthInsurances.Delete(SelectedHealthInsurance.HealthInsuranceId);
                HealthInsurances.Remove(SelectedHealthInsurance);
                SelectedHealthInsurance = null;
            }
            catch (Exception ex)
            {
                // TODO Exception
            }
        }

        private void SaveUser()
        {
            try
            {
                if (SelectedHealthInsurance.HealthInsuranceId != 0)
                    DataContext.Instance.HealthInsurances.Update(SelectedHealthInsurance);
                else
                    SelectedHealthInsurance.HealthInsuranceId =
                        DataContext.Instance.HealthInsurances.Insert(SelectedHealthInsurance);
            }
            catch (Exception ex)
            {
                // TODO Exception
            }
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