using DevExpress.Mvvm;
using DevExpress.Xpf.PivotGrid;
using DevExpress.Xpf.Printing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FinancialAnalysis.Logic
{
    public class PivotExportCommand : ICommand
    {
        public PivotExportCommand() { }
        public event EventHandler CanExecuteChanged;
        public bool CanExecute(object parameter)
        {
            return true;
        }
        public void Execute(object parameter)
        {
            DocumentPreview preview = new DocumentPreview();
            PrintableControlLink link = new PrintableControlLink(parameter as IPrintableControl);
            link.Landscape = true;
            link.CreateDocument(false);
            link.PrintingSystem.Document.AutoFitToPagesWidth = 1;
            link.ExportToPdf("Pivot.pdf");


            //((PivotGridControl)parameter).ExportToPdf("test.pdf", new DevExpress.XtraPrinting.PdfExportOptions() { ImageQuality = DevExpress.XtraPrinting.PdfJpegImageQuality.Highest });
            //Process.Start("test.pdf");
        }
    }
}
