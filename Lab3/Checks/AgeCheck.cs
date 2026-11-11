using Lab3.Models;
namespace Lab3.Checks;

public class AgeCheck : CarCheck
{
    private readonly int _maxAge;

    public AgeCheck(int maxAge)
    {
        _maxAge = maxAge;
    }

    public override string Check(Car car)
    {
        int age = DateTime.Now.Year - car.Year;
        if (age > _maxAge)
        {
            return $"Skip, too old ({age} years, max {_maxAge})";
        }
        return base.Check(car);
    }
}
