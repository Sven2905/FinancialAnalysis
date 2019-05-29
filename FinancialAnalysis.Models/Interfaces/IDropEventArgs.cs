using DevExpress.Xpf.Grid;
using System.Collections;

namespace FinancialAnalysis.Models.Interfaces
{
    public interface IDropEventArgs
    {
        bool Handled { get; set; }
        IList Items { get; }
        object TargetRow { get; }
        DropTargetType DropTargetType { get; }
        DevExpress.Xpf.Grid.GridControl GridControl { get; }
    }
}
