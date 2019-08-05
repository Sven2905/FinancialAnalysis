using DevExpress.Xpf.Core;
using FinancialAnalysis.Logic.ViewModels;

namespace FinancialAnalysis.Windows
{
    /// <summary>
    /// Interaction logic for CreditSplitWindow.xaml
    /// </summary>
    public partial class CreditSplitWindow : DXWindow
    {
        public CreditSplitWindow()
        {
            InitializeComponent();
            CreditSplitViewModel vm = new CreditSplitViewModel();
            DataContext = vm;
            if (vm.CloseAction == null)
            {
                vm.CloseAction = () => { };
            }

            vm.CloseAction = Close;
        }
    }
}