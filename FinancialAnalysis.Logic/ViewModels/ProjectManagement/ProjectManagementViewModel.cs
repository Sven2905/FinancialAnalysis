using FinancialAnalysis.Models.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class ProjectManagementViewModel
    {
        public User ActualUser { get { return Globals.ActualUser; } }

        #region UserRights
        public bool ShowProjects { get { return Globals.ActualUser.IsUserRightGranted(Permission.AccessProject) || ActualUser.IsAdministrator; } }
        public bool ShowEmployees { get { return Globals.ActualUser.IsUserRightGranted(Permission.AccessEmployee) || ActualUser.IsAdministrator; } }
        public bool ShowCostCenters { get { return Globals.ActualUser.IsUserRightGranted(Permission.AccessCostCenter) || ActualUser.IsAdministrator; } }
        public bool ShowProjectWorkingTimes { get { return Globals.ActualUser.IsUserRightGranted(Permission.AccessProjectWorkingTime) || ActualUser.IsAdministrator; } }
        #endregion UserRights
    }
}
