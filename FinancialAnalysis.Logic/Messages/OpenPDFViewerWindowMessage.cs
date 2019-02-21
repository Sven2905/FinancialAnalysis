using System.IO;

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

        public OpenPDFViewerWindowMessage(MemoryStream MemoryStream)
        {
            this.MemoryStream = MemoryStream;
        }

        public string Path { get; set; }
        public MemoryStream MemoryStream { get; set; }
        public int ScannedDocumentId { get; set; }
    }
}