using System.Windows;
using DevExpress.Mvvm;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Logic.ViewModels;
using FinancialAnalysis.UI.Desktop;
using FinancialAnalysis.Windows;

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

        #endregion Konstruktor

        #region Properties

        /// <summary>
        ///     Nur zum Aufruf notwendig.
        /// </summary>
        public bool BindableProperty => true;

        #endregion Properties

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
            Messenger.Default.Register<OpenClientWindowMessage>(this,
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
                        else if (!string.IsNullOrEmpty(msg.Path))
                            model.Path = msg.Path;
                        else
                        {
                            model.MemoryStream = msg.MemoryStream;
                        }
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
            //Messenger.Default.Register<OpenSplashScreenMessage>(this,
            //    msg =>
            //    {
            //        DXSplashScreen.Show<SplashScreenView>();
            //    });
            Messenger.Default.Register<OpenProductCategoriesWindowMessage>(this,
                msg =>
                {
                    var window = new ProductCategoriesWindow();
                    window.ShowDialog();
                });
            Messenger.Default.Register<OpenCostCenterCategoriesWindowMessage>(this,
                msg =>
                {
                    var window = new CostCenterCategoriesWindow();
                    window.ShowDialog();
                });
            Messenger.Default.Register<OpenSalesTypesWindowMessage>(this,
                msg =>
                {
                    var window = new SalesTypesWindow();
                    window.ShowDialog();
                });
            Messenger.Default.Register<OpenInvoiceTypesWindowMessage>(this,
                msg =>
                {
                    var window = new InvoiceTypesWindow();
                    window.ShowDialog();
                });
            Messenger.Default.Register<OpenWarehousesWindowMessage>(this,
                msg =>
                {
                    var window = new WarehousesWindow();
                    window.ShowDialog();
                });
            Messenger.Default.Register<OpenQuantityWindowMessage>(this,
               msg =>
               {
                   var window = new QuantityWindow();
                   if (window.DataContext is QuantityViewModel model)
                   {
                       model.MaxQuantity = msg.MaxQuantity;
                   }
                   window.ShowDialog();
               });
            Messenger.Default.Register<OpenInvoiceWindowMessage>(this,
              msg =>
              {
                  var window = new InvoiceWindow();
                  if (window.DataContext is InvoiceViewModel model)
                  {
                      model.SalesOrder = msg.SalesOrder;
                  }
                  window.ShowDialog();
              });
        }

        #endregion Methods
    }
}