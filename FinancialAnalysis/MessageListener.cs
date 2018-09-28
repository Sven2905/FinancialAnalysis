using DevExpress.Mvvm;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Logic.ViewModels;
using FinancialAnalysis.Windows;

namespace FinancialAnalysis.UI
{
    /// <summary>
    /// Zentrale Listener-Klasse für alle Nachrichten.
    /// </summary>
    internal class MessageListener
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
                     {
                         model.AccountingType = msg.AccountingType;
                     }

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
                     var model = window.DataContext as PDFViewerViewModel;
                     if (model != null)
                     {
                         model.ScannedDocumentId = msg.ScannedDocumentId;
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
