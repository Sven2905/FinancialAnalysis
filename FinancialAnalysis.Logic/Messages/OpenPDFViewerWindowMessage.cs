using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.Messages
{
    public class OpenPDFViewerWindowMessage
    {
        public OpenPDFViewerWindowMessage(int ScannedDocumentId)
        {
            this.ScannedDocumentId = ScannedDocumentId;
        }

        public int ScannedDocumentId { get; set; }
    }
}
