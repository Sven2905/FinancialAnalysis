using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas.PriceCalculationMethods
{
    public delegate void ValueChanged();

    public class ItemPriceCalculationInputItem
    {
        #region Events

        public event ValueChanged ValueChanged;

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
            set { productionMaterial = value; ValueChanged(); }
        }

        /// <summary>
        /// Materialgemeinkosten (%)
        /// </summary>
        public decimal MaterialOverheadCosts
        {
            get { return materialOverheadCosts; }
            set { materialOverheadCosts = value; ValueChanged(); }
        }

        /// <summary>
        /// Fertigungslöhne
        /// </summary>
        public decimal ProductWages
        {
            get { return productWages; }
            set { productWages = value; ValueChanged(); }
        }

        /// <summary>
        /// Fertigungsgemeinkosten (%)
        /// </summary>
        public decimal ProductOverheads
        {
            get { return productOverheads; }
            set { productOverheads = value; ValueChanged(); }
        }

        /// <summary>
        /// Verwaltungsgemeinkosten (%)
        /// </summary>
        public decimal AdministrativeOverheads
        {
            get { return administrativeOverheads; }
            set { administrativeOverheads = value; ValueChanged(); }
        }

        /// <summary>
        /// Vertriebsgemeinkosten (%)
        /// </summary>
        public decimal SalesOverheads
        {
            get { return salesOverheads; }
            set { salesOverheads = value; ValueChanged(); }
        }

        /// <summary>
        /// Gewinnzuschlag (%)
        /// </summary>
        public decimal ProfitSurcharge
        {
            get { return profitSurcharge; }
            set { profitSurcharge = value; ValueChanged(); }
        }

        /// <summary>
        /// Kundenskonto (%)
        /// </summary>
        public decimal CustomerCashback
        {
            get { return customerCashback; }
            set { customerCashback = value; ValueChanged(); }
        }

        /// <summary>
        /// Vertreterprovision (%)
        /// </summary>
        public decimal AgentCommission
        {
            get { return agentCommission; }
            set { agentCommission = value; ValueChanged(); }
        }

        /// <summary>
        /// Kundenrabatt 
        /// </summary>
        public decimal CustomerDiscount
        {
            get { return customerDiscount; }
            set { customerDiscount = value; ValueChanged(); }
        }

        /// <summary>
        /// Umsatzsteuer (%)
        /// </summary>
        public decimal Tax
        {
            get { return tax; }
            set { tax = value; ValueChanged(); }
        }

        #endregion Properties
    }
}
