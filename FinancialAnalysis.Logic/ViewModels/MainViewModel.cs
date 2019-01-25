using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;
using System;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region UserRights
        public bool ShowAccounting { get { return UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessAccounting) || Globals.ActualUser.IsAdministrator; } }
        public bool ShowProjectManagement { get { return UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessProjectManagement) || Globals.ActualUser.IsAdministrator; } }
        public bool ShowWarehouseManagement { get { return UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessWarehouseManagement) || Globals.ActualUser.IsAdministrator; } }
        public bool ShowConfiguration { get { return UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessConfiguration) || Globals.ActualUser.IsAdministrator; } }
        #endregion UserRights

        public string CurrentTime
        {
            get { return _currentTime; }
            set { _currentTime = value; RaisePropertiesChanged(); }
        }

        private string _currentTime;

        /// <summary>
        ///     Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
                return;

            UpdateTime();
        }

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
    }
}