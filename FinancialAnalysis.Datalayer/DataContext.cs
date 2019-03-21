using FinancialAnalysis.Datalayer.Accounting;
using FinancialAnalysis.Datalayer.Administration;
using FinancialAnalysis.Datalayer.ClientManagement;
using FinancialAnalysis.Datalayer.Configurations;
using FinancialAnalysis.Datalayer.ProductManagement;
using FinancialAnalysis.Datalayer.ProjectManagement;
using FinancialAnalysis.Datalayer.SalesManagement;
using FinancialAnalysis.Datalayer.Tables;
using FinancialAnalysis.Datalayer.WarehouseManagement;
using FinancialAnalysis.Logic;
using FinancialAnalysis.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FinancialAnalysis.Datalayer
{
    public class DataContext : IDisposable
    {
        public static DataContext Instance { get; } = new DataContext();

        public TableVersions TableVersions { get; set; } = new TableVersions();
        public BalanceAccounts BalanceAccounts { get; set; } = new BalanceAccounts();
        public Bookings Bookings { get; set; } = new Bookings();
        public CarBodies CarBodies { get; set; } = new CarBodies();
        public CarEngines CarEngines { get; set; } = new CarEngines();
        public CarGenerations CarGenerations { get; set; } = new CarGenerations();
        public CarMakes CarMakes { get; set; } = new CarMakes();
        public CarModels CarModels { get; set; } = new CarModels();
        public CarTrims CarTrims { get; set; } = new CarTrims();
        public Clients Clients { get; set; } = new Clients();
        public Companies Companies { get; set; } = new Companies();
        public CostAccountCategories CostAccountCategories { get; set; } = new CostAccountCategories();
        public CostAccounts CostAccounts { get; set; } = new CostAccounts();
        public CostCenterBudgets CostCenterBudgets { get; set; } = new CostCenterBudgets();
        public CostCenterCategories CostCenterCategories { get; set; } = new CostCenterCategories();
        public CostCenters CostCenters { get; set; } = new CostCenters();
        public Creditors Creditors { get; set; } = new Creditors();
        public Credits Credits { get; set; } = new Credits();
        public Debitors Debitors { get; set; } = new Debitors();
        public Debits Debits { get; set; } = new Debits();
        public Employees Employees { get; set; } = new Employees();
        public FixedCostAllocations FixedCostAllocations { get; set; } = new FixedCostAllocations();
        public GainAndLossAccounts GainAndLossAccounts { get; set; } = new GainAndLossAccounts();
        public HealthInsurances HealthInsurances { get; set; } = new HealthInsurances();
        public InvoicePositions InvoicePositions { get; set; } = new InvoicePositions();
        public InvoiceReminders InvoiceReminders { get; set; } = new InvoiceReminders();
        public Invoices Invoices { get; set; } = new Invoices();
        public InvoiceTypes InvoiceTypes { get; set; } = new InvoiceTypes();
        public MailConfigurations MailConfigurations { get; set; } = new MailConfigurations();
        public PaymentConditions PaymentConditions { get; set; } = new PaymentConditions();
        public ProductCategories ProductCategories { get; set; } = new ProductCategories();
        public Products Products { get; set; } = new Products();
        public ProjectEmployeeMappings ProjectEmployeeMappings { get; set; } = new ProjectEmployeeMappings();
        public ProjectRoles ProjectRoles { get; set; } = new ProjectRoles();
        public Projects Projects { get; set; } = new Projects();
        public ProjectWorkingTimes ProjectWorkingTimes { get; set; } = new ProjectWorkingTimes();
        public SalesOrderPositions SalesOrderPositions { get; set; } = new SalesOrderPositions();
        public SalesOrders SalesOrders { get; set; } = new SalesOrders();
        public SalesTypes SalesTypes { get; set; } = new SalesTypes();
        public ScannedDocuments ScannedDocuments { get; set; } = new ScannedDocuments();
        public Shipments Shipments { get; set; } = new Shipments();
        public ShipmentTypes ShipmentTypes { get; set; } = new ShipmentTypes();
        public ShippedProducts ShippedProducts { get; set; } = new ShippedProducts();
        public StockedProducts StockedProducts { get; set; } = new StockedProducts();
        public Stockyards Stockyards { get; set; } = new Stockyards();
        public TaxTypes TaxTypes { get; set; } = new TaxTypes();
        public UserRightUserMappings UserRightUserMappings { get; set; } = new UserRightUserMappings();
        public UserRights UserRights { get; set; } = new UserRights();
        public Users Users { get; set; } = new Users();
        public Vehicles Vehicles { get; set; } = new Vehicles();
        public Warehouses Warehouses { get; set; } = new Warehouses();
        public WarehouseStockingHistories WarehouseStockingHistories { get; set; } = new WarehouseStockingHistories();

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void CreateDatabaseSchema()
        {
            CheckAndCreateStoredProcedures();
            Task.Run(() =>
            {
                Seed();
                AddReferences();
            });
        }

        private void CheckAndCreateStoredProcedures()
        {
            TableVersions.CheckAndCreateStoredProcedures();
            Users.CheckAndCreateStoredProcedures();
            Clients.CheckAndCreateStoredProcedures();
            Companies.CheckAndCreateStoredProcedures();
            CostAccountCategories.CheckAndCreateStoredProcedures();
            CostAccounts.CheckAndCreateStoredProcedures();
            TaxTypes.CheckAndCreateStoredProcedures();
            Creditors.CheckAndCreateStoredProcedures();
            Debitors.CheckAndCreateStoredProcedures();
            Credits.CheckAndCreateStoredProcedures();
            Debits.CheckAndCreateStoredProcedures();
            ScannedDocuments.CheckAndCreateStoredProcedures();
            Bookings.CheckAndCreateStoredProcedures();
            ProductCategories.CheckAndCreateStoredProcedures();
            Products.CheckAndCreateStoredProcedures();
            Projects.CheckAndCreateStoredProcedures();
            CostCenters.CheckAndCreateStoredProcedures();
            Employees.CheckAndCreateStoredProcedures();
            ProjectRoles.CheckAndCreateStoredProcedures();
            ProjectEmployeeMappings.CheckAndCreateStoredProcedures();
            PaymentConditions.CheckAndCreateStoredProcedures();
            MailConfigurations.CheckAndCreateStoredProcedures();
            ProjectWorkingTimes.CheckAndCreateStoredProcedures();
            HealthInsurances.CheckAndCreateStoredProcedures();
            UserRights.CheckAndCreateStoredProcedures();
            UserRightUserMappings.CheckAndCreateStoredProcedures();
            Warehouses.CheckAndCreateStoredProcedures();
            Stockyards.CheckAndCreateStoredProcedures();
            InvoiceTypes.CheckAndCreateStoredProcedures();
            Invoices.CheckAndCreateStoredProcedures();
            ShipmentTypes.CheckAndCreateStoredProcedures();
            SalesTypes.CheckAndCreateStoredProcedures();
            Shipments.CheckAndCreateStoredProcedures();
            SalesOrderPositions.CheckAndCreateStoredProcedures();
            SalesOrders.CheckAndCreateStoredProcedures();
            StockedProducts.CheckAndCreateStoredProcedures();
            ShippedProducts.CheckAndCreateStoredProcedures();
            InvoicePositions.CheckAndCreateStoredProcedures();
            CostCenterCategories.CheckAndCreateStoredProcedures();
            WarehouseStockingHistories.CheckAndCreateStoredProcedures();
            FixedCostAllocations.CheckAndCreateStoredProcedures();
            CostCenterBudgets.CheckAndCreateStoredProcedures();
            BalanceAccounts.CheckAndCreateStoredProcedures();
            GainAndLossAccounts.CheckAndCreateStoredProcedures();
            InvoiceReminders.CheckAndCreateStoredProcedures();
            CarBodies.CheckAndCreateStoredProcedures();
            CarEngines.CheckAndCreateStoredProcedures();
            CarGenerations.CheckAndCreateStoredProcedures();
            CarMakes.CheckAndCreateStoredProcedures();
            CarModels.CheckAndCreateStoredProcedures();
            CarTrims.CheckAndCreateStoredProcedures();
            Vehicles.CheckAndCreateStoredProcedures();
        }

        private void AddReferences()
        {
            CostAccounts.AddReferences();
            Creditors.AddReferences();
            Debitors.AddReferences();
            Credits.AddReferences();
            Debits.AddReferences();
            ScannedDocuments.AddReferences();
            Products.AddReferences();
            Projects.AddReferences();
            Employees.AddReferences();
            ProjectEmployeeMappings.AddReferences();
            ProjectWorkingTimes.AddReferences();
            UserRightUserMappings.AddReferences();
            Stockyards.AddReferences();
            Invoices.AddReferences();
            SalesOrderPositions.AddReferences();
            SalesOrders.AddReferences();
            StockedProducts.AddReferences();
            Companies.AddReferences();
            ShippedProducts.AddReferences();
            InvoicePositions.AddReferences();
            Bookings.AddReferences();
            CostCenters.AddReferences();
            FixedCostAllocations.AddReferences();
            CostCenterBudgets.AddReferences();
            InvoiceReminders.AddReferences();
        }

        private void Seed()
        {
            var _Import = new Import();
            if (!Instance.TaxTypes.GetAll().Any())
            {
                _Import.SeedTaxTypes();
            }
            if (!Instance.BalanceAccounts.GetAll().Any())
            {
                _Import.SeedBalanceAccounts();
            }
            if (!Instance.GainAndLossAccounts.GetAll().Any())
            {
                _Import.SeedGainAndLossAccounts();
            }
            if (!Instance.CostAccounts.GetAll().Any())
            {
                _Import.ImportCostAccounts(Standardkontenrahmen.SKR03);
            }
            if (!Instance.Clients.GetAll().Any())
            {
                _Import.SeedCompany();
            }
            if (!Instance.CostCenters.GetAll().Any())
            {
                _Import.SeedCostCenters();
            }
            if (!Instance.HealthInsurances.GetAll().Any())
            {
                _Import.SeedHealthInsurance();
            }
            if (!Instance.PaymentConditions.GetAll().Any())
            {
                _Import.SeedPaymentCondition();
            }
            if (!Instance.CarMakes.GetAll().Any())
            {
                _Import.SeedCars();
            }
        }
    }
}