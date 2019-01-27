using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class ProductManagementViewModel : ViewModelBase
    {
        #region UserRights
        public bool ShowProducts { get { return UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessProducts) || Globals.ActualUser.IsAdministrator; } }
        #endregion UserRights

        public ProductManagementViewModel()
        {
            if (IsInDesignMode)
                return;
        }
    }
}
