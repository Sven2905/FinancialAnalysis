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
                                        Permission.AccountingBookings);

        public bool ShowBookingHistories => Globals.ActiveUser.IsAdministrator
                                            || UserManager.Instance.IsUserRightGranted(Globals.ActiveUser,
                                                Permission.AccountingBookingHistory);

        public bool ShowCostAccounts => Globals.ActiveUser.IsAdministrator
                                        || UserManager.Instance.IsUserRightGranted(Globals.ActiveUser,
                                            Permission.AccountingCostAccounts);

        public bool ShowCreditorDebitors => Globals.ActiveUser.IsAdministrator
                                            || UserManager.Instance.IsUserRightGranted(Globals.ActiveUser,
                                                Permission.AccountingCreditorDebitors);

        public bool ShowTaxTypes => Globals.ActiveUser.IsAdministrator
                                    || UserManager.Instance.IsUserRightGranted(Globals.ActiveUser,
                                        Permission.AccountingTaxTypes);

        public bool ShowCostCenters => Globals.ActiveUser.IsAdministrator
                                    || UserManager.Instance.IsUserRightGranted(Globals.ActiveUser,
                                        Permission.AccountingCostCenters);

        #endregion UserRights
    }
}