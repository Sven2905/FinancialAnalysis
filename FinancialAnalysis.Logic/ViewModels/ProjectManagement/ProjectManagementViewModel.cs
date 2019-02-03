using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class ProjectManagementViewModel : ViewModelBase
    {
        public ProjectManagementViewModel()
        {
            if (IsInDesignMode) return;
        }

        public User ActualUser => Globals.ActualUser;

        #region UserRights

        public bool ShowProjects =>
            UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessProjects) ||
            ActualUser.IsAdministrator;

        public bool ShowEmployees =>
            UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessEmployees) ||
            ActualUser.IsAdministrator;

        public bool ShowCostCenters =>
            UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessCostCenters) ||
            ActualUser.IsAdministrator;

        public bool ShowProjectWorkingTimes =>
            UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessProjectWorkingTimes) ||
            ActualUser.IsAdministrator;

        #endregion UserRights
    }
}