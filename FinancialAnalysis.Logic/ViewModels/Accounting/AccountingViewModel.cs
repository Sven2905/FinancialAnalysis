using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class AccountingViewModel : ViewModelBase
    {
        public AccountingViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }
        }

        #region UserRights

        public bool ShowBookings => Globals.ActiveUser.IsAdministrator
                                    || UserManager.Instance.IsUserRightGranted(Globals.ActiveUser,
                                        Permission.AccessBookings);

        public bool ShowBookingHistories => Globals.ActiveUser.IsAdministrator
                                            || UserManager.Instance.IsUserRightGranted(Globals.ActiveUser,
                                                Permission.AccessBookingHistory);

        public bool ShowCostAccounts => Globals.ActiveUser.IsAdministrator
                                        || UserManager.Instance.IsUserRightGranted(Globals.ActiveUser,
                                            Permission.AccessCostAccounts);

        public bool ShowCreditorDebitors => Globals.ActiveUser.IsAdministrator
                                            || UserManager.Instance.IsUserRightGranted(Globals.ActiveUser,
                                                Permission.AccessCreditorDebitors);

        public bool ShowTaxTypes => Globals.ActiveUser.IsAdministrator
                                    || UserManager.Instance.IsUserRightGranted(Globals.ActiveUser,
                                        Permission.AccessTaxTypes);

        public bool ShowCostCenters => Globals.ActiveUser.IsAdministrator
                                    || UserManager.Instance.IsUserRightGranted(Globals.ActiveUser,
                                        Permission.AccessCostCenters);
        #endregion UserRights
    }
}