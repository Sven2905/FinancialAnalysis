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
        AccessBooking = 1,
        AccessPaymentCondidition = 2,
        AccessCostAccount = 3,
        AccessCostCenter = 4,
        AccessCreditorDebitor = 5,
        AccessTaxType = 6,
        AccessBookingHistory = 7,

        // Configuration 100 - 199
        AccessConfiguration = 100,
        AccessMail = 101,
        AccessUsers = 102,

        // ProjectManagement 200 - 299
        AccessProjectManagement = 200,
        AccessEmployee = 201,
        AccessProject = 202,
        AccessProjectWorkingTime = 203,

    }
}
