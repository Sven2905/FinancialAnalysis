using DevExpress.Mvvm;
using DevExpress.Xpf.Grid;

using FinancialAnalysis.Models.Accounting;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Utilities;
using WebApiWrapper.Accounting;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class TaxTypeViewModel : ViewModelBase
    {
        public TaxTypeViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }

            RowUpdatedCommand = new DelegateCommand<RowEventArgs>(AddNewItem);
            DeleteFocusedRowCommand = new DelegateCommand(DeleteItem);
            RefreshCommand = new DelegateCommand(RefreshList);
            RefreshList();
        }

        public TaxType SelectedItem { get; set; }
        public ObservableCollection<TaxType> TaxTypeList { get; set; }
        public ICommand RowUpdatedCommand { get; }
        public ICommand DeleteFocusedRowCommand { get; }
        public ICommand RefreshCommand { get; }

        private void AddNewItem(RowEventArgs e)
        {
            TaxType newItem = (TaxType)e.Row;
            if (newItem.TaxTypeId == 0)
            {
                TaxTypes.Insert(newItem);
            }
            else
            {
                TaxTypes.Update(newItem);
            }
        }

        private void DeleteItem()
        {
            if (SelectedItem is null)
            {
                return;
            }

            TaxTypes.Delete(SelectedItem.TaxTypeId);
            TaxTypeList.Remove(SelectedItem);
        }

        private void RefreshList()
        {
            TaxTypeList = TaxTypes.GetAll().ToOberservableCollection();
        }
    }
}