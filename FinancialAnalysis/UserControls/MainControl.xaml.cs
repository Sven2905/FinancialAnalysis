using System;
using System.Windows.Controls;

namespace FinancialAnalysis.UserControls
{
    /// <summary>
    ///     Interaktionslogik für MainControl.xaml
    /// </summary>
    public partial class MainControl : UserControl
    {
        public MainControl()
        {
            InitializeComponent();

            btnCreditorsDebitors.Content = "Kreditoren" + Environment.NewLine + "Debitoren";
        }
    }
}