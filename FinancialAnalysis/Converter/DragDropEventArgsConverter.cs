using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.DragDrop;
using FinancialAnalysis.Models.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace FinancialAnalysis.UserControls
{
    public class DragDropEventArgsConverter : MarkupExtension, IEventArgsConverter
    {
        static DragDropEventArgsConverter Instance = new DragDropEventArgsConverter();

        public DragDropEventArgsConverter() { }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance;
        }

        public object Convert(object sender, object args)
        {
            return args is GridDropEventArgs ? new DropEventArgsWrapper((GridDropEventArgs)args) : args;
        }
    }



    public class DropEventArgsWrapper : IDropEventArgs
    {
        protected GridDropEventArgs InnerArgs { get; private set; }
        public virtual bool Handled
        {
            get { return InnerArgs.Handled; }
            set { InnerArgs.Handled = value; }
        }

        public virtual IList Items
        {
            get { return InnerArgs.DraggedRows; }
        }

        public virtual object TargetRow
        {
            get { return InnerArgs.TargetRow; }
        }

        public virtual DropTargetType DropTargetType
        {
            get { return InnerArgs.DropTargetType; }
        }

        public DropEventArgsWrapper(GridDropEventArgs baseArgs)
        {
            InnerArgs = baseArgs;
        }
    }
}
