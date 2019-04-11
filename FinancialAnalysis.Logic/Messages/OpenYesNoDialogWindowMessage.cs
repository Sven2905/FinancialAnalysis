using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.Messages
{
    public class OpenYesNoDialogWindowMessage
    {
        public OpenYesNoDialogWindowMessage()
        {

        }

        public OpenYesNoDialogWindowMessage(string Title, string Message)
        {
            this.Title = Title;
            this.Message = Message;
        }

        public string Title { get; set; }
        public string Message { get; set; }
    }
}
