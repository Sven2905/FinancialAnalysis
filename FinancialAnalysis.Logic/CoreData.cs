using FinancialAnalysis.Models.Accounting;
using FinancialAnalysis.Models.ClientManagement;
using System.Linq;
using Utilities;
using WebApiWrapper.Accounting;
using WebApiWrapper.ClientManagement;

namespace FinancialAnalysis.Logic
{
    public class CoreData
    {
        public CoreData()
        {
            RefreshData();
        }

        public static CoreData Instance { get; } = new CoreData();

        public SvenTechCollection<TaxType> TaxTypeList { get; private set; }
        public Client MyCompany { get; private set; }

        public TaxType GetTaxTypeById(int taxTypeId)
        {
            return TaxTypeList.SingleOrDefault(x => x.TaxTypeId == taxTypeId);
        }

        private Client LoadMyCompanyFromDb()
        {
            return Clients.GetById(1);
        }

        private SvenTechCollection<TaxType> LoadTaxTypesFromDb()
        {
            return TaxTypes.GetAll().ToSvenTechCollection();
        }

        public void RefreshData()
        {
            TaxTypeList = LoadTaxTypesFromDb();
            MyCompany = LoadMyCompanyFromDb();
        }
    }
}