using DevExpress.Mvvm;
using FinancialAnalysis.Logic.Manager;
using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.Enums;
using FinancialAnalysis.Models.General;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using WebApiWrapper.SalesManagement;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        //       _
        //   .__(.)< (MEOW)
        //    \___)
        //  ~~~~~~~~~~~~~~~~~~

        private string _currentTime;
        private string _Message;
        private const int showMessageInterval = 5;
        private Timer progressTimer = new Timer();

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }
            LoadLicense();
            SetSalesOrderStatusViewModel();
            SetDebitorStatusViewModel();
            SetupTimer();
            Progress = 0;
            Message = "";
        }

        private void LoadLicense()
        {
            LicenseManager.Instance.Initialize();
        }

        private void SetDebitorStatusViewModel()
        {
            DebitorStatusViewModel.SetColorToBlue();
            DebitorStatusViewModel.StatusText = "Anzahl offene Mahnungen";
            DebitorStatusViewModel.StatusValue = InvoiceReminders.GetAll().Count().ToString();
            DebitorStatusViewModel.IconWidth = 60;
            DebitorStatusViewModel.SetIconData("M544 224c44.2 0 80-35.8 80-80s-35.8-80-80-80-80 35.8-80 80 35.8 80 80 80zm0-128c26.5 0 48 21.5 48 48s-21.5 48-48 48-48-21.5-48-48 21.5-48 48-48zM320 256c61.9 0 112-50.1 112-112S381.9 32 320 32 208 82.1 208 144s50.1 112 112 112zm0-192c44.1 0 80 35.9 80 80s-35.9 80-80 80-80-35.9-80-80 35.9-80 80-80zm244 192h-40c-15.2 0-29.3 4.8-41.1 12.9 9.4 6.4 17.9 13.9 25.4 22.4 4.9-2.1 10.2-3.3 15.7-3.3h40c24.2 0 44 21.5 44 48 0 8.8 7.2 16 16 16s16-7.2 16-16c0-44.1-34.1-80-76-80zM96 224c44.2 0 80-35.8 80-80s-35.8-80-80-80-80 35.8-80 80 35.8 80 80 80zm0-128c26.5 0 48 21.5 48 48s-21.5 48-48 48-48-21.5-48-48 21.5-48 48-48zm304.1 180c-33.4 0-41.7 12-80.1 12-38.4 0-46.7-12-80.1-12-36.3 0-71.6 16.2-92.3 46.9-12.4 18.4-19.6 40.5-19.6 64.3V432c0 26.5 21.5 48 48 48h288c26.5 0 48-21.5 48-48v-44.8c0-23.8-7.2-45.9-19.6-64.3-20.7-30.7-56-46.9-92.3-46.9zM480 432c0 8.8-7.2 16-16 16H176c-8.8 0-16-7.2-16-16v-44.8c0-16.6 4.9-32.7 14.1-46.4 13.8-20.5 38.4-32.8 65.7-32.8 27.4 0 37.2 12 80.2 12s52.8-12 80.1-12c27.3 0 51.9 12.3 65.7 32.8 9.2 13.7 14.1 29.8 14.1 46.4V432zM157.1 268.9c-11.9-8.1-26-12.9-41.1-12.9H76c-41.9 0-76 35.9-76 80 0 8.8 7.2 16 16 16s16-7.2 16-16c0-26.5 19.8-48 44-48h40c5.5 0 10.8 1.2 15.7 3.3 7.5-8.5 16.1-16 25.4-22.4z");
        }

        private void SetSalesOrderStatusViewModel()
        {
            SalesOrderStatusViewModel.SetColorToOrange();
            SalesOrderStatusViewModel.StatusText = "Offene Aufträge";
            SalesOrderStatusViewModel.StatusValue = SalesOrders.GetOpenedSalesOrders().Count().ToString();
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

        public TimeFastBookingViewModel TimeFastBookingViewModel { get; set; } = new TimeFastBookingViewModel();
        public StatusViewModel SalesOrderStatusViewModel { get; set; } = new StatusViewModel();
        public StatusViewModel DebitorStatusViewModel { get; set; } = new StatusViewModel();
        public DatabaseStatus DatabaseStatus { get; set; }
        public IconItem DatabaseStatusItem { get; set; } = new IconItem();
        public double Progress { get; set; }

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
                DatabaseStatusItem.Height = 14;
                DatabaseStatusItem.Width = 16;
                DatabaseStatusItem.SetIconData("M320 368c17.64 0 32 14.36 32 32s-14.36 32-32 32-32-14.36-32-32 14.35-32 32-32m0-48c-44.18 0-80 35.82-80 80s35.82 80 80 80 80-35.82 80-80-35.82-80-80-80zm204.69-31.83c4.62-4.87 4.38-12.64-.59-17.15-115.74-105.32-292.41-105.38-408.22 0-4.96 4.51-5.2 12.28-.59 17.15l16.47 17.37c4.46 4.71 11.81 4.95 16.62.6 97.44-88.13 245.68-88.21 343.22 0 4.81 4.35 12.16 4.1 16.62-.6l16.47-17.37zm111.42-133.98C457.86-8.86 181.84-8.59 3.89 154.19c-4.94 4.52-5.22 12.14-.57 16.95l16.6 17.18c4.52 4.68 12.01 4.93 16.81.54 159.59-145.79 406.82-145.91 566.54 0 4.81 4.39 12.29 4.13 16.81-.54l16.6-17.18c4.65-4.81 4.37-12.44-.57-16.95z");
            }
            else
            {
                DatabaseStatus = DatabaseStatus.Disconnected;
                DatabaseStatusItem.Height = 14;
                DatabaseStatusItem.Width = 10;
                DatabaseStatusItem.SetIconData("M186.071 48l-38.666 144H272L120 464l54.675-208H48L67.72 48h118.351m0-48H67.72C42.965 0 22.271 18.825 19.934 43.469l-19.716 208C-2.453 279.642 19.729 304 48.004 304h64.423l-38.85 147.79C65.531 482.398 88.788 512 119.983 512c16.943 0 33.209-9.005 41.919-24.592l151.945-271.993C331.704 183.461 308.555 144 271.945 144h-61.951l22.435-83.552C240.598 30.026 217.678 0 186.071 0z");
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

        /// <summary>
        /// [SUMMARY]
        /// </summary>
        /// <param name="progress">Object of class 'double' containing progress data</param>
        /// <param name="message">Object of class 'string' containing message data</param>
        private void ProgressChanged(double progress, string message)
        {
            if ((message == null) && (progress == 0))
                Progress++;
            else if ((message == null) && (progress > 0))
                Progress = progress;
            else
            {
                Progress = progress;
                Message = message;
            }
        }

        public string Message
        {
            get { return _Message; }
            set
            {
                _Message = value; RaisePropertyChanged("Message");
                SetAndStartTimer(showMessageInterval);
            }
        }

        private void SetAndStartTimer(int intervalInSeconds)
        {
            if (progressTimer.Enabled)
            {
                progressTimer.Stop();
                progressTimer.Interval = intervalInSeconds * 1000;
                progressTimer.Start();
            }
            else
            {
                progressTimer = new Timer();
                progressTimer.Elapsed += progressTimer_Elapsed;
                progressTimer.AutoReset = false;
                progressTimer.Interval = intervalInSeconds * 1000;
                progressTimer.Start();
            }
        }

        private void progressTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Message = string.Empty;
            progressTimer.Stop();
        }

        #region UserRights

        public bool ShowAccounting => Globals.ActiveUser.IsAdministrator ||
                                      UserManager.Instance.IsUserRightGranted(Globals.ActiveUser,
                                          Permission.Accounting);

        public bool ShowProjectManagement => Globals.ActiveUser.IsAdministrator ||
                                             UserManager.Instance.IsUserRightGranted(Globals.ActiveUser,
                                                 Permission.ProjectManagement);

        public bool ShowProducts => Globals.ActiveUser.IsAdministrator ||
                                    UserManager.Instance.IsUserRightGranted(Globals.ActiveUser,
                                        Permission.Products);

        public bool ShowWarehouseManagement => Globals.ActiveUser.IsAdministrator ||
                                               UserManager.Instance.IsUserRightGranted(Globals.ActiveUser,
                                                   Permission.WarehouseManagement);

        public bool ShowConfiguration => Globals.ActiveUser.IsAdministrator ||
                                         UserManager.Instance.IsUserRightGranted(Globals.ActiveUser,
                                             Permission.Configuration);

        public bool ShowSalesManagement => Globals.ActiveUser.IsAdministrator ||
                                         UserManager.Instance.IsUserRightGranted(Globals.ActiveUser,
                                             Permission.SalesManagement);

        public bool ShowTimeManagement => Globals.ActiveUser.IsAdministrator ||
                                         UserManager.Instance.IsUserRightGranted(Globals.ActiveUser,
                                             Permission.TimeManagement);

        #endregion UserRights
    }
}