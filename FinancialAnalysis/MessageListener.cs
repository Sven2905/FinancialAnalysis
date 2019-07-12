using DevExpress.Mvvm;
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
                    KontenrahmenWindow window = new KontenrahmenWindow();
                    if (window.DataContext is KontenrahmenViewModel model)
                    {
                        model.AccountingType = msg.AccountingType;
                    }

                    window.ShowDialog();
                });

            Messenger.Default.Register<OpenDialogWindowMessage>(this,
                msg =>
                {
                    DialogWindow window = new DialogWindow();
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
                    CompanyWindow window = new CompanyWindow();
                    window.ShowDialog();
                });
            Messenger.Default.Register<OpenPDFViewerWindowMessage>(this,
                msg =>
                {
                    PDFViewerWindow window = new PDFViewerWindow();
                    if (window.DataContext is PDFViewerViewModel model)
                    {
                        if (msg.ScannedDocumentId != 0)
                        {
                            model.ScannedDocumentId = msg.ScannedDocumentId;
                        }
                        else if (!string.IsNullOrEmpty(msg.Path))
                        {
                            model.Path = msg.Path;
                        }
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
                    MainWindow window = new MainWindow();
                    MainViewModel model = window.DataContext as MainViewModel;
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
                    ProductCategoriesWindow window = new ProductCategoriesWindow();
                    window.ShowDialog();
                });
            Messenger.Default.Register<OpenCostCenterCategoriesWindowMessage>(this,
                msg =>
                {
                    CostCenterCategoriesWindow window = new CostCenterCategoriesWindow();
                    window.ShowDialog();
                });
            Messenger.Default.Register<OpenSalesTypesWindowMessage>(this,
                msg =>
                {
                    SalesTypesWindow window = new SalesTypesWindow();
                    window.ShowDialog();
                });
            Messenger.Default.Register<OpenInvoiceTypesWindowMessage>(this,
                msg =>
                {
                    InvoiceTypesWindow window = new InvoiceTypesWindow();
                    window.ShowDialog();
                });
            Messenger.Default.Register<OpenWarehousesWindowMessage>(this,
                msg =>
                {
                    WarehousesWindow window = new WarehousesWindow();
                    window.ShowDialog();
                });
            Messenger.Default.Register<OpenQuantityWindowMessage>(this,
               msg =>
               {
                   QuantityWindow window = new QuantityWindow();
                   if (window.DataContext is QuantityViewModel model)
                   {
                       model.MaxQuantity = msg.MaxQuantity;
                   }
                   window.ShowDialog();
               });
            Messenger.Default.Register<OpenInvoiceCreationWindowMessage>(this,
              msg =>
              {
                  InvoiceCreationWindow window = new InvoiceCreationWindow();
                  if (window.DataContext is InvoiceCreationViewModel model)
                  {
                      model.SalesOrder = msg.SalesOrder;
                  }
                  window.ShowDialog();
              });
            Messenger.Default.Register<OpenInvoiceListWindowMessage>(this,
              msg =>
              {
                  InvoiceListWindow window = new InvoiceListWindow();
                  if (window.DataContext is InvoiceListViewModel model)
                  {
                      model.InvoiceList = msg.Invoices;
                  }
                  window.ShowDialog();
              });
            Messenger.Default.Register<OpenDatabaseConfigurationWindow>(this,
                msg =>
                {
                    DatabaseConfigurationWindow window = new DatabaseConfigurationWindow();
                    window.ShowDialog();
                });
            Messenger.Default.Register<OpenWebApiConfigurationWindow>(this,
                msg =>
                {
                    WebApiConfigurationWindow window = new WebApiConfigurationWindow();
                    window.ShowDialog();
                });
            Messenger.Default.Register<OpenYesNoDialogWindowMessage>(this,
                msg =>
                {
                    YesNoDialogWindow window = new YesNoDialogWindow();
                    if (window.DataContext is YesNoDialogViewModel model)
                    {
                        model.Title = msg.Title;
                        model.Message = msg.Message;
                    }
                    window.ShowDialog();
                });
            Messenger.Default.Register<OpenTimeBookingWindowMessage>(this,
                msg =>
                {
                    TimeBookingWindow window = new TimeBookingWindow();
                    if (window.DataContext is TimeBookingViewModel model)
                    {
                        model.RefEmployeeId = msg.RefEmployeeId;
                    }
                    window.ShowDialog();
                });
            Messenger.Default.Register<OpenCreditSplitWindowMessage>(this,
                msg =>
                {
                    CreditSplitWindow window = new CreditSplitWindow();
                    if (window.DataContext is CreditSplitViewModel model)
                    {
                        model.TotalAmount = msg.TotalAmount;
                        model.BookingType = msg.BookingType;
                    }
                    window.ShowDialog();
                });
            Messenger.Default.Register<OpenDebitSplitWindowMessage>(this,
               msg =>
               {
                   DebitSplitWindow window = new DebitSplitWindow();
                   if (window.DataContext is DebitSplitViewModel model)
                   {
                       model.TotalAmount = msg.TotalAmount;
                       model.BookingType = msg.BookingType;
                   }
                   window.ShowDialog();
               });
        }

        #endregion Methods
    }
}