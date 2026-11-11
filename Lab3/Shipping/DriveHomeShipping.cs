using Lab3.Models;
namespace Lab3.Shipping;

public class DriveHomeShipping : IShippingStrategy
{
    private const decimal DistanceKm = 2500m;
    private const decimal FuelPerKm = 0.12m;
    private const decimal TransitPlates = 150m;

    public string Name { get; } = "Drive it home";

    public decimal CalculateShipping(Car car)
    {
        return DistanceKm * FuelPerKm + TransitPlates;
    }
}
