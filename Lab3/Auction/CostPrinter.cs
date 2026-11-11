using Lab3.Models;
using Lab3.Services;
namespace Lab3.Auction;

public class CostPrinter : ICarObserver
{
    private readonly ImportCostCalculator _calculator;

    public CostPrinter(ImportCostCalculator calculator)
    {
        _calculator = calculator;
    }

    public void OnNewCar(Car car)
    {
        Console.WriteLine($"New car: {car.Title} ({car.Year}), {car.FuelType}, {car.Mileage} km, {car.EngineCc} cc, {car.Price} EUR");
        Console.WriteLine($"Excise: {_calculator.CalculateExcise(car):N2} lei");
        Console.WriteLine($"Total: {_calculator.CalculateTotal(car):N2} EUR");
    }
}
