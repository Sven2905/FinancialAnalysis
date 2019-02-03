using System.Linq;
using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models.Mail;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class MailViewModel : ViewModelBase
    {
        public MailViewModel()
        {
            if (IsInDesignMode) return;

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
                var mailData = new MailData
                {
                    Body = "Dies ist eine automatisch generierte Testmail.", Subject = "Testmail",
                    To = MailConfiguration.Address
                };
                Mail.Send(mailData, MailConfiguration);
            }
        }

        private void SaveMailConfiguration()
        {
            if (MailConfiguration.MailConfigurationId == 0)
                MailConfiguration.MailConfigurationId =
                    DataContext.Instance.MailConfigurations.Insert(MailConfiguration);
        }

        private void LoadMailConfiguration()
        {
            MailConfiguration = DataContext.Instance.MailConfigurations.GetAll().FirstOrDefault();

            if (MailConfiguration == null) MailConfiguration = new MailConfiguration();
        }
    }
}