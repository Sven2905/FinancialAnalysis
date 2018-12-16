using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class MailViewModel : ViewModelBase
    {
        public MailViewModel()
        {
            SaveMailConfigCommand = new DelegateCommand(SaveMailConfiguration);
            SendTestMailCommand = new DelegateCommand(SendTestMail);
            MailConfiguration = new MailConfiguration();
            LoadMailConfiguration();
        }

        public MailConfiguration MailConfiguration { get; set; }
        public DelegateCommand SaveMailConfigCommand { get; set; }
        public DelegateCommand SendTestMailCommand { get; set; }

        private void SendTestMail()
        {
            if (MailConfiguration.LoginUser != "" && MailConfiguration.Password != "" && MailConfiguration.Server != "")
            {
                var mailData = new MailData() { Body = "Dies ist eine automatisch generierte Testmail.", Subject = "Testmail", To = MailConfiguration.Address };
                Mail.Send(mailData, MailConfiguration);
            }
        }

        private void SaveMailConfiguration()
        {
            if (MailConfiguration.MailConfigurationId == 0)
                using (var db = new DataLayer())
                    MailConfiguration.MailConfigurationId = db.MailConfigurations.Insert(MailConfiguration);
        }

        private void LoadMailConfiguration()
        {
            using (var db = new DataLayer())
                MailConfiguration = db.MailConfigurations.GetAll().FirstOrDefault();

            if (MailConfiguration == null)
                MailConfiguration = new MailConfiguration();
        }
    }
}
