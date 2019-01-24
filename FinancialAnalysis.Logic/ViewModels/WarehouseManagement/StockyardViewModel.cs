using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.WarehouseManagement;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class StockyardViewModel : ViewModelBase
    {
        #region Fields

        private Stockyard _SelectedStockyard;
        private SvenTechCollection<Stockyard> _Stockyards = new SvenTechCollection<Stockyard>();
        private string _FilterText;

        #endregion Fields

        #region Constructor

        public StockyardViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }

            _Stockyards = LoadAllStockyards();
            NewStockyardCommand = new DelegateCommand(NewStockyard);
            SaveStockyardCommand = new DelegateCommand(SaveStockyard, () => Validation());
            DeleteStockyardCommand = new DelegateCommand(DeleteStockyard, () => (SelectedStockyard != null));
        }

        #endregion Constructor

        #region Methods

        private SvenTechCollection<Stockyard> LoadAllStockyards()
        {
            SvenTechCollection<Stockyard> allStockyards = new SvenTechCollection<Stockyard>();
            try
            {
                using (var db = new DataLayer())
                {
                    allStockyards = db.Stockyards.GetAll().ToSvenTechCollection();
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }

            return allStockyards;
        }

        private void NewStockyard()
        {
            SelectedStockyard = new Stockyard();
            _Stockyards.Add(SelectedStockyard);
        }

        private void DeleteStockyard()
        {
            if (SelectedStockyard == null)
            {
                return;
            }

            if (SelectedStockyard.StockyardId == 0)
            {
                _Stockyards.Remove(SelectedStockyard);
                SelectedStockyard = null;
                return;
            }

            try
            {
                using (var db = new DataLayer())
                {
                    db.Stockyards.Delete(SelectedStockyard.StockyardId);
                    _Stockyards.Remove(SelectedStockyard);
                    SelectedStockyard = null;
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void SaveStockyard()
        {
            try
            {
                if (SelectedStockyard.StockyardId != 0)
                {
                    using (var db = new DataLayer())
                    {
                        db.Stockyards.Update(SelectedStockyard);
                    }
                }
                else
                {
                    using (var db = new DataLayer())
                    {
                        db.Stockyards.Insert(SelectedStockyard);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private bool Validation()
        {
            if (SelectedStockyard == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(SelectedStockyard.Name))
            {
                return false;
            }
            return true;
        }

        #endregion Methods

        #region Properties

        public SvenTechCollection<Stockyard> FilteredStockyards { get; set; } = new SvenTechCollection<Stockyard>();
        public DelegateCommand NewStockyardCommand { get; set; }
        public DelegateCommand SaveStockyardCommand { get; set; }
        public DelegateCommand DeleteStockyardCommand { get; set; }

        public string FilterText
        {
            get { return _FilterText; }
            set
            {
                _FilterText = value;
                if (!string.IsNullOrEmpty(_FilterText))
                {
                    FilteredStockyards = new SvenTechCollection<Stockyard>();
                    foreach (var item in _Stockyards)
                    {
                        if (item.Name.Contains(FilterText))
                        {
                            FilteredStockyards.Add(item);
                        }
                    }
                }
                else
                {
                    FilteredStockyards = _Stockyards;
                }
            }
        }

        public Stockyard SelectedStockyard
        {
            get { return _SelectedStockyard; }
            set { _SelectedStockyard = value; }
        }

        public User ActualUser { get { return Globals.ActualUser; } }

        #endregion Properties
    }
}
