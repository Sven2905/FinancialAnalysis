using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FinancialAnalysis.Logic.Messages
{
    public class OpenDialogWindowMessage
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public MessageBoxImage MessageBoxImage { get; set; }
    }
}
