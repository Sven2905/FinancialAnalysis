﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using FinancialAnalysis.Datalayer.StoredProcedures;
using FinancialAnalysis.Models;
using Serilog;

namespace FinancialAnalysis.Datalayer.Tables
{
    public class TableVersions : ITable
    {
        private readonly TableVersionsStoredProcedures sp = new TableVersionsStoredProcedures();

        public TableVersions()
        {
            TableName = "TableVersions";
            CheckAndCreateTable();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs\\Tables.txt", rollingInterval: RollingInterval.Month)
                .CreateLogger();
        }

        public string TableName { get; }

        public void CheckAndCreateStoredProcedures()
        {
            sp.CheckAndCreateProcedures();
        }

        /// <summary>
        ///     Checks if table exists otherwise creates it
        /// </summary>
        public void CheckAndCreateTable()
        {
            try
            {
                var con = new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB));
                var commandStr =
                    $"If not exists (select name from sysobjects where name = '{TableName}') CREATE TABLE {TableName}(TableVersionId int IDENTITY(1,1) PRIMARY KEY,Name char(50) NOT NULL,Version int NOT NULL,LastModified datetime NOT NULL)";

                using (var command = new SqlCommand(commandStr, con))
                {
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while creating table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Returns all TableVersion records
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TableVersion> GetAll()
        {
            IEnumerable<TableVersion> output = new List<TableVersion>();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.Query<TableVersion>($"dbo.{TableName}_GetAll");
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetAll' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Returns TaxType by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TableVersion GetById(int id)
        {
            var output = new TableVersion();
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    output = con.QuerySingleOrDefault<TableVersion>($"dbo.{TableName}_GetById @TableVersionId",
                        new {TableVersionId = id});
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'GetById' from table '{TableName}'", e);
            }

            return output;
        }

        /// <summary>
        ///     Inserts the list of TableVersion items
        /// </summary>
        /// <param name="tableVersions"></param>
        public void Insert(TableVersion tableVersion)
        {
            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute($"dbo.{TableName}_Insert @Name, @Version, @LastModified", tableVersion);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Insert item' from table '{TableName}'", e);
            }
        }

        /// <summary>
        ///     Update TaxType, if not exist, insert it
        /// </summary>
        /// <param name="tableVersion"></param>
        public void UpdateOrInsert(TableVersion tableVersion)
        {
            if (tableVersion.Id == 0 || GetById(tableVersion.Id) is null)
            {
                Insert(tableVersion);
                return;
            }

            Update(tableVersion);
        }

        /// <summary>
        ///     Update TaxType
        /// </summary>
        /// <param name="tableVersion"></param>
        public void Update(TableVersion tableVersion)
        {
            if (tableVersion.Id == 0 || GetById(tableVersion.Id) is null) return;

            try
            {
                using (IDbConnection con =
                    new SqlConnection(Helper.GetConnectionString(DatabaseNames.FinancialAnalysisDB)))
                {
                    con.Execute(
                        $"dbo.{TableName}_Update @TableVersionId, @Description, @DescriptionShort, @AmountOfTax, @TaxCategory, @RefAccountNumber, @RefAccountNotPayable",
                        tableVersion);
                }
            }
            catch (Exception e)
            {
                Log.Error($"Exception occured while 'Update' from table '{TableName}'", e);
            }
        }
    }
}