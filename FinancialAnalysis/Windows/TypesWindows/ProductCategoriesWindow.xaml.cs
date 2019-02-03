using DevExpress.Xpf.Core;
using FinancialAnalysis.Logic.ViewModels;

namespace FinancialAnalysis.Windows
{
    /// <summary>
    ///     Interaktionslogik für ProductCategoriesWindow.xaml
    /// </summary>
    public partial class ProductCategoriesWindow : DXWindow
    {
        public ProductCategoriesWindow()
        {
            InitializeComponent();
            var vm = new ProductCategoryViewModel();
            DataContext = vm;
            if (vm.CloseAction == null) vm.CloseAction = () => { };

            vm.CloseAction = Close;
        }
    }
}