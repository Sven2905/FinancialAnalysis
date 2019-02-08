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

        public bool ShowProjects => Globals.ActualUser.IsAdministrator ||
                            UserManager.Instance.IsUserRightGranted(Globals.ActualUser,
                                Permission.AccessProjects);
        public bool ShowEmployees => Globals.ActualUser.IsAdministrator ||
                            UserManager.Instance.IsUserRightGranted(Globals.ActualUser,
                                Permission.AccessEmployees);
        public bool ShowProjectWorkingTimes => Globals.ActualUser.IsAdministrator ||
                            UserManager.Instance.IsUserRightGranted(Globals.ActualUser,
                                Permission.AccessProjectWorkingTimes);

        #endregion UserRights
    }
}