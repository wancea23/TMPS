using Lab1.Models;
namespace Lab1.Taxes;

public class HybridExcise : IExciseRule
{
    public FuelType FuelType { get;} = FuelType.Hybrid;
    public decimal CalculateExcise(Car car)
    {
        return new PetrolExcise().CalculateExcise(car) * 0.75m;
    }
}