using Lab3.Models;
namespace Lab3.Checks;

public class MileageCheck : CarCheck
{
    private readonly int _maxMileage;

    public MileageCheck(int maxMileage)
    {
        _maxMileage = maxMileage;
    }

    public override string Check(Car car)
    {
        if (car.Mileage > _maxMileage)
        {
            return $"Skip, too many km ({car.Mileage} km, max {_maxMileage})";
        }
        return base.Check(car);
    }
}
