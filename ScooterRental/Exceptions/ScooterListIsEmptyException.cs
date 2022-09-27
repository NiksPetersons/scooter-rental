using System;

namespace ScooterRental.Exceptions
{
    public class ScooterListIsEmptyException : Exception
    {
        public ScooterListIsEmptyException() : base("There are no scooters")
        {

        }
    }
}