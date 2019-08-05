namespace Formulas.PriceCalculationMethods
{
    public delegate void ValueChangedEvent();

    public class ItemPriceCalculationInputItem
    {
        #region Events

        public event ValueChangedEvent ValueChanged;

        #endregion Events

        #region Fields

        private decimal productionMaterial = 0;
        private decimal materialOverheadCosts = 0;
        private decimal productWages = 0;
        private decimal productOverheads = 0;
        private decimal administrativeOverheads = 0;
        private decimal salesOverheads = 0;
        private decimal profitSurcharge = 0;
        private decimal customerCashback = 0;
        private decimal agentCommission = 0;
        private decimal customerDiscount = 0;
        private decimal tax = 19;

        #endregion Fields

        #region Properties

        /// <summary>
        /// Fertigungsmaterial
        /// </summary>
        public decimal ProductionMaterial
        {
            get { return productionMaterial; }
            set { productionMaterial = value; ValueChanged?.Invoke(); }
        }

        /// <summary>
        /// Materialgemeinkosten (%)
        /// </summary>
        public decimal MaterialOverheadCosts
        {
            get { return materialOverheadCosts; }
            set { materialOverheadCosts = value; ValueChanged?.Invoke(); }
        }

        /// <summary>
        /// Fertigungslöhne
        /// </summary>
        public decimal ProductWages
        {
            get { return productWages; }
            set { productWages = value; ValueChanged?.Invoke(); }
        }

        /// <summary>
        /// Fertigungsgemeinkosten (%)
        /// </summary>
        public decimal ProductOverheads
        {
            get { return productOverheads; }
            set { productOverheads = value; ValueChanged?.Invoke(); }
        }

        /// <summary>
        /// Verwaltungsgemeinkosten (%)
        /// </summary>
        public decimal AdministrativeOverheads
        {
            get { return administrativeOverheads; }
            set { administrativeOverheads = value; ValueChanged?.Invoke(); }
        }

        /// <summary>
        /// Vertriebsgemeinkosten (%)
        /// </summary>
        public decimal SalesOverheads
        {
            get { return salesOverheads; }
            set { salesOverheads = value; ValueChanged?.Invoke(); }
        }

        /// <summary>
        /// Gewinnzuschlag (%)
        /// </summary>
        public decimal ProfitSurcharge
        {
            get { return profitSurcharge; }
            set { profitSurcharge = value; ValueChanged?.Invoke(); }
        }

        /// <summary>
        /// Kundenskonto (%)
        /// </summary>
        public decimal CustomerCashback
        {
            get { return customerCashback; }
            set { customerCashback = value; ValueChanged?.Invoke(); }
        }

        /// <summary>
        /// Vertreterprovision (%)
        /// </summary>
        public decimal AgentCommission
        {
            get { return agentCommission; }
            set { agentCommission = value; ValueChanged?.Invoke(); }
        }

        /// <summary>
        /// Kundenrabatt
        /// </summary>
        public decimal CustomerDiscount
        {
            get { return customerDiscount; }
            set { customerDiscount = value; ValueChanged?.Invoke(); }
        }

        /// <summary>
        /// Umsatzsteuer (%)
        /// </summary>
        public decimal Tax
        {
            get { return tax; }
            set { tax = value; ValueChanged?.Invoke(); }
        }

        #endregion Properties
    }
}