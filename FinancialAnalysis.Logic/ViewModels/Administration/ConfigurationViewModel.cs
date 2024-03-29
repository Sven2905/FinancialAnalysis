﻿using DevExpress.Mvvm;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Administration;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class ConfigurationViewModel : ViewModelBase
    {
        public ConfigurationViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }

            OpenWebApiConfigurationCommand = new DelegateCommand(OpenWebApiConfigurationWindow);
        }

        public void OpenWebApiConfigurationWindow()
        {
            Messenger.Default.Send(new OpenWebApiConfigurationWindow());
        }

        public DelegateCommand OpenWebApiConfigurationCommand { get; set; }

        #region UserRights

        public bool ShowMailConfiguration =>
            UserManager.Instance.IsUserRightGranted(Globals.ActiveUser, Permission.ConfigurationMail) ||
            Globals.ActiveUser.IsAdministrator;

        public bool ShowUsers => UserManager.Instance.IsUserRightGranted(Globals.ActiveUser, Permission.ConfigurationUsers) ||
                                 Globals.ActiveUser.IsAdministrator;

        public bool ShowMyCompany =>
            UserManager.Instance.IsUserRightGranted(Globals.ActiveUser, Permission.ConfigurationMyCompanies) ||
            Globals.ActiveUser.IsAdministrator;

        #endregion UserRights
    }
}