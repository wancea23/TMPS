using Lab3.Models;
using Lab3.Services;
namespace Lab3.Shipping;

public class TruckShipping : IShippingStrategy
{
    public string Name { get; } = "Car transporter truck";

    public decimal CalculateShipping(Car car)
    {
        return ImportSettings.Instance.ShippingEur;
    }
}
