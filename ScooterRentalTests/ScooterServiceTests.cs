using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using ScooterRental;
using ScooterRental.Exceptions;
using Xunit;

namespace ScooterRentalTests
{
    public class ScooterServiceTests
    {
        private IScooterService _sut;
        private List<Scooter> _scooterList;
        
        public ScooterServiceTests()
        {
            _scooterList = new List<Scooter>();
            _sut = new ScooterService(_scooterList);
        }

        [Fact]
        public void ScooterService_AddScooter_ShouldAddScooterSuccessfully()
        {
            //Arrange
            _sut.AddScooter("1", 0.2m);
            //Assert
            _scooterList.Count.Should().Be(1);
        }

        [Fact]
        public void ScooterService_AddScooter_ShouldThrowDuplicateIdException()
        {
            //Arrange
            _sut.AddScooter("1", 0.2m);
            //Act
            Action action = () => _sut.AddScooter("1", 0.2m);
            //Assert
            action.Should().Throw<DuplicateScooterException>().WithMessage("Scooter with ID 1 already exists");
        }

        [Fact]
        public void ScooterService_AddScooter_ShouldThrowInvalidPriceException()
        {
            //Act
            Action action = () => _sut.AddScooter("1", 0m);
            //Assert
            action.Should().Throw<InvalidPriceException>()
                .WithMessage("Price per minute cannot be zero or negative");
        }

        [Fact]
        public void ScooterService_AddScooter_ShouldThrowInvalidIdException()
        {
            //Act
            Action action = () => _sut.AddScooter("", 0.2m);
            //Assert
            action.Should().Throw<InvalidIdException>().WithMessage("Id cannot be null or empty");
        }

        [Fact]
        public void ScooterService_RemoveScooter_ShouldBeAbleToRemoveScooterSuccessfully()
        {
            //Arrange
            _sut.AddScooter("1", 0.2m);
            //Act
            _sut.RemoveScooter("1");
            //Assert
            _scooterList.Count.Should().Be(0);
            _scooterList.Any(x => x.Id == "1").Should().BeFalse();
        }

        [Fact]
        public void ScooterService_RemoveScooter_ShouldThrowScooterDoesNotExistException()
        {
            //Arrange
            _sut.AddScooter("1", 0.2m);
            //Act
            Action action = () => _sut.RemoveScooter("2");
            //Assert
            action.Should().Throw<ScooterDoesNotExistException>()
                .WithMessage("Scooter with ID 2 does not exist");
        }

        [Fact]
        public void ScooterService_RemoveScooter_ShouldThrowInvalidIdException()
        {
            //Act
            Action action = () => _sut.RemoveScooter("");
            //Assert
            action.Should().Throw<InvalidIdException>().WithMessage("Id cannot be null or empty");
        }

        [Fact]
        public void ScooterService_GetScooters_ShouldReturnScooterListSuccessfully()
        {
            //Arrange
            _sut.AddScooter("1", 0.2m);
            _sut.AddScooter("2", 0.2m);
            _sut.AddScooter("3", 0.2m);
            //Assert
            _sut.GetScooters().Should().BeOfType<List<Scooter>>().And.HaveCount(3);
        }

        [Fact]
        public void ScooterService_GetScooters_ShouldThrowListIsEmptyException()
        {
            //Act
            Action action = () => _sut.GetScooters();
            //Assert
            action.Should().Throw<ScooterListIsEmptyException>()
                .WithMessage("There are no scooters");
        }

        [Fact]
        public void ScooterService_GetScooters_ShouldThrowNoAvailableScootersException()
        {
            //Arrange
            _sut.AddScooter("1", 0.2m);
            _sut.AddScooter("2", 0.2m);
            _sut.AddScooter("3", 0.2m);
            _scooterList.ForEach(x => x.IsRented = true);
            //Act
            Action action = () => _sut.GetScooters();
            //Assert
            action.Should().Throw<NoAvailableScootersException>()
                .WithMessage("All of the scooters are currently rented");
        }

        [Fact]
        public void ScooterService_GetScooterById_ShouldReturnScooterSuccessfully()
        {
            //Arrange
            _sut.AddScooter("2", 0.2m);
            _sut.AddScooter("1", 0.2m);
            //Act & Assert
            _sut.GetScooterById("1").Id.Should().Be("1");
            _sut.GetScooterById("1").Should().BeOfType<Scooter>();
        }

        [Fact]
        public void ScooterService_GetScooterById_ShouldThrowScooterDoesNotExistException()
        {
            //Arrange
            _sut.AddScooter("1", 0.2m);
            //Act
            Action action = () => _sut.GetScooterById("2");
            //Assert
            action.Should().Throw<ScooterDoesNotExistException>()
                .WithMessage("Scooter with ID 2 does not exist");
        }

        [Fact]
        public void ScooterService_GetScooterById_ShouldThrowInvalidIdException()
        {
            //Act
            Action action = () => _sut.GetScooterById("");
            //Assert
            action.Should().Throw<InvalidIdException>().WithMessage("Id cannot be null or empty");
        }
    }
}