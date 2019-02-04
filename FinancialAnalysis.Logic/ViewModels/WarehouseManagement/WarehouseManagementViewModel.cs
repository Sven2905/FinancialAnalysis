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

        public bool ShowStocking => Globals.ActualUser.IsAdministrator || UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessWarehouses);
        public bool ShowStockyards => Globals.ActualUser.IsAdministrator || UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessStockyards);

        #endregion UserRights
    }
}