using FinancialAnalysis.Models.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class ConfigurationViewModel
    {
        #region UserRights
        public bool ShowMailConfiguration { get { return UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessMail) || ActualUser.IsAdministrator; } }
        public bool ShowUsers { get { return UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessUsers) || ActualUser.IsAdministrator; } }
        #endregion UserRights

        public User ActualUser { get { return Globals.ActualUser; } }
    }
}
