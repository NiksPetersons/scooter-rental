namespace ScooterRental
{
    public interface IRentalFeeCalculator
    {
        decimal CalculateRentalFee(RentalHistory rentInstance);
    }
}