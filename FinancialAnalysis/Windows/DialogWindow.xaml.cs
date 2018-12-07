using System.Windows;
using FinancialAnalysis.Logic.ViewModels;

namespace FinancialAnalysis
{
    /// <summary>
    ///     Interaktionslogik für DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        public DialogWindow()
        {
            InitializeComponent();
            var vm = new DialogViewModel();
            DataContext = vm;
            if (vm.CloseAction == null)
                vm.CloseAction = () => { };
            vm.CloseAction = Close;
        }
    }
}