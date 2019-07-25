namespace Formulas.PriceCalculationMethods
{
    public static class ItemPriceCalculation
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

        public static ItemPriceCalculationOutputItem CalculatePriceForItem(ItemPriceCalculationInputItem itemPriceCalculationInputItem)
        {
            ItemPriceCalculationOutputItem itemPriceCalculationOutputItem = new ItemPriceCalculationOutputItem();

            itemPriceCalculationOutputItem.ProductionMaterial = itemPriceCalculationInputItem.ProductionMaterial;
            itemPriceCalculationOutputItem.MaterialOverheadCosts = itemPriceCalculationInputItem.ProductionMaterial * itemPriceCalculationInputItem.MaterialOverheadCosts;

            itemPriceCalculationOutputItem.ProductWages = itemPriceCalculationInputItem.ProductWages;
            itemPriceCalculationOutputItem.ProductOverheads = itemPriceCalculationInputItem.ProductWages * itemPriceCalculationInputItem.ProductOverheads;

            itemPriceCalculationOutputItem.AdministrativeOverheads = itemPriceCalculationOutputItem.ProductionCosts * itemPriceCalculationInputItem.AdministrativeOverheads;
            itemPriceCalculationOutputItem.SalesOverheads = itemPriceCalculationOutputItem.ProductionCosts * itemPriceCalculationInputItem.SalesOverheads;

            itemPriceCalculationOutputItem.ProfitSurcharge = itemPriceCalculationOutputItem.CostPrice * (itemPriceCalculationInputItem.ProfitSurcharge / 100);

            itemPriceCalculationOutputItem.CustomerCashback = itemPriceCalculationOutputItem.CashSellingPrice * (itemPriceCalculationInputItem.CustomerCashback / 100);
            itemPriceCalculationOutputItem.AgentCommission = itemPriceCalculationOutputItem.CashSellingPrice * (itemPriceCalculationInputItem.AgentCommission / 100);

            itemPriceCalculationOutputItem.CustomerDiscount = itemPriceCalculationOutputItem.TargetSalesPrice * (itemPriceCalculationInputItem.CustomerDiscount / 100);

            itemPriceCalculationOutputItem.Tax = itemPriceCalculationOutputItem.OfferPrice * (itemPriceCalculationInputItem.Tax / 100);

            return itemPriceCalculationOutputItem;
        }
    }
}