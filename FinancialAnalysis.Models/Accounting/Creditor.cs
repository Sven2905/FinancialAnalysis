using DevExpress.Mvvm;
using FinancialAnalysis.Models.ClientManagement;

namespace FinancialAnalysis.Models.Accounting
{
    public class Creditor : BindableBase
    {
        public Creditor()
        {
            Client = new Client();
            CostAccount = new CostAccount();
        }

        public int CreditorId { get; set; }
        public int RefClientId { get; set; }
        public Client Client { get; set; }
        public int RefCostAccountId { get; set; }
        public CostAccount CostAccount { get; set; }
    }
}