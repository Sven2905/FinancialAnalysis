using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class ProductManagementViewModel : ViewModelBase
    {
        public ProductManagementViewModel()
        {
            if (IsInDesignMode) return;
        }

        #region UserRights

        public bool ShowProducts =>
            UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessProducts) ||
            Globals.ActualUser.IsAdministrator;

        #endregion UserRights
    }
}