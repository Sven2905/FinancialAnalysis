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

namespace FinancialAnalysis.Windows
{
    /// <summary>
    /// Interaktionslogik für WarehousesWindow.xaml
    /// </summary>
    public partial class WarehousesWindow : DXWindow
    {
        public WarehousesWindow()
        {
            InitializeComponent();
            var vm = new WarehouseViewModel();
            DataContext = vm;
            if (vm.CloseAction == null)
                vm.CloseAction = () => { };
            vm.CloseAction = Close;
        }
    }
}
