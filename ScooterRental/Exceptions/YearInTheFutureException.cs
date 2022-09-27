using System;

namespace ScooterRental.Exceptions
{
    public class YearInTheFutureException : Exception
    {
        public YearInTheFutureException() : base("Cannot get income report for a year in the future")
        {

        }
    }
}