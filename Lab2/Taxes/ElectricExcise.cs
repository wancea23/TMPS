using Lab2.Models;
namespace Lab2.Taxes;

public class ElectricExcise : IExciseRule
{
    public FuelType FuelType { get; } = FuelType.Electric;
    public decimal CalculateExcise(Car car)
    {
        return 0;
    }
}