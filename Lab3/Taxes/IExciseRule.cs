using Lab3.Models;
namespace Lab3.Taxes;

public interface IExciseRule
{
    FuelType FuelType { get; }
    decimal CalculateExcise(Car car);
}