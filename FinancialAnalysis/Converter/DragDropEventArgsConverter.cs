using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.DragDrop;
using FinancialAnalysis.Models.Interfaces;
using System;
using System.Collections;
using System.Windows.Markup;

namespace FinancialAnalysis.UserControls
{
    public class DragDropEventArgsConverter : MarkupExtension, IEventArgsConverter
    {
        private static readonly DragDropEventArgsConverter Instance = new DragDropEventArgsConverter();

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
        protected GridDropEventArgs InnerArgs { get; }

        public virtual bool Handled
        {
            get => InnerArgs.Handled;
            set => InnerArgs.Handled = value;
        }

        public virtual IList Items => InnerArgs.DraggedRows;

        public virtual object TargetRow => InnerArgs.TargetRow;

        public virtual DropTargetType DropTargetType => InnerArgs.DropTargetType;

        public virtual GridControl GridControl => InnerArgs.GridControl;

        public DropEventArgsWrapper(GridDropEventArgs baseArgs)
        {
            InnerArgs = baseArgs;
        }
    }
}
