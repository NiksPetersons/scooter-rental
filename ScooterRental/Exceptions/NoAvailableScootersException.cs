using System;

namespace ScooterRental.Exceptions
{
    public class NoAvailableScootersException : Exception
    {
        public NoAvailableScootersException() : base("All of the scooters are currently rented")
        {

        }
    }
}