using DevExpress.Mvvm;

using FinancialAnalysis.Logic.Messages;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Windows;
using Utilities;
using WebApiWrapper;
using WebApiWrapper.Administration;
using WebApiWrapper.SalesManagement;

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
            if (IsInDesignMode)
            {
                return;
            }

            if (File.Exists(@".\WebApiConfig.cfg"))
            {
                LoadWebApiConfigurationFromFile();
            }
            else
            {
                Messenger.Default.Send(new OpenWebApiConfigurationWindow());
            }

            if (!PingHost(WebApiConfiguration.Instance.Server))
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Verbindungsfehler", "Die WebApi ist nicht erreichbar, bitte Verbindungsparameter überprüfen.", MessageBoxImage.Error));
                Messenger.Default.Send(new OpenWebApiConfigurationWindow());
            }

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
            var webApiConfigurationFile = BinarySerialization.ReadFromBinaryFile<WebApiConfiguration>(@".\WebApiConfig.cfg");
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
            var foundUser = UserManager.Instance.GetUserByNameAndPassword(UserName, Password);

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

            //var notificationManager = new NotificationManager();

            //notificationManager.Show(new NotificationContent
            //{
            //    Title = "Login",
            //    Message = "Sie wurden erfolgreich eingeloggt.",
            //    Type = NotificationType.Information
            //});


            Globals.ActiveUser = foundUser;
            return true;
        }

        private bool PingHost(string nameOrAddress)
        {
            bool pingable = false;
            Ping pinger = null;

            try
            {
                pinger = new Ping();
                PingReply reply = pinger.Send(nameOrAddress);
                pingable = reply.Status == IPStatus.Success;
            }
            catch (PingException)
            {
                // Discard PingExceptions and return false;
            }
            finally
            {
                if (pinger != null)
                {
                    pinger.Dispose();
                }
            }

            return pingable;
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