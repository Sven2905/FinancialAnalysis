using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models.Accounting;
using System.IO;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class PDFViewerViewModel : ViewModelBase
    {
        private readonly ScannedDocument _ScannedDocument;
        private int _ScannedDocumentId;
        private byte[] content;

        private void LoadDocument()
        {
            using (DataLayer db = new DataLayer())
            {
                content = db.ScannedDocuments.GetById(ScannedDocumentId).Content;
            }
            MemoryStream ms = new MemoryStream(content);
            ScannedDocument = ms;
        }

        public MemoryStream ScannedDocument { get; set; }
        public int ScannedDocumentId
        {
            get { return _ScannedDocumentId; }
            set
            {
                _ScannedDocumentId = value; if (_ScannedDocumentId != 0)
                {
                    LoadDocument();
                }
            }
        }
    }
}
