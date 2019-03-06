﻿using DevExpress.Mvvm;
using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Logic.SalesManagement;
using FinancialAnalysis.Models.SalesManagement;
using System;
using System.Linq;
using System.Windows.Input;
using Utilities;

namespace FinancialAnalysis.Logic.ViewModels
{
    public class PendingSaleOrdersViewModel : ViewModelBase
    {
        #region Contructor

        public PendingSaleOrdersViewModel()
        {
            if (IsInDesignMode)
            {
                return;
            }

            SetCommands();
            RefreshData();
        }

        #endregion Contructor

        #region Fields

        private SalesOrder _SelectedSalesOrder;
        private string _FilterText;

        #endregion Fields

        #region Methods

        private void SetCommands()
        {
            CloseOrderCommand = new DelegateCommand(CloseOrder, () => SelectedSalesOrder != null);
            CreateInvoiceWindowCommand = new DelegateCommand(CreateInvoice, () => SelectedSalesOrder != null);
            ShowPDFOrderReportCommand = new DelegateCommand(ShowPDFOrderReport, () => SelectedSalesOrder != null);
            OpenInvoiceListCommand = new DelegateCommand(ShowInvoices, () => SelectedSalesOrder != null && SelectedSalesOrder.Invoices.Count > 0);
        }

        private void CloseOrder()
        {
            SelectedSalesOrder.IsClosed = true;
            DataContext.Instance.SalesOrders.Update(SelectedSalesOrder);
            RefreshData();
            SelectedSalesOrder = null;
        }

        private void ShowPDFOrderReport()
        {
            var salesOrderReportData = new SalesOrderReportData
            {
                MyCompany = Globals.CoreData.MyCompany,
                SalesOrder = SelectedSalesOrder,
                Employee = SelectedSalesOrder.Employee
            };

            SalesReportPDFCreator.CreateAndShowOrderReport(salesOrderReportData, false);
        }

        private decimal CalculateOutstandingInvoiceAmount()
        {
            if (SelectedSalesOrder != null)
            {
                return SelectedSalesOrder.SumTotal - SelectedSalesOrder.Invoices.Sum(x => x.PaidAmount);
            }
            else
            {
                return 0;
            }
        }

        private void RefreshData()
        {
            FilteredSalesOrders = SalesOrders = DataContext.Instance.SalesOrders.GetOpenedSalesOrders().ToSvenTechCollection();
            CheckOrdersStatus();
            var invoices = DataContext.Instance.Invoices.GetAll();
        }

        private void CountInvoicesForLabel()
        {
            if(SelectedSalesOrder != null && OpenInvoices != null)
                InvoicesCount = $" {SelectedSalesOrder.Invoices.Count} ({OpenInvoices.Count})";
        }

        private void CheckOrdersStatus()
        {
            foreach (var order in SalesOrders)
            {
                order.CheckStatus();
            }
        }

        private void CreateInvoice()
        {
            var invoiceSalesOrder = SelectedSalesOrder.Clone();

            foreach (var invoice in SelectedSalesOrder.Invoices)
            {
                foreach (var invoicePosition in invoice.InvoicePositions)
                {
                    invoiceSalesOrder.SalesOrderPositions.SingleOrDefault(x => x.SalesOrderPositionId == invoicePosition.RefSalesOrderPositionId).Quantity -= invoicePosition.Quantity;
                }
            }

            for (int i = invoiceSalesOrder.SalesOrderPositions.Count - 1; i >= 0; i--)
            {
                if (invoiceSalesOrder.SalesOrderPositions[i].Quantity == 0)
                {
                    invoiceSalesOrder.SalesOrderPositions.RemoveAt(i);
                }
            }

            Messenger.Default.Send(new OpenInvoiceCreationWindowMessage(invoiceSalesOrder));
        }

        private void ShowInvoices()
        {
            Messenger.Default.Send(new OpenInvoiceListWindowMessage(SelectedSalesOrder.Invoices));
        }

        #endregion Methods

        #region Properties

        public SvenTechCollection<SalesOrder> SalesOrders { get; set; }
        public SvenTechCollection<SalesOrder> FilteredSalesOrders { get; set; }
        public ICommand CloseOrderCommand { get; set; }
        public ICommand CreateInvoiceWindowCommand { get; set; }
        public ICommand ShowPDFOrderReportCommand { get; set; }
        public ICommand OpenInvoiceListCommand { get; set; }
        public decimal OutstandingInvoiceAmount => CalculateOutstandingInvoiceAmount();
        public string InvoicesCount { get; set; } = "0";
        public SvenTechCollection<Invoice> OpenInvoices => SelectedSalesOrder.Invoices.Where(x => !x.IsPaid).ToSvenTechCollection();

        public string FilterText
        {
            get { return _FilterText; }
            set
            {
                _FilterText = value;
                if (!string.IsNullOrEmpty(_FilterText))
                {
                    FilteredSalesOrders = SalesOrders.Where(x => x.SalesOrderId.ToString().Contains(_FilterText) || x.Debitor.Client.Name.IndexOf(_FilterText, StringComparison.OrdinalIgnoreCase) >= 0).ToSvenTechCollection();
                }
                else
                {
                    FilteredSalesOrders = SalesOrders;
                }
            }
        }

        public SalesOrder SelectedSalesOrder
        {
            get { return _SelectedSalesOrder; }
            set { _SelectedSalesOrder = value; CountInvoicesForLabel(); }
        }

        #endregion Properties
    }
}