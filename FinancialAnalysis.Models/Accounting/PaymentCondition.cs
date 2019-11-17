using DevExpress.Mvvm;
using Newtonsoft.Json;
using System;

namespace FinancialAnalysis.Models.Accounting
{
    /// <summary>
    /// Zahlungskondition
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class PaymentCondition : BaseClass
    {
        /// <summary>
        /// Id
        /// </summary>
        public int PaymentConditionId { get; set; }

        /// <summary>
        /// Name der Zahlungskondition
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Prozentsatz
        /// </summary>
        public decimal Percentage { get; set; }

        /// <summary>
        /// Wert abhängig von der Wahl der Zahlungstyps
        /// </summary>
        public int TimeValue { get; set; }

        /// <summary>
        /// Zahlungstyp
        /// </summary>
        public PayType PayType { get; set; }

        /// <summary>
        /// Überprüft ob die Zahlungskonditionen eingehalten wurden
        /// </summary>
        /// <param name="dueDate">Fälligkeitsdatum</param>
        /// <param name="payDate">Zahlungsdatum</param>
        /// <returns></returns>
        public bool CheckIfAdhered(DateTime dueDate, DateTime payDate)
        {
            bool result = false;

            switch (PayType)
            {
                case PayType.Intervall:
                    if (payDate.AddDays(TimeValue) <= dueDate)
                    {
                        result = true;
                    }
                    break;

                case PayType.ThisMonth:
                    if (payDate <= new DateTime(dueDate.Year, dueDate.Month, TimeValue))
                    {
                        result = true;
                    }
                    break;

                case PayType.NextMonth:
                    if (payDate <= new DateTime(dueDate.Year, dueDate.Month + 1, TimeValue))
                    {
                        result = true;
                    }
                    break;

                default:
                    result = false;
                    break;
            }

            return result;
        }
    }
}