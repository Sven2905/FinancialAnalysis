﻿using System;
using FinancialAnalysis.Datalayer.Accounting;
using FinancialAnalysis.Datalayer.Administration;
using FinancialAnalysis.Datalayer.Configurations;
using FinancialAnalysis.Datalayer.ProductManagement;
using FinancialAnalysis.Datalayer.ProjectManagement;
using FinancialAnalysis.Datalayer.Tables;

namespace FinancialAnalysis.Datalayer
{
    public class DataLayer : IDisposable
    {
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
        public Products ProductPrototypes { get; set; } = new Products();
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
            Users.CheckAndCreateStoredProcedures();
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
            PaymentConditions.AddReferences();
            ProjectWorkingTimes.AddReferences();
            UserRightUserMappings.AddReferences();
        }
    }
}