namespace FinancialAnalysis.Models.Administration
{
    public enum Permission
    {
        // Accounting 1 - 99
        Accounting = 1,

        AccountingBookings = 2,
        AccountingPaymentCondiditions = 3,
        AccountingCostAccounts = 4,
        AccountingCostCenters = 5,
        AccountingCreditorDebitors = 6,
        AccountingTaxTypes = 7,
        AccountingBookingHistory = 8,
        AccountingCostCenterCategories = 9,
        AccountingBalances = 10,
        AccountingBookingHistories = 11,
        AccountingDepreciations = 12,
        AccountingFixedCostAccounts = 13,

        // Configuration 100 - 199
        Configuration = 100,

        ConfigurationMail = 101,
        ConfigurationUsers = 102,
        ConfigurationMyCompanies = 103,

        // ProjectManagement 200 - 299
        ProjectManagement = 200,

        ProjectManagementUsers = 201,
        ProjectManagementProjects = 202,
        ProjectManagementProjectWorkingTimes = 203,

        // WarehouseManagement 300 - 399
        WarehouseManagement = 300,

        Warehouses = 301,
        WarehouseSave = 302,
        WarehouseDelete = 303,
        WarehouseStockyards = 304,
        WarehouseStocking = 305,
        WarehouseStockingStore = 306,
        WarehouseStockingTakeOut = 307,

        // PurchaseManagement 400 - 499
        //AccessPurchaseManagement = 400,
        //AccessBills = 401,
        //AccessBillTypes = 402,
        //AccessGoodsReceivedNotes = 403,
        //AccessPurchaseOrders = 404,
        //AccessPurchaseTypes = 405,

        // SaleManagement 500 - 599
        SalesManagement = 500,

        SalesInvoices = 501,
        SalesInvoiceTypes = 502,
        SalesShipments = 503,
        SalesShipmentTypes = 504,
        SalesOrders = 505,
        SalesTypes = 506,
        SalesPendingSaleOrders = 507,

        // ProductManagement 600 - 699
        ProductManagement = 600,

        Products = 601,
        ProductCategories = 602,

        // TimeManagement 700 - 799
        TimeManagement = 700,

        TimeBooking = 701,
        TimeBookingForOthers = 702,
        TimeHolidayRequest = 703,
        TimeHolidayRequestForOthers = 704,

        CarPoolManagement = 800
    }
}