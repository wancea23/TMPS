using Lab3.Models;
using Lab3.Services;
namespace Lab3.Checks;

public class BudgetCheck : CarCheck
{
    private readonly ImportCostCalculator _calculator;
    private readonly decimal _budget;

    public BudgetCheck(ImportCostCalculator calculator, decimal budget)
    {
        _calculator = calculator;
        _budget = budget;
    }

    public override string Check(Car car)
    {
        decimal total = _calculator.CalculateTotal(car);
        if (total > _budget)
        {
            return $"Skip, over budget ({total:N2} EUR, max {_budget:N2})";
        }
        return base.Check(car);
    }
}
