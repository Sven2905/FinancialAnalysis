using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class ProjectManagementViewModel : ViewModelBase
    {
        public User ActualUser { get { return Globals.ActualUser; } }

        public ProjectManagementViewModel()
        {
            if (IsInDesignMode)
                return;
        }

        #region UserRights
        public bool ShowProjects { get { return UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessProjects) || ActualUser.IsAdministrator; } }
        public bool ShowEmployees { get { return UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessEmployees) || ActualUser.IsAdministrator; } }
        public bool ShowCostCenters { get { return UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessCostCenters) || ActualUser.IsAdministrator; } }
        public bool ShowProjectWorkingTimes { get { return UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessProjectWorkingTimes) || ActualUser.IsAdministrator; } }
        #endregion UserRights
    }
}
