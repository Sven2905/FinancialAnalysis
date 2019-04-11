using DevExpress.Xpf.Core;
using FinancialAnalysis.Logic.ViewModels;

namespace FinancialAnalysis
{
    /// <summary>
    /// Interaction logic for YesNoDialogWindow.xaml
    /// </summary>
    public partial class YesNoDialogWindow : DXWindow
    {
        public YesNoDialogWindow()
        {
            InitializeComponent();
            var vm = new YesNoDialogViewModel();
            DataContext = vm;
            if (vm.CloseAction == null) vm.CloseAction = () => { };

            vm.CloseAction = Close;
        }
    }
}
