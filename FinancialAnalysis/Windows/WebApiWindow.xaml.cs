using DevExpress.Xpf.Core;
using FinancialAnalysis.Logic.ViewModels;

namespace FinancialAnalysis.Windows
{
    /// <summary>
    /// Interaction logic for WebApiConfigurationWindow.xaml
    /// </summary>
    public partial class WebApiConfigurationWindow : DXWindow
    {
        public WebApiConfigurationWindow()
        {
            InitializeComponent();
            WebApiConfigurationViewModel vm = new WebApiConfigurationViewModel();
            DataContext = vm;
            if (vm.CloseAction == null)
            {
                vm.CloseAction = () => { };
            }

            vm.CloseAction = Close;
        }
    }
}