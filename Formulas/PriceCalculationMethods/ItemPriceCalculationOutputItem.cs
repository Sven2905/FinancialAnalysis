using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formulas.PriceCalculationMethods
{
    public class ItemPriceCalculationOutputItem
    {
        #region Materialkosten

        /// <summary>
        /// Fertigungsmaterial
        /// </summary>
        public decimal ProductionMaterial { get; set; } = 0;

        /// <summary>
        /// Materialgemeinkosten
        /// </summary>
        public decimal MaterialOverheadCosts { get; set; } = 0;

        /// <summary>
        /// Materialkosten
        /// </summary>
        public decimal MaterialCosts => ProductionMaterial + MaterialOverheadCosts;

        #endregion Materialkosten

        #region Fertigungskosten

        /// <summary>
        /// Fertigungslöhne
        /// </summary>
        public decimal ProductWages { get; set; } = 0;

        /// <summary>
        /// Fertigungsgemeinkosten
        /// </summary>
        public decimal ProductOverheads { get; set; } = 0;

        /// <summary>
        /// Fertigungskosten
        /// </summary>
        public decimal ManufacturingCosts => ProductWages + ProductOverheads;

        #endregion Fertigungskosten

        #region Herstellkosten

        /// <summary>
        /// Herstellkosten
        /// </summary>
        public decimal ProductionCosts => MaterialCosts + ManufacturingCosts;

        #endregion Herstellkosten

        #region Selbstkosten

        /// <summary>
        /// Verwaltungsgemeinkosten
        /// </summary>
        public decimal AdministrativeOverheads { get; set; } = 0;

        /// <summary>
        /// Vertriebsgemeinkosten
        /// </summary>
        public decimal SalesOverheads { get; set; } = 0;

        /// <summary>
        /// Selbstkosten
        /// </summary>
        public decimal CostPrice => ProductionCosts + AdministrativeOverheads + SalesOverheads;

        #endregion Selbstkosten

        #region Barverkaufspreis

        /// <summary>
        /// Gewinnzuschlag
        /// </summary>
        public decimal ProfitSurcharge { get; set; } = 0;

        /// <summary>
        /// Barverkaufspreis
        /// </summary>
        public decimal CashSellingPrice => ProfitSurcharge + CostPrice;

        #endregion Barverkaufspreis

        #region Zielverkaufspreis

        /// <summary>
        /// Kundenskonto
        /// </summary>
        public decimal CustomerCashback { get; set; } = 0;

        /// <summary>
        /// Vertreterprovision
        /// </summary>
        public decimal AgentCommission { get; set; } = 0;

        /// <summary>
        /// Zielverkaufspreis
        /// </summary>
        public decimal TargetSalesPrice => CashSellingPrice + CustomerCashback + AgentCommission;

        #endregion Zielverkaufspreis

        #region Angebotspreis

        /// <summary>
        /// Kundenrabatt 
        /// </summary>
        public decimal CustomerDiscount { get; set; } = 0;

        /// <summary>
        /// Angebotspreis
        /// </summary>
        public decimal OfferPrice => TargetSalesPrice + CustomerDiscount;

        #endregion Angebotspreis

        #region Bruttoverkaufspreis

        /// <summary>
        /// Umsatzsteuer
        /// </summary>
        public decimal Tax { get; set; } = 0;

        /// <summary>
        /// Bruttoverkaufspreis
        /// </summary>
        public decimal GrossSellingPrice => OfferPrice + Tax;

        #endregion Bruttoverkaufspreis
    }
}
