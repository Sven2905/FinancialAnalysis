namespace Formulas.PriceCalculationMethods
{
    public delegate void ValueChangedEvent();

    public class StandardItemPriceCalculation
    {
        // https://www.buhl.de/meinbuero/preiskalkulation-2/

        /*
         * Fertigungsmaterial
         * + Materialgemeinkosten
         * = Materialkosten (MK)
         *
         * Fertigungslöhne
         * + Fertigungsgemeinkosten
         * = Fertigungskosten (FK)
         *
         * = Herstellkosten: MK + FK
         *
         * + Verwaltungsgemeinkosten
         * + Vertriebsgemeinkosten
         * = Selbstkosten
         *
         * + Gewinnzuschlag
         * = Barverkaufspreis
         *
         * + Kundenskonto
         * + Vertreterprovision
         * = Zielverkaufspreis
         *
         * + Kundenrabatt
         * = Angebotspreis
         *
         * + Umsatzsteuer
         * = Bruttoverkaufspreis

         */

        #region Events

        public event ValueChangedEvent ValueChanged;

        #endregion Events

        #region Fields

        private decimal productionMaterial = 0;
        private decimal profitSurcharge = 0;
        private decimal customerCashback = 0;
        private decimal agentCommission = 0;
        private decimal customerDiscount = 0;
        private decimal tax = 19;
        private decimal productionTime;
        private decimal hourlyWage;
        private int itemAmountPerAnno;
        private decimal materialOverHeadCostCentersAmount;
        private decimal productOverHeadCostCentersAmount;
        private decimal administrativeOverHeadCostCentersAmount;
        private decimal salesOverHeadCostCentersAmount;

        #endregion Fields

        #region Properties

        #region Allgemein

        /// <summary>
        /// Produzierte Stückzahl pro Jahr
        /// </summary>
        public int ItemAmountPerAnno
        {
            get { return itemAmountPerAnno; }
            set { itemAmountPerAnno = value; ValueChanged?.Invoke(); }
        }

        #endregion Allgemein

        #region Materialkosten

        /// <summary>
        /// Fertigungsmaterial
        /// </summary>
        public decimal ProductionMaterial
        {
            get { return productionMaterial; }
            set { productionMaterial = value; ValueChanged?.Invoke(); }
        }

        /// <summary>
        /// Anfallende Gesamtkosten auf der Kostenstelle Material
        /// </summary>
        public decimal MaterialOverHeadCostCentersAmount
        {
            get { return materialOverHeadCostCentersAmount; }
            set { materialOverHeadCostCentersAmount = value; }
        }

        /// <summary>
        /// Materialgemeinkosten (%)
        /// </summary>
        public decimal MaterialOverheadCosts
        {
            get
            {
                if (ItemAmountPerAnno > 0 && ProductionMaterial > 0)
                    return materialOverHeadCostCentersAmount / (itemAmountPerAnno * productionMaterial);
                else
                    return 0;
            }
        }

        /// <summary>
        /// Materialgemeinkosten
        /// </summary>
        public decimal MaterialOverheadCostsValue => productionMaterial * MaterialOverheadCosts;

        /// <summary>
        /// Materialkosten
        /// </summary>
        public decimal MaterialCosts => productionMaterial + MaterialOverheadCostsValue;

        #endregion Materialkosten

        #region Fertigungskosten

        /// <summary>
        /// Stundenlohn
        /// </summary>
        public decimal HourlyWage
        {
            get { return hourlyWage; }
            set { hourlyWage = value; ValueChanged?.Invoke(); }
        }

        /// <summary>
        /// Produktionszeit pro Stück in Stunden
        /// </summary>
        public decimal ProductionTime
        {
            get { return productionTime; }
            set { productionTime = value; ValueChanged?.Invoke(); }
        }

        /// <summary>
        /// Fertigungseinzelkosten
        /// </summary>
        public decimal ProductWages => hourlyWage * productionTime;

        /// <summary>
        /// Anfallende Gesamtkosten auf der Kostenstelle Produktion
        /// </summary>
        public decimal ProductOverHeadCostCentersAmount
        {
            get { return productOverHeadCostCentersAmount; }
            set { productOverHeadCostCentersAmount = value; }
        }

        /// <summary>
        /// Fertigungsgemeinkosten (%)
        /// </summary>
        public decimal ProductOverheads
        {
            get
            {
                if (ItemAmountPerAnno > 0 && ProductWages > 0)
                    return productOverHeadCostCentersAmount / (itemAmountPerAnno * ProductWages);
                else
                    return 0;
            }
        }

        /// <summary>
        /// Fertigungsgemeinkosten
        /// </summary>
        public decimal ProductOverheadsValue => ProductWages * ProductOverheads;

        /// <summary>
        /// Fertigungskosten
        /// </summary>
        public decimal ManufacturingCosts => ProductWages + ProductOverheadsValue;

        #endregion Fertigungskosten

        #region Herstellkosten

        /// <summary>
        /// Herstellkosten
        /// </summary>
        public decimal ProductionCosts => MaterialCosts + ManufacturingCosts;

        #endregion Herstellkosten

        #region Selbstkosten

        /// <summary>
        /// Anfallende Gesamtkosten auf der Kostenstelle Verwaltung
        /// </summary>
        public decimal AdministrativeOverHeadCostCentersAmount
        {
            get { return administrativeOverHeadCostCentersAmount; }
            set { administrativeOverHeadCostCentersAmount = value; }
        }

        /// <summary>
        /// Verwaltungsgemeinkosten (%)
        /// </summary>
        public decimal AdministrativeOverheads
        {
            get
            {
                if (itemAmountPerAnno > 0 && ProductionCosts > 0)
                    return administrativeOverHeadCostCentersAmount / (itemAmountPerAnno * ProductionCosts);
                else
                    return 0;
            }
        }

        /// <summary>
        /// Verwaltungsgemeinkosten
        /// </summary>
        public decimal AdministrativeOverheadsValue => ProductionCosts * AdministrativeOverheads;

        /// <summary>
        /// Anfallende Gesamtkosten auf der Kostenstelle Vertrieb
        /// </summary>
        public decimal SalesOverHeadCostCentersAmount
        {
            get { return salesOverHeadCostCentersAmount; }
            set { salesOverHeadCostCentersAmount = value; }
        }

        /// <summary>
        /// Vertriebsgemeinkosten (%)
        /// </summary>
        public decimal SalesOverheads
        {
            get
            {
                if (itemAmountPerAnno > 0 && ProductionCosts > 0)
                    return salesOverHeadCostCentersAmount / (itemAmountPerAnno * ProductionCosts);
                else
                    return 0;
            }
        }

        /// <summary>
        /// Vertriebsgemeinkosten
        /// </summary>
        public decimal SalesOverheadsValue => ProductionCosts * SalesOverheads;

        /// <summary>
        /// Selbstkosten
        /// </summary>
        public decimal CostPrice => ProductionCosts + AdministrativeOverheadsValue + SalesOverheadsValue;

        #endregion Selbstkosten

        #region Barverkaufspreis
        
        /// <summary>
        /// Gewinnzuschlag (%)
        /// </summary>
        public decimal ProfitSurcharge
        {
            get { return profitSurcharge; }
            set { profitSurcharge = value; ValueChanged?.Invoke(); }
        }

        /// <summary>
        /// Der Gewinn
        /// </summary>
        public decimal ProfitSurchargeValue => CostPrice* (profitSurcharge / 100);

        /// <summary>
        /// Barverkaufspreis
        /// </summary>
        public decimal CashSellingPrice => ProfitSurchargeValue + CostPrice;

        #endregion Barverkaufspreis

        #region Zielverkaufspreis

        /// <summary>
        /// Kundenskonto (%)
        /// </summary>
        public decimal CustomerCashback
        {
            get { return customerCashback; }
            set { customerCashback = value; ValueChanged?.Invoke(); }
        }

        /// <summary>
        /// Kundenskonto
        /// </summary>
        public decimal CustomerCashbackValue => CashSellingPrice * (customerCashback / 100);

        /// <summary>
        /// Vertreterprovision (%)
        /// </summary>
        public decimal AgentCommission
        {
            get { return agentCommission; }
            set { agentCommission = value; ValueChanged?.Invoke(); }
        }

        /// <summary>
        /// Vertreterposition
        /// </summary>
        public decimal AgentCommissionValue => CashSellingPrice * (agentCommission / 100);

        public decimal TargetSalesPrice => CashSellingPrice + CustomerCashbackValue + AgentCommissionValue;

        #endregion Zielverkaufspreis

        #region Angebotspreis

        /// <summary>
        /// Kundenrabatt (%)
        /// </summary>
        public decimal CustomerDiscount
        {
            get { return customerDiscount; }
            set { customerDiscount = value; ValueChanged?.Invoke(); }
        }

        /// <summary>
        /// Kundenrabatt
        /// </summary>
        public decimal CustomerDiscountValue => TargetSalesPrice * (customerDiscount / 100);

        /// <summary>
        /// Angebotspreis
        /// </summary>
        public decimal OfferPrice => TargetSalesPrice + CustomerDiscountValue;

        #endregion Angebotspreis

        #region Bruttoverkaufspreis

        /// <summary>
        /// Umsatzsteuer (%)
        /// </summary>
        public decimal Tax
        {
            get { return tax; }
            set { tax = value; ValueChanged?.Invoke(); }
        }

        /// <summary>
        /// Steuerbetrag
        /// </summary>
        public decimal TaxValue => OfferPrice * (tax / 100);

        /// <summary>
        /// Bruttoverkaufspreis
        /// </summary>
        public decimal GrossSellingPrice => OfferPrice + TaxValue;

        #endregion Bruttoverkaufspreis

        #endregion Properties

    }
}