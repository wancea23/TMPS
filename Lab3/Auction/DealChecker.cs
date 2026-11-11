using Lab3.Checks;
using Lab3.Models;
namespace Lab3.Auction;

public class DealChecker : ICarObserver
{
    private readonly CarCheck _checks;

    public DealChecker(CarCheck checks)
    {
        _checks = checks;
    }

    public void OnNewCar(Car car)
    {
        Console.WriteLine($"Verdict: {_checks.Check(car)}");
        Console.WriteLine();
    }
}
