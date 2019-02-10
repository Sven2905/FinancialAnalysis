using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class SalesManagementViewModel : ViewModelBase
    {
        public SalesManagementViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }
        }

        #region UserRights

        public bool ShowSalesOrders => Globals.ActiveUser.IsAdministrator || UserManager.Instance.IsUserRightGranted(Globals.ActiveUser, Permission.AccessSalesOrders);

        public bool ShowPendingSaleOrders => Globals.ActiveUser.IsAdministrator ||
                                             UserManager.Instance.IsUserRightGranted(Globals.ActiveUser,
                                                 Permission.AccessPendingSaleOrders);

        #endregion UserRights
    }
}