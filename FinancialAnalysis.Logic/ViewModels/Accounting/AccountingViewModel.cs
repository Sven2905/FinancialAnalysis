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
        public bool ShowBookings { get { return Globals.ActualUser.IsUserRightGranted(Permission.AccessBooking) || ActualUser.IsAdministrator; } }
        public bool ShowBookingHistories { get { return Globals.ActualUser.IsUserRightGranted(Permission.AccessBookingHistory) || ActualUser.IsAdministrator; } }
        public bool ShowCostAccounts { get { return Globals.ActualUser.IsUserRightGranted(Permission.AccessCostAccount) || ActualUser.IsAdministrator; } }
        public bool ShowCreditorDebitors { get { return Globals.ActualUser.IsUserRightGranted(Permission.AccessCreditorDebitor) || ActualUser.IsAdministrator; } }
        public bool ShowTaxTypes { get { return Globals.ActualUser.IsUserRightGranted(Permission.AccessTaxType) || ActualUser.IsAdministrator; } }
        #endregion UserRights
    }
}
