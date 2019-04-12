using DevExpress.Mvvm;
using FinancialAnalysis.Logic.General;
using FinancialAnalysis.Models.Administration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using WebApiWrapper;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class WebApiConfigurationViewModel : ViewModelBase
    {
        public WebApiConfigurationViewModel()
        {
            SaveCommand = new DelegateCommand(() => { SaveToFile(); CloseAction(); });
            if (File.Exists(@".\WebApiConfig.cfg"))
            {
                LoadFromFile();
            }
            CloseCommand = new DelegateCommand(() => CloseAction());
        }

        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CloseCommand { get; set; }
        public Action CloseAction { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }

        private void SaveToFile()
        {
            WebApiConfiguration.Instance.Server = Server;
            WebApiConfiguration.Instance.Port = Port;
            BinarySerialization.WriteToBinaryFile(@".\WebApiConfig.cfg", WebApiConfiguration.Instance);
            NotificationMessages.ShowSuccess(message: "Einstellungen wurden erfolgreich gespeichert.");
        }

        private void LoadFromFile()
        {
            var webApiConfigurationFile = BinarySerialization.ReadFromBinaryFile<WebApiConfiguration>(@".\WebApiConfig.cfg");
            Server = webApiConfigurationFile.Server;
            Port = webApiConfigurationFile.Port;
        }
    }
}
