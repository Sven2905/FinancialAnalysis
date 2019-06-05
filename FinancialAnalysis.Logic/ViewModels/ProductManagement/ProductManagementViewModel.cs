using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class ProductManagementViewModel : ViewModelBase
    {
        public ProductManagementViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }
        }

        #region UserRights

        public bool ShowProducts =>
            UserManager.Instance.IsUserRightGranted(Globals.ActiveUser, Permission.Products) ||
            Globals.ActiveUser.IsAdministrator;

        #endregion UserRights
    }
}