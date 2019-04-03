using System.Windows;
using DevExpress.Xpf.Core;
using FinancialAnalysis.Logic.ViewModels;

namespace FinancialAnalysis
{
    /// <summary>
    ///     Interaktionslogik für DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : DXWindow
    {
        public DialogWindow()
        {
            InitializeComponent();
            var vm = new DialogViewModel();
            DataContext = vm;
            if (vm.CloseAction == null) vm.CloseAction = () => { };

            vm.CloseAction = Close;
        }
    }
}