using DevExpress.Xpf.Core;
using FinancialAnalysis.Logic.ViewModels;

namespace FinancialAnalysis.Windows
{
    /// <summary>
    ///     Interaktionslogik für CostCenterCategoriesWindow.xaml
    /// </summary>
    public partial class CostCenterCategoriesWindow : DXWindow
    {
        public CostCenterCategoriesWindow()
        {
            InitializeComponent();
            var vm = new CostCenterCategoryViewModel();
            DataContext = vm;
            if (vm.CloseAction == null) vm.CloseAction = () => { };

            vm.CloseAction = Close;
        }
    }
}