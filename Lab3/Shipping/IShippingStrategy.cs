using Lab3.Models;
namespace Lab3.Shipping;

public interface IShippingStrategy
{
    string Name { get; }
    decimal CalculateShipping(Car car);
}
