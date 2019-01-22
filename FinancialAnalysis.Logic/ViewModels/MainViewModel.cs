using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;
using System;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public User ActualUser { get { return Globals.ActualUser; } }

        #region UserRights
        public bool ShowBookingHistory { get { return UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessBookingHistory) || ActualUser.IsAdministrator; } }
        public bool ShowTaxType { get { return UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessTaxType) || ActualUser.IsAdministrator; } }
        public bool ShowProjectManagement { get { return UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessProjectManagement) || ActualUser.IsAdministrator; } }
        public bool ShowBooking { get { return UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessBooking) || ActualUser.IsAdministrator; } }
        public bool ShowCostAccount { get { return UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessCostAccount) || ActualUser.IsAdministrator; } }
        public bool ShowCreditorsDebitors { get { return UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessCreditorDebitor) || ActualUser.IsAdministrator; } }
        public bool ShowConfiguration { get { return UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessConfiguration) || ActualUser.IsAdministrator; } }
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
            {
                return;
            }

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