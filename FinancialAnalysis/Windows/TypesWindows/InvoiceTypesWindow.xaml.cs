using DevExpress.Xpf.Core;
using FinancialAnalysis.Logic.ViewModels;

namespace FinancialAnalysis.Windows
{
    /// <summary>
    ///     Interaktionslogik für InvoiceTypesWindow.xaml
    /// </summary>
    public partial class InvoiceTypesWindow : DXWindow
    {
        public InvoiceTypesWindow()
        {
            InitializeComponent();
            InvoiceTypeViewModel vm = new InvoiceTypeViewModel();
            DataContext = vm;
            if (vm.CloseAction == null)
            {
                vm.CloseAction = () => { };
            }

            vm.CloseAction = Close;
        }
    }
}