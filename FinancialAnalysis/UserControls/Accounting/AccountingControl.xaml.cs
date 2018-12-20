using System;
using System.Windows.Controls;

namespace FinancialAnalysis.UserControls
{
    /// <summary>
    ///     Interaktionslogik für AccountingControl.xaml
    /// </summary>
    public partial class AccountingControl : UserControl
    {
        public AccountingControl()
        {
            InitializeComponent();

            btnCreditorsDebitors.Content = "Kreditoren" + Environment.NewLine + "Debitoren";
        }
    }
}