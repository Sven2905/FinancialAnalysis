﻿using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.ClientManagement;
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

        public SvenTechCollection<TaxType> TaxTypes { get; private set; }
        public Client MyCompany { get; private set; }

        public TaxType GetTaxTypeById(int taxTypeId)
        {
            return TaxTypes.SingleOrDefault(x => x.TaxTypeId == taxTypeId);
        }

        public CoreData()
        {
            RefreshData();
        }

        private Client LoadMyCompanyFromDb()
        {
            return DataContext.Instance.Clients.GetById(1);
        }

        private SvenTechCollection<TaxType> LoadTaxTypesFromDb()
        {
            return DataContext.Instance.TaxTypes.GetAll().ToSvenTechCollection();
        }

        public void RefreshData()
        {
            TaxTypes = LoadTaxTypesFromDb();
            MyCompany = LoadMyCompanyFromDb();
        }
    }
}
