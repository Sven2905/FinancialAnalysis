using System.IO;
using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models.Accounting;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class PDFViewerViewModel : ViewModelBase
    {
        public PDFViewerViewModel()
        {
            if (IsInDesignMode)
                return;
        }

        private readonly ScannedDocument _ScannedDocument;
        private string _Path;
        private int _ScannedDocumentId;
        private byte[] content;

        private void LoadDocumentById()
        {
                content = DataLayer.Instance.ScannedDocuments.GetById(ScannedDocumentId).Content;

            var ms = new MemoryStream(content);
            ScannedDocument = ms;
        }

        private void LoadDocumentByPath()
        {
            content = File.ReadAllBytes(_Path);
            var ms = new MemoryStream(content);
            ScannedDocument = ms;
        }

        #region Properties

        public MemoryStream ScannedDocument { get; set; }

        public int ScannedDocumentId
        {
            get => _ScannedDocumentId;
            set
            {
                _ScannedDocumentId = value;
                if (_ScannedDocumentId != 0) LoadDocumentById();
            }
        }

        public string Path
        {
            get => _Path;
            set
            {
                _Path = value;
                if (!string.IsNullOrEmpty(_Path)) LoadDocumentByPath();
            }
        }

        #endregion Properties
    }
}