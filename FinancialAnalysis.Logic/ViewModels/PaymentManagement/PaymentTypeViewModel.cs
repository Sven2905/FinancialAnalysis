using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.PaymentManagement;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class PaymentTypeViewModel : ViewModelBase
    {
        #region Fields

        private readonly PaymentType _SelectedPaymentType;
        private SvenTechCollection<PaymentType> _PaymentTypes = new SvenTechCollection<PaymentType>();
        private string _FilterText;

        #endregion Fields

        #region Constructor

        public PaymentTypeViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }

            _PaymentTypes = LoadAllPaymentTypes();
            NewPaymentTypeCommand = new DelegateCommand(NewPaymentType);
            SavePaymentTypeCommand = new DelegateCommand(SavePaymentType, () => Validation());
            DeletePaymentTypeCommand = new DelegateCommand(DeletePaymentType, () => (SelectedPaymentType != null));
        }

        #endregion Constructor

        #region Methods

        private SvenTechCollection<PaymentType> LoadAllPaymentTypes()
        {
            SvenTechCollection<PaymentType> allPaymentTypes = new SvenTechCollection<PaymentType>();
            try
            {
                using (var db = new DataLayer())
                {
                    allPaymentTypes = db.PaymentTypes.GetAll().ToSvenTechCollection();
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }

            return allPaymentTypes;
        }

        private void NewPaymentType()
        {
            SelectedPaymentType = new PaymentType();
            _PaymentTypes.Add(SelectedPaymentType);
        }

        private void DeletePaymentType()
        {
            if (SelectedPaymentType == null)
            {
                return;
            }

            if (SelectedPaymentType.PaymentTypeId == 0)
            {
                _PaymentTypes.Remove(SelectedPaymentType);
                SelectedPaymentType = null;
                return;
            }

            try
            {
                using (var db = new DataLayer())
                {
                    db.PaymentTypes.Delete(SelectedPaymentType.PaymentTypeId);
                    _PaymentTypes.Remove(SelectedPaymentType);
                    SelectedPaymentType = null;
                }
            }
            catch (System.Exception ex)
            {
                Messenger.Default.Send(new OpenDialogWindowMessage("Error", ex.Message, System.Windows.MessageBoxImage.Error));
            }
        }

        private void SavePaymentType()
        {
            try
            {
                if (SelectedPaymentType.PaymentTypeId != 0)
                {
                    using (var db = new DataLayer())
                    {
                        db.PaymentTypes.Update(SelectedPaymentType);
                    }
                }
                else
                {
                    using (var db = new DataLayer())
                    {
                        db.PaymentTypes.Insert(SelectedPaymentType);
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
            if (SelectedPaymentType == null)
            {
                return false;
            }
            if (string.IsNullOrEmpty(SelectedPaymentType.Name))
            {
                return false;
            }
            return true;
        }

        #endregion Methods

        #region Properties

        public SvenTechCollection<PaymentType> FilteredPaymentTypes { get; set; } = new SvenTechCollection<PaymentType>();
        public DelegateCommand NewPaymentTypeCommand { get; set; }
        public DelegateCommand SavePaymentTypeCommand { get; set; }
        public DelegateCommand DeletePaymentTypeCommand { get; set; }
        public string FilterText
        {
            get { return _FilterText; }
            set
            {
                _FilterText = value;
                if (!string.IsNullOrEmpty(_FilterText))
                {
                    FilteredPaymentTypes = new SvenTechCollection<PaymentType>();
                    foreach (var item in _PaymentTypes)
                    {
                        if (item.Name.Contains(FilterText))
                        {
                            FilteredPaymentTypes.Add(item);
                        }
                    }
                }
                else
                {
                    FilteredPaymentTypes = _PaymentTypes;
                }
            }
        }
        public PaymentType SelectedPaymentType { get; set; }

        public User ActualUser { get { return Globals.ActualUser; } }

        #endregion Properties
    }
}
