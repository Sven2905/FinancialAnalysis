﻿using DevExpress.Mvvm;
using DevExpress.Xpf.Core;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Logic.ViewModels;
using FinancialAnalysis.UI.Desktop;
using FinancialAnalysis.Windows;
using System.Windows;

namespace FinancialAnalysis.UI
{
    /// <summary>
    ///     Zentrale Listener-Klasse für alle Nachrichten.
    /// </summary>
    internal class MessageListener
    {
        #region Konstruktor

        public MessageListener()
        {
            InitMessenger();
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Nur zum Aufruf notwendig.
        /// </summary>
        public bool BindableProperty => true;

        #endregion

        #region Methods

        /// <summary>
        ///     Registriert alle Nachrichten und deren Aktion nach dem Aufruf
        /// </summary>
        private void InitMessenger()
        {
            Messenger.Default.Register<OpenKontenrahmenWindowMessage>(this,
                msg =>
                {
                    var window = new KontenrahmenWindow();
                    if (window.DataContext is KontenrahmenViewModel model) model.AccountingType = msg.AccountingType;

                    window.ShowDialog();
                });

            Messenger.Default.Register<OpenDialogWindowMessage>(this,
                msg =>
                {
                    var window = new DialogWindow();
                    if (window.DataContext is DialogViewModel model)
                    {
                        model.Message = msg.Message;
                        model.Title = msg.Title;
                        model.MessageBoxImage = msg.MessageBoxImage;
                    }

                    window.ShowDialog();
                });
            Messenger.Default.Register<OpenCompanyWindowMessage>(this,
                msg =>
                {
                    var window = new CompanyWindow();
                    window.ShowDialog();
                });
            Messenger.Default.Register<OpenPDFViewerWindowMessage>(this,
                msg =>
                {
                    var window = new PDFViewerWindow();
                    if (window.DataContext is PDFViewerViewModel model)
                    {
                        if (msg.ScannedDocumentId != 0)
                            model.ScannedDocumentId = msg.ScannedDocumentId;
                        else
                            model.Path = msg.Path;
                    }

                    window.ShowDialog();
                });
            Messenger.Default.Register<OpenMainWindowMessage>(this,
                 msg =>
                 {
                     var window = new MainWindow();
                     var model = window.DataContext as MainViewModel;
                     Application.Current.MainWindow.Close();
                     Application.Current.MainWindow = null;
                     Application.Current.MainWindow = window;
                     window.Show();
                 });
            Messenger.Default.Register<OpenSplashScreenMessage>(this,
                msg =>
                {
                    DXSplashScreen.Show<SplashScreenView>();
                });
        }

        #endregion
    }
}