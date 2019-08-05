using DevExpress.Xpf.Core;
using FinancialAnalysis.Logic.ViewModels;

namespace FinancialAnalysis.Windows
{
    /// <summary>
    /// Interaction logic for DebitSplitWindow.xaml
    /// </summary>
    public partial class DebitSplitWindow : DXWindow
    {
        public DebitSplitWindow()
        {
            InitializeComponent();
            DebitSplitViewModel vm = new DebitSplitViewModel();
            DataContext = vm;
            if (vm.CloseAction == null)
            {
                vm.CloseAction = () => { };
            }

            vm.CloseAction = Close;
        }
    }
}