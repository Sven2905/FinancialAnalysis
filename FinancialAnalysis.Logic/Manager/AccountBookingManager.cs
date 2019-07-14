using FinancialAnalysis.Models;
using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiWrapper.Accounting;

namespace FinancialAnalysis.Logic.Manager
{
    public class AccountBookingManager
    {
        private int tempCreditDebitId = 1;
        private int tempBookingId = 1;

        public static AccountBookingManager Instance { get; } = new AccountBookingManager();

        public ObservableCollection<Booking> BookingList { get; set; } = new ObservableCollection<Booking>();
        public Booking SelectedBooking { get; set; }

        /// <summary>
        /// Creates a new booking item, selects it and adds it to the BookingList
        /// </summary>
        /// <param name="date"></param>
        /// <param name="description"></param>
        public void NewBookingItem(DateTime date, string description)
        {
            if (string.IsNullOrEmpty(description))
                throw new Exception("Bitte Kontenrahmen angeben.");

            Booking booking = new Booking(date, description);
            booking.BookingId = tempBookingId;
            tempBookingId++;
            SelectedBooking = booking;

            if (BookingList == null)
                BookingList = new ObservableCollection<Booking>();
            BookingList.Add(booking);
        }

        /// <summary>
        /// Adds debits and credits to the SelectedBooking item
        /// </summary>
        /// <param name="credits"></param>
        /// <param name="debits"></param>
        public void AddCreditsAndDebits(List<Credit> credits, List<Debit> debits)
        {
            if (credits == null || debits == null
                || credits.Count == 0 || debits.Count == 0
                || credits.Sum(x => x.Amount) != debits.Sum(x => x.Amount)
                || credits.Any(x => x.RefCostAccountId == 0) || debits.Any(x => x.RefCostAccountId == 0))
            {
                throw new ArgumentException();
            }

            AddCredits(credits);
            AddDebits(debits);
        }

        private void AddCredits(IEnumerable<Credit> credits)
        {
            if (SelectedBooking != null)
                SelectedBooking.Credits.AddRange(credits);
        }

        private void AddDebits(IEnumerable<Debit> debits)
        {
            if (SelectedBooking != null)
                SelectedBooking.Debits.AddRange(debits);
        }

        /// <summary>
        /// Creates credits and debits and add them to the SelectedBooking item
        /// </summary>
        /// <param name="grossNetType"></param>
        /// <param name="bookingType"></param>
        /// <param name="amount"></param>
        /// <param name="costAccountCreditor"></param>
        /// <param name="costAccountDebitor"></param>
        /// <param name="taxType"></param>
        /// <returns></returns>
        public bool CreateAndAddCreditDebit(GrossNetType grossNetType, BookingType bookingType, decimal amount, CostAccount costAccountCreditor, CostAccount costAccountDebitor, TaxType taxType)
        {
            if (costAccountCreditor == null || costAccountDebitor == null || taxType == null)
                return false;

            CalculateTax(grossNetType, taxType, amount, out decimal tax, out decimal amountWithoutTax);

            Debit debit = new Debit();
            Credit credit = new Credit();

            TaxType nonTax = TaxTypes.GetById(1);

            if (bookingType == BookingType.Invoice)
            {
                if (taxType.Description.IndexOf("Vorsteuer", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    SelectedBooking.Credits.AddRange(CreateCredits(grossNetType, nonTax, tax + amountWithoutTax, costAccountCreditor));
                    SelectedBooking.Debits.AddRange(CreateDebits(grossNetType, taxType, amount, costAccountDebitor));
                }
                else if (taxType.Description.IndexOf("Umsatzsteuer", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    SelectedBooking.Credits.AddRange(CreateCredits(grossNetType, taxType, amount, costAccountCreditor));
                    SelectedBooking.Debits.AddRange(CreateDebits(grossNetType, nonTax, tax + amountWithoutTax, costAccountDebitor));
                }
                else
                {
                    SelectedBooking.Credits.AddRange(CreateCredits(grossNetType, nonTax, amount, costAccountCreditor));
                    SelectedBooking.Debits.AddRange(CreateDebits(grossNetType, nonTax, amount, costAccountDebitor));
                }
            }
            else if (bookingType == BookingType.CreditAdvice)
            {
                // ToDo Check with Tobias !!!

                if (taxType.Description.IndexOf("Vorsteuer", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    SelectedBooking.Credits.AddRange(CreateCredits(grossNetType, nonTax, amount * (-1), costAccountCreditor));
                    SelectedBooking.Debits.AddRange(CreateDebits(grossNetType, taxType, amount, costAccountDebitor));
                }
                else if (taxType.Description.IndexOf("Umsatzsteuer", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    SelectedBooking.Credits.AddRange(CreateCredits(grossNetType, taxType, amount, costAccountCreditor));
                    SelectedBooking.Debits.AddRange(CreateDebits(grossNetType, nonTax, amount * (-1), costAccountDebitor));
                }
                else
                {
                    SelectedBooking.Credits.AddRange(CreateCredits(grossNetType, nonTax, amount, costAccountCreditor));
                    SelectedBooking.Debits.AddRange(CreateDebits(grossNetType, nonTax, amount * (-1), costAccountDebitor));
                }
            }

            return true;
        }

        public void CreateCreditDebit(GrossNetType grossNetType, BookingType bookingType, decimal amount, CostAccount costAccountCreditor, CostAccount costAccountDebitor, TaxType taxType, out List<Credit> credits, out List<Debit> debits)
        {
            credits = new List<Credit>();
            debits = new List<Debit>();

            if (costAccountCreditor == null || costAccountDebitor == null || taxType == null)
                return ;

            CalculateTax(grossNetType, taxType, amount, out decimal tax, out decimal amountWithoutTax);

            Debit debit = new Debit();
            Credit credit = new Credit();

            TaxType nonTax = TaxTypes.GetById(1);

            if (bookingType == BookingType.Invoice)
            {
                if (taxType.Description.IndexOf("Vorsteuer", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    credits.AddRange(CreateCredits(grossNetType, nonTax, tax + amountWithoutTax, costAccountCreditor));
                    debits.AddRange(CreateDebits(grossNetType, taxType, amount, costAccountDebitor));
                }
                else if (taxType.Description.IndexOf("Umsatzsteuer", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    credits.AddRange(CreateCredits(grossNetType, taxType, amount, costAccountCreditor));
                    debits.AddRange(CreateDebits(grossNetType, nonTax, tax + amountWithoutTax, costAccountDebitor));
                }
                else
                {
                    credits.AddRange(CreateCredits(grossNetType, nonTax, amount, costAccountCreditor));
                    debits.AddRange(CreateDebits(grossNetType, nonTax, amount, costAccountDebitor));
                }
            }
            else if (bookingType == BookingType.CreditAdvice)
            {
                // ToDo Check with Tobias !!!

                if (taxType.Description.IndexOf("Vorsteuer", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    credits.AddRange(CreateCredits(grossNetType, nonTax, amount * (-1), costAccountCreditor));
                    debits.AddRange(CreateDebits(grossNetType, taxType, amount, costAccountDebitor));
                }
                else if (taxType.Description.IndexOf("Umsatzsteuer", StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    credits.AddRange(CreateCredits(grossNetType, taxType, amount, costAccountCreditor));
                    debits.AddRange(CreateDebits(grossNetType, nonTax, amount * (-1), costAccountDebitor));
                }
                else
                {
                    credits.AddRange(CreateCredits(grossNetType, nonTax, amount, costAccountCreditor));
                    debits.AddRange(CreateDebits(grossNetType, nonTax, amount * (-1), costAccountDebitor));
                }
            }
        }

        /// <summary>
        /// Creates debit items and return them
        /// </summary>
        /// <param name="grossNetType"></param>
        /// <param name="taxType"></param>
        /// <param name="amount"></param>
        /// <param name="costAccount"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public List<Debit> CreateDebits(GrossNetType grossNetType, TaxType taxType, decimal amount, CostAccount costAccount, string description = "")
        {
            if (costAccount == null)
                throw new Exception("Bitte Kontenrahmen angeben.");

            List<Debit> debits = new List<Debit>();
            Debit debit = new Debit(amount, costAccount.CostAccountId, 0);
            debit.CostAccount = costAccount;

            CalculateTax(grossNetType, taxType, amount, out decimal tax, out decimal amountWithoutTax);

            debit.DebitId = tempCreditDebitId;

            if (tax > 0)
            {
                Debit debitTax = new Debit(tax, taxType.RefCostAccount, 0);
                debitTax.CostAccount = CostAccounts.GetById(taxType.RefCostAccount);
                debitTax.RefDebitId = tempCreditDebitId;
                debitTax.DebitId = ++tempCreditDebitId;
                debitTax.IsTax = true;
                debits.Add(debitTax);
            }

            debit.Amount = amountWithoutTax;
            debit.Description = description;
            debits.Add(debit);
            tempCreditDebitId++;

            return debits;
        }

        /// <summary>
        /// Creates credit items and return them
        /// </summary>
        /// <param name="grossNetType"></param>
        /// <param name="taxType"></param>
        /// <param name="amount"></param>
        /// <param name="costAccount"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        public List<Credit> CreateCredits(GrossNetType grossNetType, TaxType taxType, decimal amount, CostAccount costAccount, string description = "")
        {
            if (costAccount == null)
                throw new Exception("Bitte Kontenrahmen angeben.");

            List<Credit> credits = new List<Credit>();
            Credit credit = new Credit(amount, costAccount.CostAccountId, 0);
            credit.CostAccount = costAccount;

            CalculateTax(grossNetType, taxType, amount, out decimal tax, out decimal amountWithoutTax);

            credit.CreditId = tempCreditDebitId;

            if (tax > 0)
            {
                Credit creditTax = new Credit(tax, taxType.RefCostAccount, 0);
                creditTax.CostAccount = CostAccounts.GetById(taxType.RefCostAccount);
                creditTax.RefCreditId = tempCreditDebitId;
                creditTax.CreditId = ++tempCreditDebitId;
                creditTax.IsTax = true;
                credits.Add(creditTax);
            }

            credit.Amount = amountWithoutTax;
            credit.Description = description;
            credits.Add(credit);
            tempCreditDebitId++;

            return credits;
        }

        private void CalculateTax(GrossNetType grossNetType, TaxType taxType, decimal amount, out decimal tax, out decimal amountWithoutTax)
        {
            if (taxType != null && taxType.AmountOfTax > 0)
            {
                if (grossNetType == GrossNetType.Brutto)
                {
                    tax = amount / (100 + taxType.AmountOfTax) * taxType.AmountOfTax;
                    amountWithoutTax = amount - tax;
                }
                else
                {
                    tax = amount / 100 * taxType.AmountOfTax;
                    amountWithoutTax = amount;
                }
            }
            else
            {
                tax = 0;
                amountWithoutTax = amount;
            }
        }

        /// <summary>
        /// Adds ScannedDocuments to the SelectedBooking item
        /// </summary>
        /// <param name="scannedDocuments"></param>
        public void AddScannedDocuments(IEnumerable<ScannedDocument> scannedDocuments)
        {
            if (SelectedBooking == null || scannedDocuments == null)
                throw new ArgumentNullException();

            SelectedBooking.ScannedDocuments.AddRange(scannedDocuments);
        }

        /// <summary>
        /// Adds fixed allocation to the SelectedBooking item, removes single cost center
        /// </summary>
        /// <param name="fixedCostAllocation"></param>
        public void AddFixedCostAllocation(FixedCostAllocation fixedCostAllocation)
        {
            if (fixedCostAllocation == null)
                throw new Exception("Bitte gültigen Kostenstellenschlüssel angeben.");

            SelectedBooking.BookingCostCenterMappingList = new List<BookingCostCenterMapping>();
            SelectedBooking.RefFixedCostAllocationId = fixedCostAllocation.FixedCostAllocationId;
            foreach (FixedCostAllocationDetail item in fixedCostAllocation.FixedCostAllocationDetails)
            {
                SelectedBooking.BookingCostCenterMappingList.Add(new BookingCostCenterMapping(0, item.RefCostCenterId, SelectedBooking.AmountWithoutTax * (decimal)(item.Shares / fixedCostAllocation.Shares.Sum())));
            }
        }

        /// <summary>
        /// Adds a single cost center to the SelectedBooking item, removes fixed allocation
        /// </summary>
        /// <param name="costCenter"></param>
        public void AddCostCenter(CostCenter costCenter)
        {
            if (costCenter == null || costCenter.CostCenterId == 0)
            {
                throw new Exception("Bitte Kostenstelle angeben.");
            }

            SelectedBooking.RefFixedCostAllocationId = 0;
            SelectedBooking.BookingCostCenterMappingList.Add(new BookingCostCenterMapping(0, costCenter.CostCenterId, SelectedBooking.AmountWithoutTax));
        }

        /// <summary>
        /// Saves all bookings on the BookingList to the database
        /// </summary>
        public void SaveBookingsToDB()
        {
            foreach (var booking in BookingList)
            {
                int bookingId = Bookings.Insert(booking);

                foreach (var item in booking.Credits.OrderBy(x => x.CreditId))
                {
                    item.RefBookingId = bookingId;
                    var fakeId = item.CreditId;
                    item.CreditId = Credits.Insert(item);
                    foreach (var credit in booking.Credits)
                    {
                        if (credit.RefCreditId == fakeId)
                            credit.RefCreditId = item.CreditId;
                    }
                }

                foreach (var item in booking.Debits.OrderBy(x => x.DebitId))
                {
                    item.RefBookingId = bookingId;
                    var fakeId = item.DebitId;
                    item.DebitId = Debits.Insert(item);
                    foreach (var debit in booking.Debits)
                    {
                        if (debit.RefDebitId == fakeId)
                            debit.RefDebitId = item.DebitId;
                    }
                }

                foreach (ScannedDocument item in booking.ScannedDocuments)
                {
                    item.RefBookingId = bookingId;
                }
                ScannedDocuments.Insert(booking.ScannedDocuments);

                foreach (BookingCostCenterMapping item in booking.BookingCostCenterMappingList)
                {
                    item.RefBookingId = bookingId;
                }
                BookingCostCenterMappings.Insert(booking.BookingCostCenterMappingList);
            }

            BookingList.Clear();
            SelectedBooking = null;
        }

        /// <summary>
        /// Removes the booking from the BookingList
        /// </summary>
        /// <param name="id"></param>
        public void RemoveBookingFromList(int id)
        {
            if (id > 0)
            {
                var tempBooking = BookingList.SingleOrDefault(x => x.BookingId == id);
                if (tempBooking != null)
                    BookingList.Remove(tempBooking);
            }
        }
    }
}
