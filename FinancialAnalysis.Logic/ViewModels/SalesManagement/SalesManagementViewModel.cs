using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class SalesManagementViewModel : ViewModelBase
    {
        #region UserRights 
        public bool ShowSalesOrders { get { return Globals.ActualUser.IsAdministrator || UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessSalesOrders); } }
        public bool ShowPendingSaleOrders { get { return Globals.ActualUser.IsAdministrator || UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessPendingSaleOrders); } }
        #endregion UserRights

        public SalesManagementViewModel()
        {
            if (IsInDesignMode)
                return;
        }
    }
}
