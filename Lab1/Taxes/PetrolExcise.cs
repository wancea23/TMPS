using Lab1.Models;
namespace Lab1.Taxes;

public class PetrolExcise : IExciseRule
{
    public FuelType FuelType { get;} = FuelType.Petrol;
    public decimal CalculateExcise(Car car)
    {
        if (car.Enginecc <= 1000)
        {
            return car.Enginecc*9.56m;
        }
        else if (car.Enginecc <= 1500)
        {
            return car.Enginecc*12.23m;
        }
        else if (car.Enginecc <= 2000)
        {
            return car.Enginecc*18.9m;
        }
        else if (car.Enginecc <= 3000)
        {
            return car.Enginecc*31.14m;
        }
        else
        {
            return car.Enginecc*55.6m;
        }
    }
}