using System.Windows;
using DevExpress.Xpf.Core;
using WinInterop = System.Windows.Interop;

namespace FinancialAnalysis.UI.Desktop
{
    /// <summary>
    ///     Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : DXWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        //{
        //    DXSplashScreen.Close();
        //    Activate();
        //}
    }
}