using FinancialAnalysis.Models.Accounting;
using System;
using System.Collections.Generic;

namespace WebApiWrapper.Accounting
{
    public static class Bookings
    {
        private const string controllerName = "Bookings";

        public static List<Booking> GetAll()
        {
            return WebApi<List<Booking>>.GetData(controllerName);
        }

        public static Booking GetById(int id)
        {
            return WebApi<Booking>.GetDataById(controllerName, id);
        }

        public static List<Booking> GetByParameter(DateTime startDate, DateTime endDate, int? costAccountCreditorId = null, int? costAccountDebitorId = null, bool OnlyCanceledBookings = false)
        {
            Dictionary<string, object> parameters = new Dictionary<string, object>
            {
                { "startDate", startDate },
                { "endDate", endDate },
                { "costAccountCreditorId", costAccountCreditorId },
                { "costAccountDebitorId", costAccountDebitorId }
            };
            if (OnlyCanceledBookings)
            {
                parameters.Add("OnlyCanceledBookings", OnlyCanceledBookings);
            }
            return WebApi<List<Booking>>.GetData(controllerName, "GetByParameter", parameters);
        }

        public static int Insert(Booking Booking)
        {
            return WebApi<int>.PostAsync(controllerName, Booking, "SinglePost").Result;
        }

        public static int Insert(IEnumerable<Booking> Bookings)
        {
            return WebApi<int>.PostAsync(controllerName, Bookings, "MultiPost").Result;
        }

        public static bool UpdateCancelingStatus(Booking Booking)
        {
            return WebApi<bool>.PutAsync(controllerName, Booking, "PutUpdateCancelingStatus").Result;
        }
    }
}