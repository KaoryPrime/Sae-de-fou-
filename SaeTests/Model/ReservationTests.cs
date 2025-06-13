using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sae.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sae.Model.Tests
{
    [TestClass()]
    public class ReservationTests
    {
        [TestMethod]
        public void Validation_WithValidDates_ReturnsTrue()
        {
            DateTime startDate = DateTime.Today.AddDays(1);
            DateTime endDate = DateTime.Today.AddDays(3);

            bool result = IsReservationDatesValid(startDate, endDate);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Validation_EndDateSameAsStartDate_ReturnsFalse()
        {
            DateTime date = DateTime.Today.AddDays(1);

            bool result = IsReservationDatesValid(date, date);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Validation_EndDateBeforeStartDate_ReturnsFalse()
        {
            DateTime startDate = DateTime.Today.AddDays(3);
            DateTime endDate = DateTime.Today.AddDays(1);

            bool result = IsReservationDatesValid(startDate, endDate);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Validation_StartDateInThePast_ReturnsFalse()
        {
            DateTime startDate = DateTime.Today.AddDays(-1);
            DateTime endDate = DateTime.Today.AddDays(1);

            bool result = IsReservationDatesValid(startDate, endDate);

            Assert.IsFalse(result);
        }

        [TestMethod]
        [DataRow(1, 100.0, 100.0)]
        [DataRow(4, 50.0, 200.0)]
        [DataRow(5, 20.0, 100.0)]
        public void PriceCalculation_ForValidDuration_ReturnsCorrectPrice(int days, double dailyPrice, double expectedTotal)
        {
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today.AddDays(days);
            decimal price = Convert.ToDecimal(dailyPrice);
            decimal expected = Convert.ToDecimal(expectedTotal);

            decimal actual = CalculateTotalPrice(startDate, endDate, price);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PriceCalculation_ForLessThanOneDay_ReturnsOneDayPrice()
        {
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today.AddHours(5);
            decimal dailyPrice = 75.0m;

            decimal actual = CalculateTotalPrice(startDate, endDate, dailyPrice);

            Assert.AreEqual(dailyPrice, actual);
        }

        [TestMethod()]
        private bool IsReservationDatesValid(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue || !endDate.HasValue) return false;
            if (endDate.Value <= startDate.Value) return false;
            if (startDate.Value.Date < DateTime.Today) return false;

            if ((endDate.Value - startDate.Value).TotalDays > 5) return false;

            return true;
        }

        private decimal CalculateTotalPrice(DateTime startDate, DateTime endDate, decimal dailyPrice)
        {
            if (endDate > startDate)
            {
                double numberOfDays = Math.Ceiling((endDate - startDate).TotalDays);
                decimal total = (decimal)numberOfDays * dailyPrice;
                return total;
            }
            return 0;
        }

        [TestMethod]
        public void Validation_RentalLongerThanFiveDays_ReturnsFalse()
        {
            DateTime startDate = DateTime.Today.AddDays(1);
            DateTime endDate = DateTime.Today.AddDays(7); // 6 jours de location

            bool result = IsReservationDatesValid(startDate, endDate);

            Assert.IsFalse(result);
        }
    }
}