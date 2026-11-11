using Lab3.Taxes;
using Lab3.Models;
using Lab3.Shipping;
namespace Lab3.Services;

public class ImportCostCalculator
{
    private readonly IShippingStrategy _shipping;

    public ImportCostCalculator(IShippingStrategy shipping)
    {
        _shipping = shipping;
    }

    public decimal CalculateExcise(Car car)
    {
        IExciseRule rule = ExciseRuleFactory.Create(car.FuelType);
        return rule.CalculateExcise(car);
    }

    public decimal CalculateTotal(Car car)
    {
        var settings = ImportSettings.Instance;
        return car.Price + CalculateExcise(car) / settings.MdlPerEur + _shipping.CalculateShipping(car) + car.Price * settings.ProcedureFeeRate;
    }
}
