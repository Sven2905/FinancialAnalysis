using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Logic.Messages
{
    public class OpenPDFViewerWindowMessage
    {
        public OpenPDFViewerWindowMessage(int ScannedDocumentId = 0)
        {
            this.ScannedDocumentId = ScannedDocumentId;
        }

        public OpenPDFViewerWindowMessage(string Path)
        {
            this.Path = Path;
        }

        public string Path { get; set; }
        public int ScannedDocumentId { get; set; }
    }
}
