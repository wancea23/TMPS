using Lab1.Taxes;
using Lab1.Models;
namespace Lab1.Services;

public class ImportCostCalculator
{
    private const decimal MdlPerEur = 19.9863m;      // exchange rate
    private const decimal ShippingEur = 900m;        // transport estimate
    private const decimal ProcedureFeeRate = 0.004m; // customs fee 0.4%

    private readonly List<IExciseRule> _rules;

    public ImportCostCalculator(List<IExciseRule> rules)
    {
        _rules = rules;
    }

    public decimal CalculateExcise(Car car)
    {
        foreach (var rule in _rules)
        {
            if (rule.FuelType == car.FuelType)
            {
                return rule.CalculateExcise(car);
            }
        }
        throw new Exception($"No excise rule found for fuel type {car.FuelType}");
    }

    public decimal CalculateTotal(Car car)
    {
        return car.Price + CalculateExcise(car) / MdlPerEur + ShippingEur + car.Price * ProcedureFeeRate;
    }
}
