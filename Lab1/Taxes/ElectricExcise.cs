using Lab1.Models;
namespace Lab1.Taxes;

public class ElectricExcise : IExciseRule
{
    public FuelType FuelType { get;} = FuelType.Electric;
    public decimal CalculateExcise(Car car)
    {
        return 0;
    }
}