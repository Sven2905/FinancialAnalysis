using DevExpress.Mvvm;
using FinancialAnalysis.Logic.Messages;
using System;
using System.IO;
using System.Windows;
using Utilities;
using WebApiWrapper;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        #region Fields

        private int _Counter;

        #endregion Fields

        #region Constructor

        public LoginViewModel()
        {
            //        string deviceId = new DeviceIdBuilder()
            //.AddMachineName()
            //.AddMacAddress()
            //.AddProcessorId()
            //.AddMotherboardSerialNumber()
            //.ToString();

            if (IsInDesignMode)
                return;

            if (File.Exists(@".\WebApiConfig.cfg"))
                LoadWebApiConfigurationFromFile();
            else
                Messenger.Default.Send(new OpenWebApiConfigurationWindow());

#if (DEBUG)
            UserName = "Admin";
            Password = "Password";
#endif

            LoginCommand = new DelegateCommand(Login, () => !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrEmpty(Password));
            ExitCommand = new DelegateCommand(Exit);
        }

        #endregion Constructor

        #region Methods

        private void LoadWebApiConfigurationFromFile()
        {
            WebApiConfiguration webApiConfigurationFile = BinarySerialization.ReadFromBinaryFile<WebApiConfiguration>(@".\WebApiConfig.cfg");
            WebApiConfiguration.Instance.Server = webApiConfigurationFile.Server;
            WebApiConfiguration.Instance.Port = webApiConfigurationFile.Port;
        }

        private void Login()
        {
            if (CheckCredentials())
            {
                ShowError = false;
                //Messenger.Default.Send(new OpenSplashScreenMessage());
                Messenger.Default.Send(new OpenMainWindowMessage());
            }
            else
            {
                _Counter++;
            }

            if (_Counter >= 3)
            {
                Exit();
            }
        }

        private void Exit()
        {
            Application.Current.Shutdown();
        }

        private bool CheckCredentials()
        {
            Models.Administration.User foundUser = UserManager.Instance.GetUserByNameAndPassword(UserName, Password);

            if (foundUser == null)
            {
                ShowError = true;
                ErrorText = "Falscher Benutzername oder Passwort!";
                return false;
            }

            if (!foundUser.IsActive)
            {
                ShowError = true;
                ErrorText = "Benutzer ist deaktiviert!";
                return false;
            }

            Globals.ActiveUser = foundUser;
            return true;
        }

        #endregion Methods

        #region Properties

        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand ExitCommand { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool ShowError { get; set; }
        public string ErrorText { get; set; }

        #endregion Properties
    }
}