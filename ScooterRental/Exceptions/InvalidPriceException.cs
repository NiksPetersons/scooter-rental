using System;

namespace ScooterRental.Exceptions
{
    public class InvalidPriceException : Exception
    {
        public InvalidPriceException() : base("Price per minute cannot be zero or negative")
        {

        }
    }
}