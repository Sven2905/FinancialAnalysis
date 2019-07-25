namespace FinancialAnalysis.Logic.Messages
{
    public class OpenQuantityWindowMessage
    {
        public OpenQuantityWindowMessage(int MaxQuantity)
        {
            this.MaxQuantity = MaxQuantity;
        }

        public int MaxQuantity { get; set; }
    }
}