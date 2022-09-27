using System;
using System.Collections.Generic;
using FluentAssertions;
using ScooterRental;
using ScooterRental.Exceptions;
using Xunit;

namespace ScooterRentalTests
{
    public class RentalCompanyTests
    {
        private IRentalCompany _sut;
        private string _name = "Test Company";
        private IList<RentalHistory> _rentalHistory;
        private IScooterService _rentalService;
        private IRentalFeeCalculator _rentCalc;

        public RentalCompanyTests()
        {
            _rentalHistory = new List<RentalHistory>();
            _rentalService = new ScooterService(new List<Scooter>());
            _rentCalc = new RentalFeeCalculator();
            _sut = new RentalCompany(_name, _rentalHistory, _rentalService, _rentCalc);
            _rentalService.AddScooter("1", 0.2m);
            _rentalService.AddScooter("2", 0.25m);
            _rentalService.AddScooter("3", 0.3m);
        }

        [Fact]
        public void RentalCompanyCreation_ShouldBeAbleToGetNameAndRentalHistory()
        {
            //Assert
            _sut.Name.Should().Be(_name);
            _rentalHistory.Should().BeOfType<List<RentalHistory>>();
        }

        [Fact]
        public void RentalCompany_StartRent_ShouldBeAbleToRentScooterSuccessfully()
        {
            //Act
            _sut.StartRent("1");
            Scooter scooter = _rentalService.GetScooterById("1");
            //Assert
            scooter.IsRented.Should().Be(true);
        }

        [Fact]
        public void RentalCompany_StartRent_ShouldThrowScooterNotAvailableException()
        {
            //Act
            _sut.StartRent("1");
            Action action = () => _sut.StartRent("1");
            //Assert
            action.Should().Throw<ScooterNotAvailableException>()
                .WithMessage("Scooter with ID 1 is not available");
        }

        [Fact]
        public void RentalCompany_StartRent_ShouldThrowInvalidIdException()
        {
            //Act
            Action action = () => _sut.StartRent("");
            //Assert
            action.Should().Throw<InvalidIdException>()
                .WithMessage("Id cannot be null or empty");
        }

        [Fact]
        public void RentalCompany_StartRent_ShouldThrowScooterDoesNotExistException()
        {
            //Act
            Action action = () => _sut.StartRent("5");
            //Assert
            action.Should().Throw<ScooterDoesNotExistException>()
                .WithMessage("Scooter with ID 5 does not exist");
        }

        [Fact]
        public void RentalCompany_EndRent_ShouldBeAbleToEndRentSuccessfully()
        {
            //Act
            _sut.StartRent("1");
            _sut.EndRent("1");
            //Assert
            _rentalHistory[0].RentEnd.Should().NotBeNull();

        }

        [Fact]
        public void RentalCompany_EndRent_ShouldThrowInvalidIdException()
        {
            //Act
            Action action = () => _sut.EndRent("");
            //Assert
            action.Should().Throw<InvalidIdException>()
                .WithMessage("Id cannot be null or empty");
        }

        [Fact]
        public void RentalCompany_EndRent_ShouldThrowIdDoesNotExistException()
        {
            //Act
            Action action = () => _sut.EndRent("5");
            //Assert
            action.Should().Throw<ScooterDoesNotExistException>()
                .WithMessage("Scooter with ID 5 does not exist");
        }

        [Fact]
        public void RentalCompany_CalculateIncome_ShouldThrowYearInTheFutureException()
        {
            //Act
            Action action = () => _sut.CalculateIncome(DateTime.Now.AddYears(+1).Year, false);
            //Assert
            action.Should().Throw<YearInTheFutureException>().WithMessage("Cannot get income report for a year in the future");
        }

        [Theory]
        [InlineData(2020, false, 38.2)]
        [InlineData(2020, true, 38.2)]
        [InlineData(null, false, 60.2)]
        [InlineData(2022, true, 24.2)]
        [InlineData(null, true, 62.4)]
        public void RentalCompany_CalculateIncome_ShouldReturnCorrectDecimal(int? year
            , bool includeNotCompletedRentals, decimal expected)
        {
            //Arrange
            _rentalHistory = SimulateRentalHistory();
            //Act & Assert
            _sut.CalculateIncome(year, includeNotCompletedRentals).Should().Be(expected);
        }

        private List<RentalHistory> SimulateRentalHistory()
        {
            List<RentalHistory> rentalHistory = new List<RentalHistory>();

            var scooter1 = new Scooter("1", 0.2m);
            var scooter2 = new Scooter("2", 0.3m);
            var scooter3 = new Scooter("3", 0.25m);
            _rentalHistory.Add(new RentalHistory(scooter1
                , new DateTime(2020, 01, 01, 0, 0, 0)
                , new DateTime(2020, 01, 01, 0, 1, 0)));//0.2
            _rentalHistory.Add(new RentalHistory(scooter2
                , new DateTime(2020, 01, 01, 0, 0, 0)
                , new DateTime(2020, 01, 01, 1, 0, 0)));//18
            _rentalHistory.Add(new RentalHistory(scooter3
                , new DateTime(2020, 01, 01, 0, 0, 0)
                , new DateTime(2020, 01, 02, 0, 0, 0)));//20
            _rentalHistory.Add(new RentalHistory(scooter1
                , new DateTime(2022, 01, 01, 0, 0, 0)
                , new DateTime(2022, 01, 02, 0, 0, 0)));//20
            _rentalHistory.Add(new RentalHistory(scooter1
                , new DateTime(2022, 01, 01, 0, 0, 0)
                , new DateTime(2022, 01, 01, 0, 10, 0)));//2
            _rentalHistory.Add(new RentalHistory(scooter1, DateTime.Now.AddMinutes(-10)));//2.2

            return rentalHistory;
        }
    }
}