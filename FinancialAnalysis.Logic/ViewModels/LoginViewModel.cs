using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        public LoginViewModel()
        {
            Seed();
            LoginCommand = new DelegateCommand(Login);
            ExitCommand = new DelegateCommand(Exit);
        }

        private void Seed()
        {
            using (var db = new DataLayer())
            {
                db.CreateDatabaseSchema();
                if (db.Users.GetAll().Count() == 0)
                {
                    var user = new User()
                    {
                        Firstname = "Admin",
                        Lastname = "Admin",
                        LoginUser = "Admin",
                        Password = "Password",
                        Mail = "admin@sven.tech",
                        Contraction = "AD"
                    };

                    db.Users.Insert(user);
                }
            }
        }

        private void Login()
        {
            if (CheckCredentials())
            {
                Globals.ActualUser = User;
                Messenger.Default.Send(new OpenSplashScreenMessage());
                Messenger.Default.Send(new OpenMainWindowMessage());
                return;
            }
            User = new User();
        }

        private void Exit() => Environment.Exit(0);

        private bool CheckCredentials()
        {
            if (User == null)
                return false;

            if (User.LoginUser != string.Empty && User.Password != string.Empty)
                using (var db = new DataLayer())
                    User = db.Users.GetUserByNameAndPassword(User.LoginUser, User.Password);

            if (User == null || User.UserId == 0)
                return false;

            return true;
        }

        public DelegateCommand LoginCommand { get; set; }
        public DelegateCommand ExitCommand { get; set; }
        public User User { get; set; } = new User();
    }
}
