using DevExpress.Mvvm;
using DevExpress.Xpf.Grid;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models.Accounting;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Utilities;
using System.Collections.Generic;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class TaxTypeViewModel : ViewModelBase
    {
        public TaxType SelectedItem { get; set; }
        public ObservableCollection<TaxType> TaxTypes { get; set; }
        public ICommand RowUpdatedCommand { get; private set; }
        public ICommand DeleteFocusedRowCommand { get; private set; }
        public ICommand RefreshCommand { get; private set; }

        public TaxTypeViewModel()
        {
            RowUpdatedCommand = new DelegateCommand<RowEventArgs>(AddNewItem);
            DeleteFocusedRowCommand = new DelegateCommand(DeleteItem);
            RefreshCommand = new DelegateCommand(RefreshList);
            RefreshList();
        }

        private void AddNewItem(RowEventArgs e)
        {
            DataLayer db = new DataLayer();
            var newItem = (TaxType)e.Row;
            db.TaxTypes.UpdateOrInsert(newItem);
        }

        private void DeleteItem()
        {
            if (SelectedItem is null)
                return;

            DataLayer db = new DataLayer();
            db.TaxTypes.Delete(SelectedItem);
            TaxTypes.Remove(SelectedItem);
        }

        private void RefreshList()
        {
            DataLayer db = new DataLayer();
            TaxTypes = db.TaxTypes.GetAll().ToOberservableCollection();
        }
    }
}
