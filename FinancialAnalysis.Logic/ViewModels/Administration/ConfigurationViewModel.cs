using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class ConfigurationViewModel : ViewModelBase
    {
        #region UserRights
        public bool ShowMailConfiguration { get { return UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessMail) || Globals.ActualUser.IsAdministrator; } }
        public bool ShowUsers { get { return UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessUsers) || Globals.ActualUser.IsAdministrator; } }
        public bool ShowMyCompany { get { return UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessMyCompanies) || Globals.ActualUser.IsAdministrator; } }
        #endregion UserRights

        public ConfigurationViewModel()
        {
            if (IsInDesignMode)
                return;
        }
    }
}
