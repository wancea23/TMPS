using Lab2.Taxes;
using Lab2.Models;
namespace Lab2.Services;

public class ImportCostCalculator
{
    public decimal CalculateExcise(Car car)
    {
        IExciseRule rule = ExciseRuleFactory.Create(car.FuelType);
        return rule.CalculateExcise(car);
    }

    public decimal CalculateTotal(Car car)
    {
        var settings = ImportSettings.Instance;
        return car.Price + CalculateExcise(car) / settings.MdlPerEur + settings.ShippingEur + car.Price * settings.ProcedureFeeRate;
    }
}
