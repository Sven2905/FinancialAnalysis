using DevExpress.Mvvm;
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
    public class ConfigurationViewModel : ViewModelBase
    {
        public ConfigurationViewModel()
        {
            SaveMailConfigCommand = new DelegateCommand(SaveMailConfiguration);
            MailConfiguration = new MailConfiguration();
            LoadMailConfiguration();
        }

        public MailConfiguration MailConfiguration { get; set; }
        public DelegateCommand SaveMailConfigCommand { get; set; }
        string filePath = Environment.CurrentDirectory + "\\config.dat";

        private void SaveMailConfiguration()
        {
            MailConfiguration.Server = Encryption.EncryptText(MailConfiguration.Server, @"G*ZCx[WD;d<k3*Gc");
            MailConfiguration.User = Encryption.EncryptText(MailConfiguration.User, @"G*ZCx[WD;d<k3*Gc");
            MailConfiguration.Password = Encryption.EncryptText(MailConfiguration.Password, @"G*ZCx[WD;d<k3*Gc");

            var result = XmlHelper.ToXml(MailConfiguration);
            using (var sw = new StreamWriter(filePath))
            {
                sw.WriteLine(result);
                sw.Close();
            }
        }

        private void LoadMailConfiguration()
        {
            if (!File.Exists(filePath))
                return;

            var xml = string.Empty;

            using (var sr = new StreamReader(filePath))
            {
                xml = sr.ReadToEnd();
            }

            MailConfiguration = XmlHelper.FromXml<MailConfiguration>(xml);
            MailConfiguration.Server = Encryption.DecryptText(MailConfiguration.Server, @"G*ZCx[WD;d<k3*Gc");
            MailConfiguration.User = Encryption.DecryptText(MailConfiguration.User, @"G*ZCx[WD;d<k3*Gc");
            MailConfiguration.Password = Encryption.DecryptText(MailConfiguration.Password, @"G*ZCx[WD;d<k3*Gc");
        }
    }
}
