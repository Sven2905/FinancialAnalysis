using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class WarehouseManagementViewModel : ViewModelBase
    {
        public WarehouseManagementViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }
        }

        #region UserRights

        public bool ShowStocking => Globals.ActiveUser.IsAdministrator || UserManager.Instance.IsUserRightGranted(Globals.ActiveUser, Permission.AccessWarehouses);
        public bool ShowStockyards => Globals.ActiveUser.IsAdministrator || UserManager.Instance.IsUserRightGranted(Globals.ActiveUser, Permission.AccessStockyards);

        #endregion UserRights
    }
}