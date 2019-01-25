using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Administration;
using System;
using System.Linq;
using System.Windows;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        #region Constructor

        public LoginViewModel()
        {
            if (IsInDesignMode)
                return;

            Seed();
            LoginCommand = new DelegateCommand(Login, () => (!string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrEmpty(Password)));
            ExitCommand = new DelegateCommand(Exit);
        }

        #endregion Constructor

        #region Fields

        private int _Counter = 0;

        #endregion Fields

        #region Methods

        private void Seed()
        {
            DataLayer.Instance.CreateDatabaseSchema();
            if (!DataLayer.Instance.Users.GetAll().Any())
            {
                var user = new User()
                {
                    IsAdministrator = true,
                    Firstname = "Mr.",
                    Lastname = "Admin",
                    LoginUser = "Admin",
                    Password = "Password",
                    Mail = "admin@sven.tech",
                    Contraction = "AD"
                };

                DataLayer.Instance.Users.Insert(user);

            }

            if (!DataLayer.Instance.UserRights.GetAll().Any())
            {
                var _Import = new Import();
                _Import.ImportUserRights();
                _Import.SeedTypes();
            }
        }

        private void Login()
        {
            if (CheckCredentials())
            {
                ShowError = false;
                Messenger.Default.Send(new OpenSplashScreenMessage());
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

        private void Exit() => Application.Current.Shutdown();

        private bool CheckCredentials()
        {
            User foundUser = UserManager.Instance.GetUserByNameAndPassword(UserName, Password);

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

            Globals.ActualUser = foundUser;
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
