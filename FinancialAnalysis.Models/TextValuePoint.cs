namespace FinancialAnalysis.Models
{
    public class TextValuePoint
    {
        public TextValuePoint(string Text, double Value)
        {
            this.Text = Text;
            this.Value = Value;
        }

        public string Text { get; set; }
        public double Value { get; set; }
    }
}