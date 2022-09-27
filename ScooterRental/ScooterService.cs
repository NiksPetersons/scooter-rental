using System.Collections.Generic;
using System.Linq;
using ScooterRental.Exceptions;

namespace ScooterRental
{
    public class ScooterService : IScooterService
    {
        private readonly List<Scooter> _scooterList;

        public ScooterService(List<Scooter> inventory)
        {
            _scooterList = inventory;
        }

        public void AddScooter(string id, decimal pricePerMinute)
        {
            if (_scooterList.Any(x => x.Id == id))
            {
                throw new DuplicateScooterException(id);
            }

            if (pricePerMinute <= 0)
            {
                throw new InvalidPriceException();
            }

            Validations.IdIsNullOrEmptyValidation(id);

            _scooterList.Add(new Scooter(id, pricePerMinute));
        }

        public Scooter GetScooterById(string scooterId)
        {
            Validations.IdIsNullOrEmptyValidation(scooterId);

            Validations.IdDoesNotExistValidation(scooterId, _scooterList);

            return _scooterList.FirstOrDefault(x => x.Id == scooterId);
        }

        public IList<Scooter> GetScooters()
        {
            if (_scooterList.Count == 0)
            {
                throw new ScooterListIsEmptyException();
            }

            if (_scooterList.Where(x => x.IsRented == true).Count() == _scooterList.Count)
            {
                throw new NoAvailableScootersException();
            }

            return _scooterList.Where(x => x.IsRented == false).ToList();
        }

        public void RemoveScooter(string id)
        {
            Validations.IdIsNullOrEmptyValidation(id);

            Validations.IdDoesNotExistValidation(id, _scooterList);

            _scooterList.RemoveAll(x => x.Id == id);
        }
    }
}