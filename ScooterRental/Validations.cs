using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ScooterRental.Exceptions;

namespace ScooterRental
{
    public static class Validations
    {
        public static void IdIsNullOrEmptyValidation(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new InvalidIdException();
            }
        }

        public static void IdDoesNotExistValidation(string id, List<Scooter> scooterList)
        {
            if (!scooterList.Any(x => x.Id == id))
            {
                throw new ScooterDoesNotExistException(id);
            }
        }
    }
}