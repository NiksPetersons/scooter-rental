using System;
using System.Globalization;
using FluentAssertions;
using ScooterRental;
using Xunit;

namespace ScooterRentalTests
{
    public class RentalFeeCalculatorTests
    {
        private RentalHistory _sut;
        private IRentalFeeCalculator _calculator;

        public RentalFeeCalculatorTests()
        {
            _calculator = new RentalFeeCalculator();
            _sut = new RentalHistory(new Scooter("1", 0.2m)
                , new DateTime(2022, 01, 01, 0, 0, 0));
        }

        [Theory]
        [InlineData("01/01/2022 00:05", 1)]
        [InlineData("02/01/2022 01:06", 33.2)]
        [InlineData("11/01/2022 01:00", 212)]
        [InlineData("21/01/2022 01:12", 414.4)]
        public void RentalFeeCalculator_ShouldReturnCorrectDecimal(string date, decimal expected)
        {
            //Arrange
            _sut.RentEnd = DateTime.ParseExact(date, "dd/MM/yyyy hh:mm", CultureInfo.InvariantCulture);
            //Act & Assert
            _calculator.CalculateRentalFee(_sut).Should().Be(expected);
        }
    }
}