using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class WarehouseManagementViewModel : ViewModelBase
    {
        public WarehouseManagementViewModel()
        {
            if (IsInDesignMode) return;
        }

        #region UserRights

        public bool ShowWarehouses =>
            UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessWarehouses) ||
            Globals.ActualUser.IsAdministrator;

        public bool ShowStockyards =>
            UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessStockyards) ||
            Globals.ActualUser.IsAdministrator;

        #endregion UserRights
    }
}