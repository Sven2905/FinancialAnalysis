using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Logic.ViewModel;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FinancialAnalysis.UI
{
    /// <summary>
    /// Zentrale Listener-Klasse für alle Nachrichten.
    /// </summary>
    class MessageListener
    {

        #region Konstruktor

        public MessageListener()
        {
            InitMessenger();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Registriert alle Nachrichten und deren Aktion nach dem Aufruf
        /// </summary>
        private void InitMessenger()
        {
            Messenger.Default.Register<OpenKontenrahmenWindowMessage>(this,
                 msg =>
                 {
                     var window = new KontenrahmenWindow();
                     var model = window.DataContext as KontenrahmenViewModel;
                     if (model != null)
                         model.AccountingType = msg.AccountingType;
                     window.ShowDialog();
                 });

            Messenger.Default.Register<OpenDialogWindowMessage>(this,
                 msg =>
                 {
                     var window = new DialogWindow();
                     var model = window.DataContext as DialogViewModel;
                     if (model != null)
                     {
                         model.Message = msg.Message;
                         model.Title = msg.Title;
                         model.MessageBoxImage = msg.MessageBoxImage;
                     }
                     window.ShowDialog();
                 });
        }

        #endregion

        #region Properties

        /// <summary>
        /// Nur zum Aufruf notwendig.
        /// </summary>
        public bool BindableProperty => true;

        #endregion

    }
}
