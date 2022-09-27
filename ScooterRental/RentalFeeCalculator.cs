using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScooterRental
{
    public class RentalFeeCalculator : IRentalFeeCalculator
    {
        public decimal CalculateRentalFee(RentalHistory rentInstance)
        {
            decimal totalFee = 0;
            DateTime rentEnd = rentInstance.RentEnd ?? DateTime.Now;
            DateTime rentStart = rentInstance.RentStart;
            decimal pricePerMinute = rentInstance.Scooter.PricePerMinute;
            
            if ((rentEnd - rentStart).Days == 0)
            {
                TimeSpan differenceTimeSpan = rentEnd - rentStart;
                var totalMinutes = Math.Ceiling(differenceTimeSpan.TotalMinutes);

                if ((decimal)totalMinutes * rentInstance.Scooter.PricePerMinute > 20m)
                {
                    totalFee = 20m;
                }
                else
                {
                    totalFee = (decimal)totalMinutes * rentInstance.Scooter.PricePerMinute;
                }
            }
            else
            {
                var firstDayIncome = (decimal)(rentStart.Date.AddDays(1) - rentStart).TotalMinutes * pricePerMinute;

                if (firstDayIncome > 20m)
                {
                    firstDayIncome = 20m;
                }

                var lastDayIncome = (decimal)(rentEnd - rentEnd.Date).TotalMinutes * pricePerMinute;

                if (lastDayIncome > 20m)
                {
                    lastDayIncome = 20m;
                }

                var daysBetween = (rentEnd.AddDays(-1) - rentStart).Days;
                decimal betweenIncome = daysBetween * 20m;
                totalFee = firstDayIncome + lastDayIncome + betweenIncome;
            }

            return totalFee;
        }
    }
}
