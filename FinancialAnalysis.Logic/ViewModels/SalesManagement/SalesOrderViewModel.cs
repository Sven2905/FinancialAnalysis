using System.Collections.Generic;
using System.Threading.Tasks;
using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.Administration;
using FinancialAnalysis.Models.ProductManagement;
using FinancialAnalysis.Models.ProjectManagement;
using FinancialAnalysis.Models.Reports;
using FinancialAnalysis.Models.SalesManagement;
using Utilities;

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

            Task.Run(() => GetData());
            Messenger.Default.Register<SelectedSalesType>(this, ChangeSelectedSalesType);
            SetCommands();
        }

        #endregion Constructor

        #region UserRights

        public bool AllowSalesTypes =>
            UserManager.Instance.IsUserRightGranted(Globals.ActiveUser, Permission.AccessSalesTypes) ||
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

        public Employee Employee { get; set; }
        public SalesOrderPosition SalesOrderPosition { get; set; } = new SalesOrderPosition();
        public SalesOrderPosition SelectedSalesOrderPosition { get; set; }
        public SvenTechCollection<Debitor> Debitors { get; set; } = new SvenTechCollection<Debitor>();
        public SvenTechCollection<Employee> Employees { get; set; } = new SvenTechCollection<Employee>();
        public SvenTechCollection<Product> Products { get; set; } = new SvenTechCollection<Product>();
        public SvenTechCollection<SalesType> SalesTypes { get; set; } = new SvenTechCollection<SalesType>();

        public SvenTechCollection<SalesOrderPosition> SalesOrderPositions { get; set; } =
            new SvenTechCollection<SalesOrderPosition>();

        public DelegateCommand NewSalesOrderCommand { get; set; }
        public DelegateCommand CreatePDFCommand { get; set; }
        public DelegateCommand SavePositionCommand { get; set; }
        public DelegateCommand DeletePositionCommand { get; set; }
        public DelegateCommand SaveSalesOrderCommand { get; set; }
        public DelegateCommand OpenSalesTypesWindowCommand { get; set; }

        #endregion Properties

        #region Methods

        private void SetCommands()
        {
            CreatePDFCommand = new DelegateCommand(CreatePDFFile, () => ValidatePDFCreation());
            SavePositionCommand = new DelegateCommand(SaveSalesOrderPosition, () => ValidateSalesOrderPosition());
            DeletePositionCommand =
                new DelegateCommand(DeleteSalesOrderPosition, () => SelectedSalesOrderPosition != null);
            SaveSalesOrderCommand = new DelegateCommand(SaveSalesOrder, () => ValidateSalesOrder());
            OpenSalesTypesWindowCommand = new DelegateCommand(OpenSalesTypesWindow);
        }

        private bool ValidatePDFCreation()
        {
            if (SalesOrder.SalesOrderPositions.Count < 1) return false;

            if (!ValidateSalesOrder()) return false;

            return true;
        }

        private bool ValidateSalesOrderPosition()
        {
            if (SalesOrderPosition.RefProductId == 0) return false;

            return true;
        }

        private bool ValidateSalesOrder()
        {
            if (SalesOrder == null) return false;

            if (SalesOrder.SalesOrderPositions.Count < 1) return false;

            if (SalesOrder.RefDebitorId == 0 || SalesOrder.RefSalesTypeId == 0) return false;

            return true;
        }

        private void OpenSalesTypesWindow()
        {
            Messenger.Default.Send(new OpenSalesTypesWindowMessage());
        }

        private void DeleteSalesOrderPosition()
        {
            if (SelectedSalesOrderPosition != null) SalesOrder.SalesOrderPositions.Remove(SelectedSalesOrderPosition);
        }

        private void SaveSalesOrderPosition()
        {
            SalesOrder.SalesOrderPositions.Add(SalesOrderPosition);
            SalesOrderPosition = new SalesOrderPosition();
            SelectedProduct = new Product();
        }

        private void CreatePDFFile()
        {
            var salesOrderReportData = new SalesOrderReportData
            {
                MyCompany = Globals.CoreData.MyCompany,
                SalesOrder = SalesOrder,
                Employee = Employee
            };

            var listReportData = new List<SalesOrderReportData> {salesOrderReportData};

            var sor = new SalesOrderReport
            {
                DataSource = listReportData
            };
            sor.CreateDocument();
            var path = @"C:\test\test.pdf";
            sor.PrintingSystem.ExportToPdf(path);
            Messenger.Default.Send(new OpenPDFViewerWindowMessage(path));
        }

        private void SaveSalesOrder()
        {
            SalesOrder.RefShipmentTypeId = 1;
            SalesOrder.SalesOrderId = DataContext.Instance.SalesOrders.Insert(SalesOrder);

            foreach (var item in SalesOrder.SalesOrderPositions)
            {
                item.RefSalesOrderId = SalesOrder.SalesOrderId;
                DataContext.Instance.SalesOrderPositions.Insert(item);
            }

            Clear();
        }

        private void Clear()
        {
            SalesOrder = new SalesOrder();
            SelectedProduct = new Product();
            SalesOrderPositions.Clear();
        }

        private void GetData()
        {
            Debitors = DataContext.Instance.Debitors.GetAll().ToSvenTechCollection();
            Products = DataContext.Instance.Products.GetAll().ToSvenTechCollection();
            SalesTypes = DataContext.Instance.SalesTypes.GetAll().ToSvenTechCollection();
            Employees = DataContext.Instance.Employees.GetAll().ToSvenTechCollection();
        }

        private void ChangeSelectedSalesType(SelectedSalesType SelectedSalesType)
        {
            SalesTypes = DataContext.Instance.SalesTypes.GetAll().ToSvenTechCollection();
            SalesOrder.SalesType = SelectedSalesType.SalesType;
            SalesOrder.RefSalesTypeId = SelectedSalesType.SalesType.SalesTypeId;
            RaisePropertyChanged("SalesOrder");
        }

        #endregion Methods
    }
}