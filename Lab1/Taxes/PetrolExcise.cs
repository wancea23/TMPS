using Lab1.Models;
namespace Lab1.Taxes;

public class PetrolExcise : IExciseRule
{
    public FuelType FuelType { get; } = FuelType.Petrol;
    public decimal CalculateExcise(Car car)
    {
        if (car.EngineCc <= 1000)
        {
            return car.EngineCc * 9.56m;
        }
        else if (car.EngineCc <= 1500)
        {
            return car.EngineCc * 12.23m;
        }
        else if (car.EngineCc <= 2000)
        {
            return car.EngineCc * 18.9m;
        }
        else if (car.EngineCc <= 3000)
        {
            return car.EngineCc * 31.14m;
        }
        else
        {
            return car.EngineCc * 55.6m;
        }
    }
}