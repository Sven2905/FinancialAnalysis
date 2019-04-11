using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic
{
    public class YesNoDialogResult
    {
        public YesNoDialogResult()
        {

        }

        public YesNoDialogResult(bool DialogResult)
        {
            this.Result = DialogResult;
        }

        public bool Result { get; set; }
    }
}
