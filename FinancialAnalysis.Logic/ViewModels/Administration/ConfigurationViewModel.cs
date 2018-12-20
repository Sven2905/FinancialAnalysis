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
        public bool ShowMailConfiguration { get { return Globals.ActualUser.IsUserRightGranted(Permission.AccessMail) || ActualUser.IsAdministrator; } }
        public bool ShowUsers { get { return Globals.ActualUser.IsUserRightGranted(Permission.AccessUsers) || ActualUser.IsAdministrator; } }
        #endregion UserRights

        public User ActualUser { get { return Globals.ActualUser; } }
    }
}
