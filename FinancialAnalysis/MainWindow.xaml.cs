using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Xpf.Core;
using WinInterop = System.Windows.Interop;
using System.Runtime.InteropServices;
using FinancialAnalysis.Logic.Model.ViewModel;

namespace FinancialAnalysis.UI.Desktop
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : DXWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void DXWindow_Loaded(object sender, RoutedEventArgs e)
        {
            DXSplashScreen.Close();
            this.Activate();
        }
    }
}