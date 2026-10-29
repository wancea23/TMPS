using Lab2.Models;
namespace Lab2.Taxes;

public interface IExciseRule
{
    FuelType FuelType { get; }
    decimal CalculateExcise(Car car);
}