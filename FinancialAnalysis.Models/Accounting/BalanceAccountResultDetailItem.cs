using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Accounting
{
    public class BalanceAccountResultDetailItem : BalanceAccountResultItem
    {
        public string DateString { get; set; }
        public DateTime Date => ParseDateString();
        public string Description { get; set; }
        public string MainCategoryName { get; set; }
        public string Sub1CategoryName { get; set; }

        public DateTime ParseDateString()
        {
            if (string.IsNullOrEmpty(DateString))
                return DateTime.Now;

            var dateArray = DateString.Split('-');

            if (dateArray.Length < 2)
                return DateTime.Now;

            return new DateTime(Convert.ToInt32(dateArray[1]), Convert.ToInt32(dateArray[0]), 1);
        }
    }
}
