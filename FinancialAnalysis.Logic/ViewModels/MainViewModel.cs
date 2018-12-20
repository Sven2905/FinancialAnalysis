using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Administration;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.ViewModels
{
    /// <summary>
    ///     This class contains properties that the main View can data bind to.
    ///     <para>
    ///         Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    ///     </para>
    ///     <para>
    ///         You can also use Blend to data bind with the tool's support.
    ///     </para>
    ///     <para>
    ///         See http://www.galasoft.ch/mvvm
    ///     </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public User ActualUser { get { return Globals.ActualUser; } }

        #region UserRights
        public bool ShowBooking { get { return Globals.ActualUser.IsUserRightGranted(Permission.AccessBooking) || ActualUser.IsAdministrator; } }
        public bool ShowBookingHistory { get { return Globals.ActualUser.IsUserRightGranted(Permission.AccessBookingHistory) || ActualUser.IsAdministrator; } }
        public bool ShowProjectManagement { get { return Globals.ActualUser.IsUserRightGranted(Permission.AccessProjectManagement) || ActualUser.IsAdministrator; } }
        public bool ShowTaxType { get { return Globals.ActualUser.IsUserRightGranted(Permission.AccessTaxType) || ActualUser.IsAdministrator; } }
        public bool ShowCostAccount { get { return Globals.ActualUser.IsUserRightGranted(Permission.AccessCostAccount) || ActualUser.IsAdministrator; } }
        public bool ShowCreditorsDebitors { get { return Globals.ActualUser.IsUserRightGranted(Permission.AccessCreditorDebitor) || ActualUser.IsAdministrator; } }
        public bool ShowConfiguration { get { return Globals.ActualUser.IsUserRightGranted(Permission.AccessConfiguration) || ActualUser.IsAdministrator; } }
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

            using (var db = new DataLayer())
            {
                if (db.TaxTypes.GetAll().Count() == 0)
                {
                    var _Import = new Import();
                    db.TaxTypes.Seed();
                    _Import.ImportCostAccounts(Standardkontenrahmen.SKR03);
                }
            }

            UpdateTime();
        }

        private void UpdateTime()
        {
            Task.Run(() =>
            {
                CurrentTime = DateTime.Now.ToString("G");
                Task.Delay(1000);
                UpdateTime();
            });
        }
    }
}