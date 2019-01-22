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
        public User ActualUser { get { return Globals.ActualUser; } }

        #region UserRights
        public bool ShowBookings { get { return UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessBooking) || ActualUser.IsAdministrator; } }
        public bool ShowBookingHistories { get { return UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessBookingHistory) || ActualUser.IsAdministrator; } }
        public bool ShowCostAccounts { get { return UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessCostAccount) || ActualUser.IsAdministrator; } }
        public bool ShowCreditorDebitors { get { return UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessCreditorDebitor) || ActualUser.IsAdministrator; } }
        public bool ShowTaxTypes { get { return UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessTaxType) || ActualUser.IsAdministrator; } }
        #endregion UserRights
    }
}
