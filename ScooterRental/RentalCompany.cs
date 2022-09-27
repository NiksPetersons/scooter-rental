using System;
using System.Collections.Generic;
using System.Linq;
using ScooterRental.Exceptions;

namespace ScooterRental
{
    public class RentalCompany : IRentalCompany
    {
        public string Name { get; }
        private IList<RentalHistory> _rentalHistory;
        private IScooterService _rentalService;
        private IRentalFeeCalculator _rentalFeeCalculator;

        public RentalCompany(string name, IList<RentalHistory> rentHistory, IScooterService service, IRentalFeeCalculator iRentalFeeCalculator)
        {
            Name = name;
            _rentalHistory = rentHistory;
            _rentalService = service;
            _rentalFeeCalculator = iRentalFeeCalculator;
        }

        public decimal CalculateIncome(int? year, bool includeNotCompletedRentals)
        {
            if (year > DateTime.Now.Year)
            {
                throw new YearInTheFutureException();
            }

            decimal income = 0;
            List<RentalHistory> relevantRentalList;

            if (year.HasValue)
            {
                relevantRentalList = _rentalHistory.Where(x => x.RentEnd.HasValue && x.RentEnd.Value.Year == year).ToList();
                income += relevantRentalList.Aggregate(0m, (ac, x) => ac += _rentalFeeCalculator.CalculateRentalFee(x));

                if (includeNotCompletedRentals && year == DateTime.Now.Year)
                {
                    relevantRentalList = _rentalHistory.Where(x => x.RentEnd == null).ToList();
                    income += relevantRentalList.Aggregate(0m, (ac, x) => ac += _rentalFeeCalculator.CalculateRentalFee(x));
                }
            }
            else if(includeNotCompletedRentals)
            {
                income += _rentalHistory.Aggregate(0m, (ac, x) => ac += _rentalFeeCalculator.CalculateRentalFee(x));
            }
            else
            {
                relevantRentalList = _rentalHistory.Where(x => x.RentEnd.HasValue).ToList();
                income += relevantRentalList.Aggregate(0m, (ac, x) => ac += _rentalFeeCalculator.CalculateRentalFee(x));
            }

            return income;
        }

        public decimal EndRent(string id)
        {
            Scooter scooterInQuestion = _rentalService.GetScooterById(id);
            RentalHistory historyEntry = _rentalHistory.Last(x => x.Scooter == scooterInQuestion 
                                                                  && x.Scooter.IsRented == true);
            decimal totalRentalFee = _rentalFeeCalculator.CalculateRentalFee(historyEntry);
            historyEntry.ConcludeRent();
            return totalRentalFee;
        }

        public void StartRent(string id)
        {
            Scooter scooterInQuestion = _rentalService.GetScooterById(id);

            if (scooterInQuestion.IsRented)
            {
                throw new ScooterNotAvailableException(id);
            }

            scooterInQuestion.IsRented = true;
            _rentalHistory.Add(new RentalHistory(scooterInQuestion, DateTime.Now));
        }
    }
}