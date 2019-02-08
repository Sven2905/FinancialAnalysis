using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class AccountingViewModel : ViewModelBase
    {
        public AccountingViewModel()
        {
            if (IsInDesignMode) return;
        }

        #region UserRights

        public bool ShowBookings => Globals.ActualUser.IsAdministrator ||
                                    UserManager.Instance.IsUserRightGranted(Globals.ActualUser,
                                        Permission.AccessBookings);

        public bool ShowBookingHistories => Globals.ActualUser.IsAdministrator ||
                                            UserManager.Instance.IsUserRightGranted(Globals.ActualUser,
                                                Permission.AccessBookingHistory);

        public bool ShowCostAccounts => Globals.ActualUser.IsAdministrator ||
                                        UserManager.Instance.IsUserRightGranted(Globals.ActualUser,
                                            Permission.AccessCostAccounts);

        public bool ShowCreditorDebitors => Globals.ActualUser.IsAdministrator ||
                                            UserManager.Instance.IsUserRightGranted(Globals.ActualUser,
                                                Permission.AccessCreditorDebitors);

        public bool ShowTaxTypes => Globals.ActualUser.IsAdministrator ||
                                    UserManager.Instance.IsUserRightGranted(Globals.ActualUser,
                                        Permission.AccessTaxTypes);

        public bool ShowCostCenters => Globals.ActualUser.IsAdministrator ||
                                    UserManager.Instance.IsUserRightGranted(Globals.ActualUser, 
                                        Permission.AccessCostCenters);
        #endregion UserRights
    }
}