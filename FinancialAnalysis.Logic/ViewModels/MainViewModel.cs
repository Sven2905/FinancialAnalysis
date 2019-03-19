using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.Enums;
using FinancialAnalysis.Models.General;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Windows.Media;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private string _currentTime;

        /// <summary>
        ///     Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }
            SetSalesOrderStatusViewModel();
            SetDebitorStatusViewModel();
            SetupTimer();
        }

        private void SetDebitorStatusViewModel()
        {
            DebitorStatusViewModel.SetColorToBlue();
            DebitorStatusViewModel.StatusText = "Anzahl offene Mahnungen";
            DebitorStatusViewModel.StatusValue = DataContext.Instance.InvoiceReminders.GetAll().Count().ToString();
            DebitorStatusViewModel.IconWidth = 60;
            DebitorStatusViewModel.SetIconData("M544 224c44.2 0 80-35.8 80-80s-35.8-80-80-80-80 35.8-80 80 35.8 80 80 80zm0-128c26.5 0 48 21.5 48 48s-21.5 48-48 48-48-21.5-48-48 21.5-48 48-48zM320 256c61.9 0 112-50.1 112-112S381.9 32 320 32 208 82.1 208 144s50.1 112 112 112zm0-192c44.1 0 80 35.9 80 80s-35.9 80-80 80-80-35.9-80-80 35.9-80 80-80zm244 192h-40c-15.2 0-29.3 4.8-41.1 12.9 9.4 6.4 17.9 13.9 25.4 22.4 4.9-2.1 10.2-3.3 15.7-3.3h40c24.2 0 44 21.5 44 48 0 8.8 7.2 16 16 16s16-7.2 16-16c0-44.1-34.1-80-76-80zM96 224c44.2 0 80-35.8 80-80s-35.8-80-80-80-80 35.8-80 80 35.8 80 80 80zm0-128c26.5 0 48 21.5 48 48s-21.5 48-48 48-48-21.5-48-48 21.5-48 48-48zm304.1 180c-33.4 0-41.7 12-80.1 12-38.4 0-46.7-12-80.1-12-36.3 0-71.6 16.2-92.3 46.9-12.4 18.4-19.6 40.5-19.6 64.3V432c0 26.5 21.5 48 48 48h288c26.5 0 48-21.5 48-48v-44.8c0-23.8-7.2-45.9-19.6-64.3-20.7-30.7-56-46.9-92.3-46.9zM480 432c0 8.8-7.2 16-16 16H176c-8.8 0-16-7.2-16-16v-44.8c0-16.6 4.9-32.7 14.1-46.4 13.8-20.5 38.4-32.8 65.7-32.8 27.4 0 37.2 12 80.2 12s52.8-12 80.1-12c27.3 0 51.9 12.3 65.7 32.8 9.2 13.7 14.1 29.8 14.1 46.4V432zM157.1 268.9c-11.9-8.1-26-12.9-41.1-12.9H76c-41.9 0-76 35.9-76 80 0 8.8 7.2 16 16 16s16-7.2 16-16c0-26.5 19.8-48 44-48h40c5.5 0 10.8 1.2 15.7 3.3 7.5-8.5 16.1-16 25.4-22.4z");
        }

        private void SetSalesOrderStatusViewModel()
        {
            SalesOrderStatusViewModel.SetColorToOrange();
            SalesOrderStatusViewModel.StatusText = "Offene Auftr�ge";
            SalesOrderStatusViewModel.StatusValue = DataContext.Instance.SalesOrders.GetOpenedSalesOrders().Count().ToString();
        }

        public User ActiveUser => Globals.ActiveUser;
        public string CurrentTime
        {
            [DebuggerStepThrough]
            get => _currentTime;
            [DebuggerStepThrough]
            set
            {
                _currentTime = value;
                RaisePropertiesChanged();
            }
        }

        public StatusViewModel SalesOrderStatusViewModel { get; set; } = new StatusViewModel();
        public StatusViewModel DebitorStatusViewModel { get; set; } = new StatusViewModel();
        public DatabaseStatus DatabaseStatus { get; set; }

        private void SetupTimer()
        {
            Timer timer = new Timer(1000);
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CurrentTime = DateTime.Now.ToString("G");
            if (IsServerConnected())
            {
                DatabaseStatus = DatabaseStatus.Connected;
            }
            else
            {
                DatabaseStatus = DatabaseStatus.Disconnected;
            }
        }

        /// <summary>
        /// Test that the server is connected
        /// </summary>
        /// <param name="connectionString">The connection string</param>
        /// <returns>true if the connection is opened</returns>
        private static bool IsServerConnected()
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["FinancialAnalysisDB"].ConnectionString))
            {
                try
                {
                    connection.Open();
                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }

        #region UserRights

        public bool ShowAccounting => Globals.ActiveUser.IsAdministrator ||
                                      UserManager.Instance.IsUserRightGranted(Globals.ActiveUser,
                                          Permission.AccessAccounting);

        public bool ShowProjectManagement => Globals.ActiveUser.IsAdministrator ||
                                             UserManager.Instance.IsUserRightGranted(Globals.ActiveUser,
                                                 Permission.AccessProjectManagement);

        public bool ShowProducts => Globals.ActiveUser.IsAdministrator ||
                                    UserManager.Instance.IsUserRightGranted(Globals.ActiveUser,
                                        Permission.AccessProducts);

        public bool ShowWarehouseManagement => Globals.ActiveUser.IsAdministrator ||
                                               UserManager.Instance.IsUserRightGranted(Globals.ActiveUser,
                                                   Permission.AccessWarehouseManagement);

        public bool ShowConfiguration => Globals.ActiveUser.IsAdministrator ||
                                         UserManager.Instance.IsUserRightGranted(Globals.ActiveUser,
                                             Permission.AccessConfiguration);

        public bool ShowSalesManagement => Globals.ActiveUser.IsAdministrator ||
                                         UserManager.Instance.IsUserRightGranted(Globals.ActiveUser,
                                             Permission.AccessSalesManagement);
        #endregion UserRights
    }
}