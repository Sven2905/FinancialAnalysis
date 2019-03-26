﻿using DevExpress.Mvvm;
using DevExpress.Mvvm.DataAnnotations;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Administration;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Formulas;
using Formulas.DepreciationMethods;
using FinancialAnalysis.Models.ProjectManagement;
using System.Collections.Generic;
using WebApiWrapper.ProjectManagement;

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

#if (DEBUG)
            UserName = "Admin";
            Password = "Password";
#endif

            Task.Run(() => { Seed(); });

            LoginCommand = new DelegateCommand(Login, () => !string.IsNullOrWhiteSpace(UserName) && !string.IsNullOrEmpty(Password));
            ExitCommand = new DelegateCommand(Exit);
        }

        #endregion Constructor

        #region Methods

        private void Seed()
        {
            DataContext.Instance.CreateDatabaseSchema();
            if (!DataContext.Instance.Users.GetAll().Any())
            {
                var user = new User
                {
                    IsAdministrator = true,
                    Firstname = "Mr.",
                    Lastname = "Admin",
                    LoginUser = "Admin",
                    Password = "Password",
                    Mail = "admin@sven.tech",
                    Contraction = "AD"
                };

                DataContext.Instance.Users.Insert(user);
            }

            var _Import = new Import();
            if (!DataContext.Instance.UserRights.GetAll().Any())
            {
                _Import.ImportUserRights();
            }
            if (!DataContext.Instance.InvoiceTypes.GetAll().Any())
            {
                _Import.SeedTypes();
            }
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