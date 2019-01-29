using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;
using System;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region UserRights
        public bool ShowAccounting { get { return Globals.ActualUser.IsAdministrator || UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessAccounting); } }
        public bool ShowProjectManagement { get { return Globals.ActualUser.IsAdministrator || UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessProjectManagement); } }
        public bool ShowProducts { get { return Globals.ActualUser.IsAdministrator || UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessProducts); } }
        public bool ShowWarehouseManagement { get { return Globals.ActualUser.IsAdministrator || UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessWarehouseManagement); } }
        public bool ShowConfiguration { get { return Globals.ActualUser.IsAdministrator || UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessConfiguration); } }
        #endregion UserRights

        public string CurrentTime
        {
            get { return _currentTime; }
            set { _currentTime = value; RaisePropertiesChanged(); }
        }

        public User ActualUser { get => Globals.ActualUser; }

        /// <summary>
        ///     Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
                return;

            UpdateTime();
        }

        private string _currentTime;
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