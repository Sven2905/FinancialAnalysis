using FinancialAnalysis.Datalayer;
using FinancialAnalysis.Logic.Messages;
using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Accounting;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Utilities;

namespace FinancialAnalysis.Logic.Model.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        public List<Company> Data { get; set; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}

            Company c = new Company()
            {
                Name = "Sven Technology",
                Street = "Horster Str. 115",
                City = "Gelsenkirchen",
                Postcode = 45899,
                BankName = "Sparkasse",
                BIC = "123456789",
                IBAN = "DE22123456789",
                ContactPerson = "Sven Fuhrmann",
                Fax = "00000",
                Phone = "1337 - 42",
                FederalState = FederalState.NW, 
                eMail = "sven@sven.tech",
                TaxNumber = "1597532468",
                UStID = "9513578642",
                Website = "sven.tech"
            };

            DataLayer db = new DataLayer(true);
            //db.TaxTypes.Seed();

            Import _Import = new Import();
            _Import.ImportCostAccounts(Standardkontenrahmen.SKR04);

            OpenKontenrahmenCommand = new RelayCommand(() =>
            {
                MessengerInstance.Send(new OpenKontenrahmenWindowMessage() { AccountingType = AccountingType.Credit });
            });

            CloseCommand = new RelayCommand(() =>
            {
                CloseAction();
            });
        }

        public RelayCommand OpenKontenrahmenCommand { get; }
        public RelayCommand CloseCommand { get; }

        public Action CloseAction { get; set; }

    }
}