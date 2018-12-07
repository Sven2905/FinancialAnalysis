using FinancialAnalysis.Datalayer.Accounting;
using FinancialAnalysis.Datalayer.Product;
using FinancialAnalysis.Datalayer.ProjectManagement;
using FinancialAnalysis.Datalayer.Tables;
using System;
using System.Collections.Generic;

namespace FinancialAnalysis.Datalayer
{
    public class DataLayer : IDisposable
    {
        public DataLayer()
        {

        }
        
        public TaxTypes TaxTypes { get; set; } = new TaxTypes();
        public TableVersions TableVersions { get; set; } = new TableVersions();
        public Companies Companies { get; set; } = new Companies();
        public CostAccountCategories CostAccountCategories { get; set; } = new CostAccountCategories();
        public CostAccounts CostAccounts { get; set; } = new CostAccounts();
        public Creditors Creditors { get; set; } = new Creditors();
        public Debitors Debitors { get; set; } = new Debitors();
        public Debits Debits { get; set; } = new Debits();
        public Credits Credits { get; set; } = new Credits();
        public Bookings Bookings { get; set; } = new Bookings();
        public ScannedDocuments ScannedDocuments { get; set; } = new ScannedDocuments();
        public ProductCategories ProductCategories { get; set; } = new ProductCategories();
        public ProductPrototypes ProductPrototypes { get; set; } = new ProductPrototypes();
        public Projects Projects { get; set; } = new Projects();
        public ProjectRoles ProjectRoles { get; set; } = new ProjectRoles();
        public Employees Employees { get; set; } = new Employees();
        public ProjectEmployeeMappings ProjectEmployeeMappings { get; set; } = new ProjectEmployeeMappings();

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public void CreateDatabaseSchema()
        {
            //if (TableVersions.GetById(1) == null || TableVersions.GetById(1).Version != 1)
            //{
                CheckAndCreateStoredProcedures();
                AddReferences();

                //TableVersions.Insert(new Models.TableVersion() { Name = "Alpha", Version = 1, LastModified = DateTime.Now });
            //}
        }

        private void CheckAndCreateStoredProcedures()
        {
            TableVersions.CheckAndCreateStoredProcedures();
            TaxTypes.CheckAndCreateStoredProcedures();
            Companies.CheckAndCreateStoredProcedures();
            CostAccountCategories.CheckAndCreateStoredProcedures();
            CostAccounts.CheckAndCreateStoredProcedures();
            Creditors.CheckAndCreateStoredProcedures();
            Debitors.CheckAndCreateStoredProcedures();
            Credits.CheckAndCreateStoredProcedures();
            Debits.CheckAndCreateStoredProcedures();
            ScannedDocuments.CheckAndCreateStoredProcedures();
            Bookings.CheckAndCreateStoredProcedures();
            ProductCategories.CheckAndCreateStoredProcedures();
            ProductPrototypes.CheckAndCreateStoredProcedures();
            Projects.CheckAndCreateStoredProcedures();
            Employees.CheckAndCreateStoredProcedures();
            ProjectRoles.CheckAndCreateStoredProcedures();
            ProjectEmployeeMappings.CheckAndCreateStoredProcedures();
        }

        private void AddReferences()
        {
            CostAccounts.AddReferences();
            Creditors.AddReferences();
            Debitors.AddReferences();
            Credits.AddReferences();
            Debits.AddReferences();
            ScannedDocuments.AddReferences();
            ProductPrototypes.AddReferences();
            Projects.AddReferences();
            Employees.AddReferences();
            ProjectEmployeeMappings.AddReferences();
        }
    }
}
