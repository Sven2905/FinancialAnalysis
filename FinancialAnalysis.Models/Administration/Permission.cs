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
        AccessBookings = 2,
        AccessPaymentCondiditions = 3,
        AccessCostAccounts = 4,
        AccessCostCenters = 5,
        AccessCreditorDebitors = 6,
        AccessTaxTypes = 7,
        AccessBookingHistory = 8,
        AccessCostCenterCategories = 9,

        // Configuration 100 - 199
        AccessConfiguration = 100,
        AccessMail = 101,
        AccessUsers = 102,
        AccessMyCompanies = 103,

        // ProjectManagement 200 - 299
        AccessProjectManagement = 200,
        AccessEmployees = 201,
        AccessProjects = 202,
        AccessProjectWorkingTimes = 203,

        // WarehouseManagement 300 - 399
        AccessWarehouseManagement = 300,
        AccessWarehouses = 301,
        AccessWarehouseSave = 302,
        AccessWarehouseDelete = 303,
        AccessStockyards = 304,


        // PurchaseManagement 400 - 499
        AccessPurchaseManagement = 400,
        AccessBills = 401,
        AccessBillTypes = 402,
        AccessGoodsReceivedNotes = 403,
        AccessPurchaseOrders = 404,
        AccessPurchaseTypes = 405,

        // SaleManagement 500 - 599
        AccessSalesManagement = 500,
        AccessInvoices = 501,
        AccessInvoiceTypes = 502,
        AccessShipments = 503,
        AccessShipmentTypes = 504,
        AccessSalesOrders = 505,
        AccessSalesTypes = 506,
        AccessPendingSaleOrders = 507,


        // ProductManagement 600 - 699
        AccessProductManagement = 600,
        AccessProducts = 601,
        AccessProductCategories = 602,


    }
}
