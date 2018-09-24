using DevExpress.Xpf.Core;
using FinancialAnalysis.Logic.ViewModels;
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
using System.Windows.Shapes;

namespace FinancialAnalysis
{
    /// <summary>
    /// Interaktionslogik für Kontenrahmen.xaml
    /// </summary>
    public partial class KontenrahmenWindow : DXWindow
    {
        public KontenrahmenWindow()
        {
            InitializeComponent();
            KontenrahmenViewModel vm = new KontenrahmenViewModel();
            this.DataContext = vm;
            if (vm.CloseAction == null)
                vm.CloseAction = new Action(() => { });
            vm.CloseAction = new Action(this.Close);
        }
    }
}
