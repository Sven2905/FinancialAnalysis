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
        AccessMyCompany = 103,

        // ProjectManagement 200 - 299
        AccessProjectManagement = 200,
        AccessEmployee = 201,
        AccessProject = 202,
        AccessProjectWorkingTime = 203,

        // WarehouseManagement 300 - 399
        AccessWarehouseManagement = 300,
        AccessWarehouse = 301,
        AccessWarehouseSave = 302,
        AccessWarehouseDelete = 303,
        AccessStockyard = 304,


        // PurchaseManagement 400 - 499
        AccessPurchaseManagement = 400,
        AccessBills = 401,
        AccessBillTypes = 402,
        AccessGoodsReceivedNotes = 403,
        AccessPurchaseOrders = 404,
        AccessPurchaseTypes = 405,

        // SaleManagement 500 - 599
        AccessSalesManagement = 500,
        AccessInvoice = 501,
        AccessInvoiceTypes = 502,
        AccessShipment = 503,
        AccessShipmentType = 504,
        AccessSalesOrders = 505,
        AccessSalesTypes = 506,
        AccessPendingSaleOrders = 507,


        // ProductManagement 600 - 699
        AccessProductManagement = 600,
        AccessProducts = 601,
        AccessProductCategories = 602,


    }
}
