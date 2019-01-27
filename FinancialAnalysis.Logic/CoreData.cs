﻿using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.CompanyManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace FinancialAnalysis.Logic
{
    public class CoreData
    {
        public static CoreData Instance { get; } = new CoreData();

        public SvenTechCollection<TaxType> TaxTypes { get; private set; } = new SvenTechCollection<TaxType>();
        public Company MyCompany { get; private set; }

        public CoreData()
        {
            RefreshData();
        }

        private Company LoadMyCompanyFromDb()
        {
            return DataLayer.Instance.Companies.GetById(1);
        }

        private SvenTechCollection<TaxType> LoadTaxTypesFromDb()
        {
            return DataLayer.Instance.TaxTypes.GetAll().ToSvenTechCollection();
        }

        public void RefreshData()
        {
            TaxTypes = LoadTaxTypesFromDb();
            MyCompany = LoadMyCompanyFromDb();
        }
    }
}