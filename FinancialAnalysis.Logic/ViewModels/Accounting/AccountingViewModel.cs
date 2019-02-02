using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class AccountingViewModel : ViewModelBase
    {
        #region UserRights
        public bool ShowBookings { get { return Globals.ActualUser.IsAdministrator || UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessBookings); } }
        public bool ShowBookingHistories { get { return Globals.ActualUser.IsAdministrator || UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessBookingHistory); } }
        public bool ShowCostAccounts { get { return Globals.ActualUser.IsAdministrator || UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessCostAccounts); } }
        public bool ShowCreditorDebitors { get { return Globals.ActualUser.IsAdministrator || UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessCreditorDebitors); } }
        public bool ShowTaxTypes { get { return Globals.ActualUser.IsAdministrator || UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessTaxTypes); } }
        #endregion UserRights

        public AccountingViewModel()
        {
            if (IsInDesignMode)
                return;
        }
    }
}
