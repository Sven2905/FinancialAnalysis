using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.ProjectManagement;
using System.IO;
using System.Windows.Media.Imaging;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class HealthInsuranceViewModel : ViewModelBase
    {
        public SvenTechCollection<HealthInsurance> HealthInsurances { get; set; } = new SvenTechCollection<HealthInsurance>();
        public DelegateCommand NewHealthInsuranceCommand { get; set; }
        public DelegateCommand SaveHealthInsuranceCommand { get; set; }
        public DelegateCommand DeleteHealthInsuranceCommand { get; set; }
        public string Password { get; set; } = string.Empty;
        public string PasswordRepeat { get; set; } = string.Empty;

        public HealthInsurance SelectedHealthInsurance { get; set; }

        public HealthInsuranceViewModel()
        {
            if (IsInDesignMode)
                return;

            HealthInsurances = LoadAllHealthInsurances();
            NewHealthInsuranceCommand = new DelegateCommand(NewHealthInsurance);
            SaveHealthInsuranceCommand = new DelegateCommand(SaveUser, () => Validation());
            DeleteHealthInsuranceCommand = new DelegateCommand(DeleteHealthInsurance, () => (SelectedHealthInsurance != null));
        }

        private SvenTechCollection<HealthInsurance> LoadAllHealthInsurances()
        {
            SvenTechCollection<HealthInsurance> allHealthInsurances = new SvenTechCollection<HealthInsurance>();
            try
            {
                    allHealthInsurances = DataContext.Instance.HealthInsurances.GetAll().ToSvenTechCollection();
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
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
            if (SelectedHealthInsurance == null)
            {
                return;
            }

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
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void SaveUser()
        {
            try
            {
                if (SelectedHealthInsurance.HealthInsuranceId != 0)
                        DataContext.Instance.HealthInsurances.Update(SelectedHealthInsurance);
                else
                        SelectedHealthInsurance.HealthInsuranceId = DataContext.Instance.HealthInsurances.Insert(SelectedHealthInsurance);
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private bool Validation()
        {
            if (SelectedHealthInsurance == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(SelectedHealthInsurance.Name))
            {
                return false;
            }
            if (SelectedHealthInsurance.HealthInsuranceId == 0)
            {
                if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(PasswordRepeat))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
