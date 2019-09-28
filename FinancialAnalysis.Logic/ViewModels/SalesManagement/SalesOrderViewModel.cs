using DevExpress.Mvvm;

using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Logic.SalesManagement;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.ProductManagement;
using FinancialAnalysis.Models.ProjectManagement;
using FinancialAnalysis.Models.SalesManagement;
using Utilities;
using WebApiWrapper.Accounting;
using WebApiWrapper.Administration;
using WebApiWrapper.ProductManagement;
using WebApiWrapper.ProjectManagement;
using WebApiWrapper.SalesManagement;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class SalesOrderViewModel : ViewModelBase
    {
        #region Fields

        private Product _SelectedProduct;

        #endregion Fields

        #region Constructor

        public SalesOrderViewModel()
        {
            Messenger.Default.Register<SelectedSalesType>(this, ChangeSelectedSalesType);

            GetData();
            Messenger.Default.Register<SelectedSalesType>(this, ChangeSelectedSalesType);
            SetCommands();
        }

        #endregion Constructor

        #region UserRights

        public bool AllowSalesTypes =>
            UserManager.Instance.IsUserRightGranted(Globals.ActiveUser, Permission.SalesTypes) ||
            Globals.ActiveUser.IsAdministrator;

        #endregion UserRights

        #region Properties

        public SalesOrder SalesOrder { get; set; } = new SalesOrder();

        public Product SelectedProduct
        {
            get => _SelectedProduct;
            set
            {
                _SelectedProduct = value;
                if (value != null)
                {
                    SalesOrderPosition.Product = _SelectedProduct;
                    SalesOrderPosition.RefProductId = _SelectedProduct.ProductId;
                    SalesOrderPosition.Price = _SelectedProduct.DefaultSellingPrice;
                }
            }
        }

        private Debitor _SelectedDebitor;

        public Debitor SelectedDebitor
        {
            get => _SelectedDebitor;
            set { _SelectedDebitor = value; SalesOrder.Debitor = _SelectedDebitor; SalesOrder.RefDebitorId = _SelectedDebitor.DebitorId; }
        }

        public User User { get; set; }
        public SalesOrderPosition SalesOrderPosition { get; set; } = new SalesOrderPosition();
        public SalesOrderPosition SelectedSalesOrderPosition { get; set; }
        public SvenTechCollection<Debitor> DebitorList { get; set; } = new SvenTechCollection<Debitor>();
        public SvenTechCollection<User> UserList { get; set; } = new SvenTechCollection<User>();
        public SvenTechCollection<Product> ProductList { get; set; } = new SvenTechCollection<Product>();
        public SvenTechCollection<SalesType> SalesTypeList { get; set; } = new SvenTechCollection<SalesType>();
        public SvenTechCollection<ShipmentType> ShipmentTypeList { get; set; } = new SvenTechCollection<ShipmentType>();

        public SvenTechCollection<SalesOrderPosition> SalesOrderPositionList { get; set; } =
            new SvenTechCollection<SalesOrderPosition>();

        public DelegateCommand NewSalesOrderCommand { get; set; }
        public DelegateCommand CreatePDFPreviewCommand { get; set; }
        public DelegateCommand SavePositionCommand { get; set; }
        public DelegateCommand DeletePositionCommand { get; set; }
        public DelegateCommand SaveSalesOrderCommand { get; set; }
        public DelegateCommand OpenSalesTypesWindowCommand { get; set; }

        #endregion Properties

        #region Methods

        private void SetCommands()
        {
            CreatePDFPreviewCommand = new DelegateCommand(() => CreatePDFFile(true), () => ValidatePDFCreation());
            SavePositionCommand = new DelegateCommand(SaveSalesOrderPosition, () => ValidateSalesOrderPosition());
            DeletePositionCommand =
                new DelegateCommand(DeleteSalesOrderPosition, () => SelectedSalesOrderPosition != null);
            SaveSalesOrderCommand = new DelegateCommand(() => { SaveSalesOrder(); CreatePDFFile(false); Clear(); }, () => ValidateSalesOrder());
            OpenSalesTypesWindowCommand = new DelegateCommand(OpenSalesTypesWindow);
        }

        private bool ValidatePDFCreation()
        {
            if (SalesOrder.SalesOrderPositions.Count < 1)
            {
                return false;
            }

            if (!ValidateSalesOrder())
            {
                return false;
            }

            return true;
        }

        private bool ValidateSalesOrderPosition()
        {
            return SalesOrderPosition.RefProductId != 0;
        }

        private bool ValidateSalesOrder()
        {
            if (SalesOrder == null)
            {
                return false;
            }

            if (SalesOrder.SalesOrderPositions.Count < 1)
            {
                return false;
            }

            if (SalesOrder.RefDebitorId == 0 || SalesOrder.RefSalesTypeId == 0)
            {
                return false;
            }

            return true;
        }

        private void OpenSalesTypesWindow()
        {
            Messenger.Default.Send(new OpenSalesTypesWindowMessage());
        }

        private void DeleteSalesOrderPosition()
        {
            if (SelectedSalesOrderPosition != null)
            {
                SalesOrder.SalesOrderPositions.Remove(SelectedSalesOrderPosition);
            }
        }

        private void SaveSalesOrderPosition()
        {
            SalesOrder.SalesOrderPositions.Add(SalesOrderPosition);
            SalesOrderPosition = new SalesOrderPosition();
            SelectedProduct = new Product();
        }

        private void CreatePDFPreview()
        {
            CreatePDFFile(true);
        }

        private void CreatePDFFile(bool IsPreview)
        {
            SalesOrderReportData salesOrderReportData = new SalesOrderReportData
            {
                MyCompany = Globals.CoreData.MyCompany,
                SalesOrder = SalesOrder,
                User = User
            };

            SalesReportPDFCreator.CreateAndShowOrderReport(salesOrderReportData, IsPreview);
        }

        private void SaveSalesOrder()
        {
            SalesOrder.RefUserId = User.UserId;
            SalesOrder.SalesOrderId = SalesOrders.Insert(SalesOrder);

            foreach (SalesOrderPosition item in SalesOrder.SalesOrderPositions)
            {
                item.RefSalesOrderId = SalesOrder.SalesOrderId;
                SalesOrderPositions.Insert(item);
            }
        }

        private void Clear()
        {
            SalesOrder = new SalesOrder();
            SelectedProduct = new Product();
            SalesOrderPositionList.Clear();
        }

        private void GetData()
        {
            DebitorList = Debitors.GetAll().ToSvenTechCollection();
            ProductList = Products.GetAll().ToSvenTechCollection();
            SalesTypeList = SalesTypes.GetAll().ToSvenTechCollection();
            UserList = Users.GetAll().ToSvenTechCollection();
            ShipmentTypeList = ShipmentTypes.GetAll().ToSvenTechCollection();
        }

        private void ChangeSelectedSalesType(SelectedSalesType SelectedSalesType)
        {
            SalesTypeList = SalesTypes.GetAll().ToSvenTechCollection();
            SalesOrder.SalesType = SelectedSalesType.SalesType;
            SalesOrder.RefSalesTypeId = SelectedSalesType.SalesType.SalesTypeId;
            RaisePropertyChanged("SalesOrder");
        }

        #endregion Methods
    }
}