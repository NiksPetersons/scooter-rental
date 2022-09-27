using System;

namespace ScooterRental
{
    public class RentalHistory
    {
        public Scooter Scooter;
        public DateTime RentStart;
        public DateTime? RentEnd;

        public RentalHistory(Scooter scooter, DateTime rentStart)
        {
            Scooter = scooter;
            RentStart = rentStart;
            RentEnd = null;
        }

        public RentalHistory(Scooter scooter, DateTime rentStart, DateTime rentEnd)
        {
            Scooter = scooter;
            RentStart = rentStart;
            RentEnd = rentEnd;
        }

        public void ConcludeRent()
        {
            RentEnd = DateTime.Now;
            Scooter.IsRented = false;
        }
    }
}