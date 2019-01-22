using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialAnalysis.Models.Administration
{
    public enum Permission
    {
        // Accounting 1 - 99
        AccessAccounting = 1,
        AccessBooking = 2,
        AccessPaymentCondidition = 3,
        AccessCostAccount = 4,
        AccessCostCenter = 5,
        AccessCreditorDebitor = 6,
        AccessTaxType = 7,
        AccessBookingHistory = 8,

        // Configuration 100 - 199
        AccessConfiguration = 100,
        AccessMail = 101,
        AccessUsers = 102,

        // ProjectManagement 200 - 299
        AccessProjectManagement = 200,
        AccessEmployee = 201,
        AccessProject = 202,
        AccessProjectWorkingTime = 203,

        // StockManagement 300 - 399
        AccessStockManagement = 300,

    }
}
