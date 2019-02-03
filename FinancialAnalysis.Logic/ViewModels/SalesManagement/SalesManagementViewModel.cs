using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class SalesManagementViewModel : ViewModelBase
    {
        public SalesManagementViewModel()
        {
            if (IsInDesignMode) return;
        }

        #region UserRights

        public bool ShowSalesOrders => Globals.ActualUser.IsAdministrator ||
                                       UserManager.Instance.IsUserRightGranted(Globals.ActualUser,
                                           Permission.AccessSalesOrders);

        public bool ShowPendingSaleOrders => Globals.ActualUser.IsAdministrator ||
                                             UserManager.Instance.IsUserRightGranted(Globals.ActualUser,
                                                 Permission.AccessPendingSaleOrders);

        #endregion UserRights
    }
}