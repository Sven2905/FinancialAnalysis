using System.IO;
using DevExpress.Mvvm;

using FinancialAnalysis.Models.Accounting;
using WebApiWrapper.Accounting;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class PDFViewerViewModel : ViewModelBase
    {
        private readonly ScannedDocument _ScannedDocument;
        private string _Path;
        private int _ScannedDocumentId;
        private byte[] content;
        private MemoryStream _MemoryStream;

        public PDFViewerViewModel()
        {
            if (IsInDesignMode) return;
        }

        private void LoadDocumentById()
        {
            content = ScannedDocuments.GetById(ScannedDocumentId).Content;

            var ms = new MemoryStream(content);
            ScannedDocument = ms;
        }

        private void LoadDocumentByPath()
        {
            content = File.ReadAllBytes(_Path);
            var ms = new MemoryStream(content);
            ScannedDocument = ms;
        }

        private void LoadDocumentByMemoryStream(MemoryStream ms)
        {
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

        public MemoryStream MemoryStream
        {
            get => _MemoryStream;
            set
            {
                _MemoryStream = value;
                LoadDocumentByMemoryStream(MemoryStream);
            }
        }

        #endregion Properties
    }
}