using System;
using System.Globalization;
using FluentAssertions;
using ScooterRental;
using Xunit;

namespace ScooterRentalTests
{
    public class RentalHistoryTests
    {
        private ScooterRental.RentalHistory _sut;
        private Scooter _scooter = new Scooter("1", 0.2m);
        private DateTime _rentStartTime = new DateTime(2022, 01, 01, 1, 1, 1);

        public RentalHistoryTests()
        {
            _sut = new RentalHistory(_scooter, _rentStartTime);
        }

        [Fact]
        public void RentalHistoryCreation_ShouldBeAbleToGetCorrectValues()
        {
            //Assert
            _sut.Scooter.Should().Be(_scooter);
            _sut.RentStart.Should().Be(_rentStartTime);
            _sut.RentEnd.Should().Be(null);
        }

        [Fact]
        public void RentalHistory_ShouldBeAbleToConcludeRentAndFillFields()
        {
            //Act
            _sut.ConcludeRent();
            //Assert
            _sut.Scooter.IsRented.Should().BeFalse();
            _sut.RentEnd.Should().NotBeNull();
        }
    }
}