using Lab1.Models;
namespace Lab1.Taxes;

public class DieselExcise : IExciseRule
{
    public FuelType FuelType { get; } = FuelType.Diesel;
    public decimal CalculateExcise(Car car)
    {
        if (car.EngineCc <= 1500)
        {
            return car.EngineCc * 12.23m;
        }
        else if (car.EngineCc <= 2500)
        {
            return car.EngineCc * 31.14m;
        }
        else
        {
            return car.EngineCc * 55.6m;
        }
    }
}