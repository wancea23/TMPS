using Lab3.Models;
namespace Lab3.Checks;

public abstract class CarCheck
{
    private CarCheck? _next;

    public CarCheck SetNext(CarCheck next)
    {
        _next = next;
        return next;
    }

    public virtual string Check(Car car)
    {
        if (_next == null)
        {
            return "OK, worth importing";
        }
        return _next.Check(car);
    }
}
