﻿using System;
using DevExpress.Mvvm;
using FinancialAnalysis.Models.Enums;
using FinancialAnalysis.Models.Interfaces;
using Newtonsoft.Json;
using Utilities;

namespace FinancialAnalysis.Models.Accounting
{
    /// <summary>
    /// Bilanz-Posten
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class BalanceAccount : BaseClass
    {
        public BalanceAccount()
        {
        }

        public BalanceAccount(int BalanceAccountId, string Name, AccountType AccountType, int ParentId = 0, bool IsDeletable = true, bool IsEditable = true, int CreatedBy = 0)
        {
            this.BalanceAccountId = BalanceAccountId;
            this.Name = Name;
            this.ParentId = ParentId;
            this.AccountType = AccountType;
            this.IsDeletable = IsDeletable;
            this.IsEditable = IsEditable;
            this.CreatedBy = CreatedBy;
        }

        /// <summary>
        /// Id
        /// </summary>
        public int BalanceAccountId { get; set; }

        /// <summary>
        /// Name des Postens
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Referenz-Id des übergeordneten Postens
        /// </summary>
        public int ParentId { get; set; } = 0;

        /// <summary>
        /// Aktiv- oder Passivkonto
        /// </summary>
        public AccountType AccountType { get; set; }

        /// <summary>
        /// Zugeordnete Kontenrahmen
        /// </summary>
        public SvenTechCollection<CostAccount> CostAccounts { get; set; } = new SvenTechCollection<CostAccount>();
    }
}