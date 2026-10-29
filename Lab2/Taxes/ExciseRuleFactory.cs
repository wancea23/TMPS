using Lab2.Models;
namespace Lab2.Taxes;

public static class ExciseRuleFactory
{
    public static IExciseRule Create(FuelType fuelType)
    {
        switch (fuelType)
        {
            case FuelType.Petrol:
                return new PetrolExcise();
            case FuelType.Diesel:
                return new DieselExcise();
            case FuelType.Hybrid:
                return new HybridExcise();
            case FuelType.Electric:
                return new ElectricExcise();
            default:
                throw new ArgumentException($"No excise rule for fuel type {fuelType}");
        }
    }
}
