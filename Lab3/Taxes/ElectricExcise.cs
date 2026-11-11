using Lab3.Models;
namespace Lab3.Taxes;

public class ElectricExcise : IExciseRule
{
    public FuelType FuelType { get; } = FuelType.Electric;
    public decimal CalculateExcise(Car car)
    {
        return 0;
    }
}