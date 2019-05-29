using DevExpress.Mvvm;
using FinancialAnalysis.Models.Administration;
using System;
using System.IO;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class DatabaseConfigurationViewModel : ViewModelBase
    {
        public DatabaseConfigurationViewModel()
        {
            SaveCommand = new DelegateCommand(() => SaveToFile());
            if (File.Exists(@".\config.cfg"))
            {
                LoadFromFile();
            }
            CloseCommand = new DelegateCommand(() => CloseAction());
        }

        public DatabaseConfiguration DatabaseConfiguration { get; set; } = new DatabaseConfiguration();
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand CloseCommand { get; set; }
        public Action CloseAction { get; set; }
        public string Server { get; set; }
        public string Database { get; set; }
        public string User { get; set; }
        public string Password { get; set; }

        private void SaveToFile()
        {
            DatabaseConfiguration.Server = Encryption.EncryptText(Server, @"s*}z3;p/Wvs,X8'qLJbrbJ}7YwDtH/TP");
            DatabaseConfiguration.Database = Encryption.EncryptText(Database, @"s*}z3;p/Wvs,X8'qLJbrbJ}7YwDtH/TP");
            DatabaseConfiguration.User = Encryption.EncryptText(User, @"s*}z3;p/Wvs,X8'qLJbrbJ}7YwDtH/TP");
            DatabaseConfiguration.Password = Encryption.EncryptText(Password, @"s*}z3;p/Wvs,X8'qLJbrbJ}7YwDtH/TP");
            BinarySerialization.WriteToBinaryFile(@".\config.cfg", DatabaseConfiguration);
        }

        private void LoadFromFile()
        {
            DatabaseConfiguration = BinarySerialization.ReadFromBinaryFile<DatabaseConfiguration>(@".\config.cfg");
            Server = Encryption.DecryptText(DatabaseConfiguration.Server, @"s*}z3;p/Wvs,X8'qLJbrbJ}7YwDtH/TP");
            Database = Encryption.DecryptText(DatabaseConfiguration.Database, @"s*}z3;p/Wvs,X8'qLJbrbJ}7YwDtH/TP");
            User = Encryption.DecryptText(DatabaseConfiguration.User, @"s*}z3;p/Wvs,X8'qLJbrbJ}7YwDtH/TP");
            Password = Encryption.DecryptText(DatabaseConfiguration.Password, @"s*}z3;p/Wvs,X8'qLJbrbJ}7YwDtH/TP");
        }
    }
}
