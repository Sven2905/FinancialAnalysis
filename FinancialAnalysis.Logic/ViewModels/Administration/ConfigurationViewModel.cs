using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class ConfigurationViewModel : ViewModelBase
    {
        public ConfigurationViewModel()
        {
            if (IsInDesignMode) return;
        }

        #region UserRights

        public bool ShowMailConfiguration =>
            UserManager.Instance.IsUserRightGranted(Globals.ActiveUser, Permission.AccessMail) ||
            Globals.ActiveUser.IsAdministrator;

        public bool ShowUsers => UserManager.Instance.IsUserRightGranted(Globals.ActiveUser, Permission.AccessUsers) ||
                                 Globals.ActiveUser.IsAdministrator;

        public bool ShowMyCompany =>
            UserManager.Instance.IsUserRightGranted(Globals.ActiveUser, Permission.AccessMyCompanies) ||
            Globals.ActiveUser.IsAdministrator;

        #endregion UserRights
    }
}