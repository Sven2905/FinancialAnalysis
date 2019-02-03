using DevExpress.Xpf.Core;
using FinancialAnalysis.Logic.ViewModels;

namespace FinancialAnalysis
{
    /// <summary>
    ///     Interaktionslogik für Kontenrahmen.xaml
    /// </summary>
    public partial class KontenrahmenWindow : DXWindow
    {
        public KontenrahmenWindow()
        {
            InitializeComponent();
            var vm = new KontenrahmenViewModel();
            DataContext = vm;
            if (vm.CloseAction == null) vm.CloseAction = () => { };

            vm.CloseAction = Close;
        }
    }
}