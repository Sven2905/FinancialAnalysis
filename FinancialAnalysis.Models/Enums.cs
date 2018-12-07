using System.ComponentModel.DataAnnotations;

namespace FinancialAnalysis.Models
{
    public enum AccountingType
    {
        Credit,
        Debit
    }

    public enum Gender
    {
        [Display(Name = "Männlich")] male,
        [Display(Name = "Weiblich")] female
    }

    public enum CivilStatus
    {
        [Display(Name = "Verheiratet")] Married,
        [Display(Name = "Ledig")] Single,
        [Display(Name = "Geschieden")] Divorced,
        [Display(Name = "Verwitwed")] Widowed
    }

    public enum Standardkontenrahmen
    {
        SKR03,
        SKR04
    }

    public enum GrossNetType
    {
        Brutto,
        Netto
    }

    public enum PayType
    {
        Intervall,
        [Display(Name = "Aktueller Monat")] ThisMonth,

        [Display(Name = "Nachfolgender Monat")]
        NextMonth,
        [Display(Name = "Bestimmter Tag")] NextSpecificDay
    }

    /// <summary>
    ///     Enum with Federal States of Germany (ISO 3166-2:DE)
    ///     <para />
    ///     BW = Baden-Württemberg,
    ///     <para />
    ///     BY = Bayern,
    ///     <para />
    ///     BE = Berlin,
    ///     <para />
    ///     BB = Brandenburg,
    ///     <para />
    ///     HB = Bremen,
    ///     <para />
    ///     HH = Hamburg,
    ///     <para />
    ///     HE = Hessen,
    ///     <para />
    ///     MV = Mecklenburg-Vorpommern,
    ///     <para />
    ///     NI = Niedersachsen,
    ///     <para />
    ///     NW = Nordrhein-Westfalen,
    ///     <para />
    ///     RP = Rheinland-Pfalz,
    ///     <para />
    ///     SL = Saarland,
    ///     <para />
    ///     SN = Sachsen,
    ///     <para />
    ///     ST = Sachsen-Anhalt,
    ///     <para />
    ///     SH = Schleswig-Holstein,
    ///     <para />
    ///     TH = Thüringen
    /// </summary>
    public enum FederalState
    {
        [Display(Name = "Baden-Württemberg")] BW,
        [Display(Name = "Bayern")] BY,
        [Display(Name = "Berlin")] BE,
        [Display(Name = "Brandenburg")] BB,
        [Display(Name = "Bremen")] HB,
        [Display(Name = "Hamburg")] HH,
        [Display(Name = "Hessen")] HE,

        [Display(Name = "Mecklenburg-Vorpommern")]
        MV,
        [Display(Name = "Niedersachsen")] NI,

        [Display(Name = "Nordrhein-Westfalen")]
        NW,
        [Display(Name = "Rheinland-Pfalz")] RP,
        [Display(Name = "Saarland")] SL,
        [Display(Name = "Sachsen")] SN,
        [Display(Name = "Sachsen-Anhalt")] ST,
        [Display(Name = "Schleswig-Holstein")] SH,
        [Display(Name = "Thüringen")] TH
    }

    public enum TaxCategory
    {
        [Display(Name = "Keine")] None,
        [Display(Name = "Netto")] Netto,
        [Display(Name = "Brutto")] Brutto,
        [Display(Name = "i.g.E")] igE,
        [Display(Name = "13b")] thirteenB,
        [Display(Name = "50%")] fiftyPercent
    }

    public enum CostAccountType
    {
        [Display(Name = "Neutral")] Neutral,
        [Display(Name = "Einnahmen")] Gain,
        [Display(Name = "Ausgaben")] Loss
    }
}