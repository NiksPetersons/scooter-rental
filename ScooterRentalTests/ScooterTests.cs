using System;
using FluentAssertions;
using ScooterRental;
using Xunit;



namespace ScooterRentalTests
{
    public class ScooterTests
    {
        [Fact]
        public void ScooterCreation_CanGetCorrectIdAndPricePerMinuteAndIsRented()
        {
            //Arrange
            var _sut = new Scooter("1", 0.2m);
            //Assert
            _sut.Id.Should().Be("1");
            _sut.PricePerMinute.Should().Be(0.2m);
            _sut.IsRented.Should().BeFalse();
        }
    }
}
