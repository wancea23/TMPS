using Lab3.Models;
namespace Lab3.Taxes;

public class HybridExcise : IExciseRule
{
    public FuelType FuelType { get; } = FuelType.Hybrid;
    public decimal CalculateExcise(Car car)
    {
        return new PetrolExcise().CalculateExcise(car) * 0.75m;
    }
}