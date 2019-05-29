using DevExpress.Xpf.Core;
using FinancialAnalysis.Logic.ViewModels;

namespace FinancialAnalysis.Windows
{
    /// <summary>
    ///     Interaktionslogik für SalesTypesWindow.xaml
    /// </summary>
    public partial class SalesTypesWindow : DXWindow
    {
        public SalesTypesWindow()
        {
            InitializeComponent();
            SalesTypesViewModel vm = new SalesTypesViewModel();
            DataContext = vm;
            if (vm.CloseAction == null)
            {
                vm.CloseAction = () => { };
            }

            vm.CloseAction = Close;
        }
    }
}