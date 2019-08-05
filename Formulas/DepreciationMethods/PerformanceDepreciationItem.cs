namespace Formulas.DepreciationMethods
{
    public class PerformanceDepreciationItem
    {
        public PerformanceDepreciationItem()
        {
        }

        public PerformanceDepreciationItem(int Year, decimal Power)
        {
            this.Year = Year;
            this.Power = Power;
        }

        public int Year { get; set; }
        public decimal Power { get; set; }
    }
}