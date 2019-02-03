using System;
using System.Threading.Tasks;
using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _currentTime;

        /// <summary>
        ///     Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode) return;

            UpdateTime();
        }

        public string CurrentTime
        {
            get => _currentTime;
            set
            {
                _currentTime = value;
                RaisePropertiesChanged();
            }
        }

        public User ActualUser => Globals.ActualUser;

        private void UpdateTime()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    CurrentTime = DateTime.Now.ToString("G");
                    Task.Delay(1000);
                }
            });
        }

        #region UserRights

        public bool ShowAccounting => Globals.ActualUser.IsAdministrator ||
                                      UserManager.Instance.IsUserRightGranted(Globals.ActualUser,
                                          Permission.AccessAccounting);

        public bool ShowProjectManagement => Globals.ActualUser.IsAdministrator ||
                                             UserManager.Instance.IsUserRightGranted(Globals.ActualUser,
                                                 Permission.AccessProjectManagement);

        public bool ShowProducts => Globals.ActualUser.IsAdministrator ||
                                    UserManager.Instance.IsUserRightGranted(Globals.ActualUser,
                                        Permission.AccessProducts);

        public bool ShowWarehouseManagement => Globals.ActualUser.IsAdministrator ||
                                               UserManager.Instance.IsUserRightGranted(Globals.ActualUser,
                                                   Permission.AccessWarehouseManagement);

        public bool ShowConfiguration => Globals.ActualUser.IsAdministrator ||
                                         UserManager.Instance.IsUserRightGranted(Globals.ActualUser,
                                             Permission.AccessConfiguration);

        #endregion UserRights
    }
}