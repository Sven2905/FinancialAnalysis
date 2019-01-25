using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class WarehouseManagementViewModel : ViewModelBase
    {
        #region UserRights
        public bool ShowWarehouses { get { return UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessWarehouse) || Globals.ActualUser.IsAdministrator; } }
        public bool ShowStockyards { get { return UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessStockyard) || Globals.ActualUser.IsAdministrator; } }
        #endregion UserRights

        public WarehouseManagementViewModel()
        {
            if (IsInDesignMode)
                return;
        }
    }
}
