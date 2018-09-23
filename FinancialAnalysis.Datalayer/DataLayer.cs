﻿using FinancialAnalysis.Datalayer.Tables;
using System;

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
        }

        private void AddReferences()
        {
            CostAccounts.AddReferences();
            Creditors.AddReferences();
            Debitors.AddReferences();
        }
    }
}
