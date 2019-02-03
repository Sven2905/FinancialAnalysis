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
            UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessMail) ||
            Globals.ActualUser.IsAdministrator;

        public bool ShowUsers => UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessUsers) ||
                                 Globals.ActualUser.IsAdministrator;

        public bool ShowMyCompany =>
            UserManager.Instance.IsUserRightGranted(Globals.ActualUser, Permission.AccessMyCompanies) ||
            Globals.ActualUser.IsAdministrator;

        #endregion UserRights
    }
}