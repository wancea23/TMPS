namespace Lab2.Models;

public class CarBuilder
{
    private string _title = "";
    private int _year = DateTime.Now.Year;
    private FuelType _fuelType = FuelType.Petrol;
    private int _mileage;
    private int _engineCc;
    private decimal _price;

    public CarBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }

    public CarBuilder WithYear(int year)
    {
        _year = year;
        return this;
    }

    public CarBuilder WithFuelType(FuelType fuelType)
    {
        _fuelType = fuelType;
        return this;
    }

    public CarBuilder WithMileage(int mileage)
    {
        _mileage = mileage;
        return this;
    }

    public CarBuilder WithEngineCc(int engineCc)
    {
        _engineCc = engineCc;
        return this;
    }

    public CarBuilder WithPrice(decimal price)
    {
        _price = price;
        return this;
    }

    public Car Build()
    {
        if (_title == "")
        {
            throw new InvalidOperationException("A car needs a title");
        }
        if (_price <= 0)
        {
            throw new InvalidOperationException($"{_title}: price must be greater than 0");
        }
        if (_fuelType != FuelType.Electric && _engineCc <= 0)
        {
            throw new InvalidOperationException($"{_title}: a {_fuelType} car needs an engine size");
        }
        return new Car(_title, _year, _fuelType, _mileage, _engineCc, _price);
    }
}
