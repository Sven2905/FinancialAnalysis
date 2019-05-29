using DevExpress.Xpf.Core;
using FinancialAnalysis.Logic.ViewModels;

namespace FinancialAnalysis.Windows
{
    /// <summary>
    ///     Interaktionslogik für WarehousesWindow.xaml
    /// </summary>
    public partial class WarehousesWindow : DXWindow
    {
        public WarehousesWindow()
        {
            InitializeComponent();
            WarehouseViewModel vm = new WarehouseViewModel();
            DataContext = vm;
            if (vm.CloseAction == null)
            {
                vm.CloseAction = () => { };
            }

            vm.CloseAction = Close;
        }
    }
}