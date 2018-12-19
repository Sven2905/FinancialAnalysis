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
            Seed();
            LoginCommand = new DelegateCommand(Login, () => (!string.IsNullOrWhiteSpace(User.LoginUser) && !string.IsNullOrEmpty(User.Password)));
            ExitCommand = new DelegateCommand(Exit);
        }

        #endregion Constructor

        #region Fields

        private int _Counter = 0;

        #endregion Fields

        #region Methods

        private void Seed()
        {
            using (var db = new DataLayer())
            {
                db.CreateDatabaseSchema();
                if (!db.Users.GetAll().Any())
                {
                    var user = new User()
                    {
                        IsAdministrator = true,
                        Firstname = "Admin",
                        Lastname = "Admin",
                        LoginUser = "Admin",
                        Password = "Password",
                        Mail = "admin@sven.tech",
                        Contraction = "AD"
                    };

                    db.Users.Insert(user);

                }

                if (!db.UserRights.GetAll().Any())
                {
                    var _Import = new Import();
                    _Import.ImportUserRights();
                }
            }
        }

        private void Login()
        {
            if (CheckCredentials())
            {
                ShowError = false;
                Globals.ActualUser = User;
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
            User foundUser;
            using (var db = new DataLayer())
            {
                foundUser = db.Users.GetUserByNameAndPassword(User.LoginUser, User.Password);
            }

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

            User = foundUser;
            return true;
        }

        #endregion Methods

        #region Properties

        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand ExitCommand { get; set; }
        public User User { get; set; } = new User();
        public bool ShowError { get; set; }
        public string ErrorText { get; set; }

        #endregion Properties
    }
}
