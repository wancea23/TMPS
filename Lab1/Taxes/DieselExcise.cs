using Lab1.Models;
namespace Lab1.Taxes;


public class DieselExcise : IExciseRule
{
    public FuelType FuelType { get;} = FuelType.Diesel;
    public decimal CalculateExcise(Car car)
    {
        if (car.Enginecc <= 1500)
        {
            return car.Enginecc*12.23m;
        }
        else if (car.Enginecc <= 2500)
        {
            return car.Enginecc*31.14m;
        }
        else
        {
            return car.Enginecc*55.6m;
        }
    }
}