﻿using FinancialAnalysis.Datalayer.Accounting;
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
    public class DataLayer : IDisposable
    {
        public static DataLayer Instance { get; } = new DataLayer();

        public TableVersions TableVersions { get; set; } = new TableVersions();
        public Clients Clients { get; set; } = new Clients();
        public Companies Companies { get; set; } = new Companies();
        public CostAccountCategories CostAccountCategories { get; set; } = new CostAccountCategories();
        public CostAccounts CostAccounts { get; set; } = new CostAccounts();
        public TaxTypes TaxTypes { get; set; } = new TaxTypes();
        public Creditors Creditors { get; set; } = new Creditors();
        public Debitors Debitors { get; set; } = new Debitors();
        public Debits Debits { get; set; } = new Debits();
        public Credits Credits { get; set; } = new Credits();
        public Bookings Bookings { get; set; } = new Bookings();
        public ScannedDocuments ScannedDocuments { get; set; } = new ScannedDocuments();
        public ProductCategories ProductCategories { get; set; } = new ProductCategories();
        public Products Products { get; set; } = new Products();
        public Projects Projects { get; set; } = new Projects();
        public CostCenters CostCenters { get; set; } = new CostCenters();
        public ProjectRoles ProjectRoles { get; set; } = new ProjectRoles();
        public HealthInsurances HealthInsurances { get; set; } = new HealthInsurances();
        public Employees Employees { get; set; } = new Employees();
        public ProjectEmployeeMappings ProjectEmployeeMappings { get; set; } = new ProjectEmployeeMappings();
        public ProjectWorkingTimes ProjectWorkingTimes { get; set; } = new ProjectWorkingTimes();
        public Cashbacks Cashbacks { get; set; } = new Cashbacks();
        public PaymentConditions PaymentConditions { get; set; } = new PaymentConditions();
        public MailConfigurations MailConfigurations { get; set; } = new MailConfigurations();
        public Users Users { get; set; } = new Users();
        public UserRights UserRights { get; set; } = new UserRights();
        public UserRightUserMappings UserRightUserMappings { get; set; } = new UserRightUserMappings();
        public Warehouses Warehouses { get; set; } = new Warehouses();
        public Stockyards Stockyards { get; set; } = new Stockyards();
        public InvoiceTypes InvoiceTypes { get; set; } = new InvoiceTypes();
        public Invoices Invoices { get; set; } = new Invoices();
        public ShipmentTypes ShipmentTypes { get; set; } = new ShipmentTypes();
        public SalesTypes SalesTypes { get; set; } = new SalesTypes();
        public Shipments Shipments { get; set; } = new Shipments();
        public SalesOrderPositions SalesOrderPositions { get; set; } = new SalesOrderPositions();
        public SalesOrders SalesOrders { get; set; } = new SalesOrders();
        public StockedProducts StockedProducts { get; set; } = new StockedProducts();
        public ShippedProducts ShippedProducts { get; set; } = new ShippedProducts();

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
            Cashbacks.CheckAndCreateStoredProcedures();
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
            PaymentConditions.AddReferences();
            ProjectWorkingTimes.AddReferences();
            UserRightUserMappings.AddReferences();
            Stockyards.AddReferences();
            Invoices.AddReferences();
            SalesOrderPositions.AddReferences();
            SalesOrders.AddReferences();
            StockedProducts.AddReferences();
            Companies.AddReferences();
            ShippedProducts.AddReferences();
        }

        private void Seed()
        {
            if (Instance.TaxTypes.GetAll().Count() == 0)
            {
                var _Import = new Import();
                _Import.SeedCompany();
                _Import.ImportCostAccounts(Standardkontenrahmen.SKR03);
                _Import.SeedTaxTypes();
            }
        }
    }
}
