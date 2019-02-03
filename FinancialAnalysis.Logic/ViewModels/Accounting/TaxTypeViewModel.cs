using System.Collections.ObjectModel;
using System.Windows.Input;
using DevExpress.Mvvm;
using DevExpress.Xpf.Grid;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models.Accounting;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class TaxTypeViewModel : ViewModelBase
    {
        public TaxTypeViewModel()
        {
            if (IsInDesignMode)
                return;

            RowUpdatedCommand = new DelegateCommand<RowEventArgs>(AddNewItem);
            DeleteFocusedRowCommand = new DelegateCommand(DeleteItem);
            RefreshCommand = new DelegateCommand(RefreshList);
            RefreshList();
        }

        public TaxType SelectedItem { get; set; }
        public ObservableCollection<TaxType> TaxTypes { get; set; }
        public ICommand RowUpdatedCommand { get; }
        public ICommand DeleteFocusedRowCommand { get; }
        public ICommand RefreshCommand { get; }

        private void AddNewItem(RowEventArgs e)
        {
            var newItem = (TaxType) e.Row;
            DataContext.Instance.TaxTypes.UpdateOrInsert(newItem);
        }

        private void DeleteItem()
        {
            if (SelectedItem is null)
                return;

            DataContext.Instance.TaxTypes.Delete(SelectedItem);
            TaxTypes.Remove(SelectedItem);
        }

        private void RefreshList()
        {
            TaxTypes = DataContext.Instance.TaxTypes.GetAll().ToOberservableCollection();
        }
    }
}