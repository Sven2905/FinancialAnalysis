using DevExpress.Xpf.Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Interfaces
{
    public interface IDropEventArgs
    {
        bool Handled { get; set; }
        IList Items { get; }
        object TargetRow { get; }
        DropTargetType DropTargetType { get; }
    }
}
