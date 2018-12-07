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
            var db = new DataLayer();
            var newItem = (TaxType) e.Row;
            db.TaxTypes.UpdateOrInsert(newItem);
        }

        private void DeleteItem()
        {
            if (SelectedItem is null)
                return;

            var db = new DataLayer();
            db.TaxTypes.Delete(SelectedItem);
            TaxTypes.Remove(SelectedItem);
        }

        private void RefreshList()
        {
            var db = new DataLayer();
            TaxTypes = db.TaxTypes.GetAll().ToOberservableCollection();
        }
    }
}