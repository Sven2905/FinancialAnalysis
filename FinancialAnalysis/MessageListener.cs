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
