﻿using DevExpress.Mvvm;
using FinancialAnalysis.Models.MailManagement;
using Utilities;
using WebApiWrapper.MailManagement;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class MailConfigurationViewModel : ViewModelBase
    {
        public MailConfigurationViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }

            SaveMailConfigCommand = new DelegateCommand(SaveMailConfiguration);
            SendTestMailCommand = new DelegateCommand(SendTestMail);
            MailConfiguration = new MailConfiguration();
            LoadMailConfiguration();
        }

        public string Password { get; set; }
        public MailConfiguration MailConfiguration { get; set; }
        public DelegateCommand SaveMailConfigCommand { get; set; }
        public DelegateCommand SendTestMailCommand { get; set; }
        public SvenTechCollection<MailConfiguration> MailConfigurationList { get; set; }
        public MailConfiguration SelectedMailConfiguration { get; set; }

        private void SendTestMail()
        {
            if (MailConfiguration.LoginUser != "" && MailConfiguration.Password != "" && MailConfiguration.Server != "")
            {
                MailConfiguration.SetPassword(Password);
                MailData mailData = new MailData
                {
                    Body = "Dies ist eine automatisch generierte Testmail.",
                    Subject = "Testmail",
                    To = MailConfiguration.Address
                };
                Mail.Send(mailData, MailConfiguration);
            }
        }

        private void SaveMailConfiguration()
        {
            MailConfiguration.SetPassword(Password);
            if (MailConfiguration.MailConfigurationId == 0)
            {
                MailConfiguration.MailConfigurationId = MailConfigurations.Insert(MailConfiguration);
            }
            else
            {
                MailConfigurations.Update(MailConfiguration);
            }
        }

        private void LoadMailConfiguration()
        {
            MailConfigurationList = MailConfigurations.GetAll().ToSvenTechCollection();

            if (MailConfiguration == null)
            {
                MailConfiguration = new MailConfiguration();
            }
            Password = MailConfiguration.GetPasswordDecrypted();
        }
    }
}