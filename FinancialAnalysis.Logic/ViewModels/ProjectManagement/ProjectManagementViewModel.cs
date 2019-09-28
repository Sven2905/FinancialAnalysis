using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class ProjectManagementViewModel : ViewModelBase
    {
        public ProjectManagementViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }
        }

        public User ActualUser => Globals.ActiveUser;

        #region UserRights

        public bool ShowProjects => Globals.ActiveUser.IsAdministrator ||
                            UserManager.Instance.IsUserRightGranted(Globals.ActiveUser,
                                Permission.ProjectManagementProjects);

        public bool ShowUsers => Globals.ActiveUser.IsAdministrator ||
                            UserManager.Instance.IsUserRightGranted(Globals.ActiveUser,
                                Permission.ProjectManagementUsers);

        public bool ShowProjectWorkingTimes => Globals.ActiveUser.IsAdministrator ||
                            UserManager.Instance.IsUserRightGranted(Globals.ActiveUser,
                                Permission.ProjectManagementProjectWorkingTimes);

        #endregion UserRights
    }
}