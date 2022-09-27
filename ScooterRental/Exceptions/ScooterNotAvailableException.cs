using System;

namespace ScooterRental.Exceptions
{
    public class ScooterNotAvailableException : Exception
    {
        public ScooterNotAvailableException(string id) : base($"Scooter with ID {id} is not available")
        {

        }
    }
}