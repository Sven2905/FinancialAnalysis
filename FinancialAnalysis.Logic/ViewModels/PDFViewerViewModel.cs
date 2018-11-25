using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models.Accounting;
using System;
using System.IO;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class PDFViewerViewModel : ViewModelBase
    {
        private readonly ScannedDocument _ScannedDocument;
        private int _ScannedDocumentId;
        private string _Path;
        private byte[] content;

        private void LoadDocumentById()
        {
            using (DataLayer db = new DataLayer())
            {
                content = db.ScannedDocuments.GetById(ScannedDocumentId).Content;
            }
            MemoryStream ms = new MemoryStream(content);
            ScannedDocument = ms;
        }

        private void LoadDocumentByPath()
        {
            content = File.ReadAllBytes(_Path);
            MemoryStream ms = new MemoryStream(content);
            ScannedDocument = ms;
        }

        #region Properties

        public MemoryStream ScannedDocument { get; set; }

        public int ScannedDocumentId
        {
            get { return _ScannedDocumentId; }
            set
            {
                _ScannedDocumentId = value; if (_ScannedDocumentId != 0)
                {
                    LoadDocumentById();
                }
            }
        }

        public string Path
        {
            get { return _Path; }
            set
            {
                _Path = value; if (!string.IsNullOrEmpty(_Path))
                {
                    LoadDocumentByPath();
                }
            }
        }

        #endregion Properties
    }
}
