using Lab1.Models;
namespace Lab1.Taxes;

public interface IExciseRule
{
    FuelType FuelType { get; }
    decimal CalculateExcise(Car car);
}