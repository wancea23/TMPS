# Laboratory Work 2: Creational Design Patterns

**Author:** Moroșanu Ion, FAF-242
**Course:** Software Design Techniques and Mechanisms (TMPS)

## Task

Implement three creational design patterns in the project, and know the rest. I chose:

* Singleton
* Factory Method
* Builder

## Domain

This lab continues the project from Lab1: a calculator for the cost of importing a used car from the EU into Moldova. The total in EUR is the car price, plus the excise tax converted from lei, plus shipping, plus the customs procedure fee:

```
total = price + excise / 19.9863 + 900 (shipping estimate) + 0.4% of the price (customs procedure fee)
```

The excise rules and rates are the same as in Lab1 (one class per fuel type). Lab2 is a copy of Lab1 with the three patterns added. Creational patterns are about how objects are created, so each pattern below takes over the creation of one kind of object: the settings, the excise rules and the cars.

## Project Structure

```
Lab2/
  Models/
    Car.cs                    car data, read only after it is created
    CarBuilder.cs             Builder: builds a Car step by step and checks it
    FuelType.cs               Petrol, Diesel, Electric, Hybrid
  Taxes/
    IExciseRule.cs            interface for one excise rule
    ExciseRuleFactory.cs      Factory Method: returns the right rule for a fuel type
    PetrolExcise.cs
    DieselExcise.cs
    HybridExcise.cs
    ElectricExcise.cs
  Services/
    ImportSettings.cs         Singleton: exchange rate, shipping and procedure fee
    ImportCostCalculator.cs   adds up the total
  Program.cs                  builds the cars and prints the results
```

## Singleton

**Definition:** a class that has only one instance, with one global point of access to it.

The exchange rate, the shipping estimate and the procedure fee rate are settings that the whole program must agree on. In Lab1 they were constants inside the calculator. Now they live in `ImportSettings`, and there can be only one `ImportSettings` object:

```csharp
public class ImportSettings
{
    private static ImportSettings? _instance;

    private ImportSettings() { }

    public static ImportSettings Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new ImportSettings();
            }
            return _instance;
        }
    }

    public decimal MdlPerEur { get; } = 19.9863m;
    public decimal ShippingEur { get; } = 900m;
    public decimal ProcedureFeeRate { get; } = 0.004m;
}
```

How it works:

* The constructor is `private`, so `new ImportSettings()` does not compile anywhere outside the class.
* The only way to get the object is `ImportSettings.Instance`. The first call creates it, every later call returns the same object (lazy initialization).

The calculator reads the settings from the single instance:

```csharp
public decimal CalculateTotal(Car car)
{
    var settings = ImportSettings.Instance;
    return car.Price + CalculateExcise(car) / settings.MdlPerEur + settings.ShippingEur + car.Price * settings.ProcedureFeeRate;
}
```

`Program.cs` reads the same instance to print the settings and checks that both references point to one object (`Same settings object everywhere: True` in the output). If the exchange rate changes, it changes in one place and every part of the program sees the new value.

This version is not thread safe: two threads could both see `_instance == null` at the same time and create two objects. The program runs on one thread, so this is fine here. A thread safe version would use a `lock` or `Lazy<T>`.

## Factory Method

**Definition:** a method that creates objects for the caller, so the caller does not need to know which concrete class it gets. It only works with a common interface.

In Lab1, `Program.cs` created every excise rule by hand and the calculator searched a list of them with a loop. Now `ExciseRuleFactory` is the only place that knows the concrete rule classes. It takes a fuel type and returns the matching rule as an `IExciseRule`:

```csharp
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
```

The calculator asks the factory for a rule and uses it through the interface. It never mentions `PetrolExcise` or any other concrete class:

```csharp
public decimal CalculateExcise(Car car)
{
    IExciseRule rule = ExciseRuleFactory.Create(car.FuelType);
    return rule.CalculateExcise(car);
}
```

To add a new fuel type, for example LPG, I would write an `LpgExcise` class and add one `case` to the factory. Nothing else changes. If a fuel has no rule, the factory throws an exception instead of the calculator returning a wrong number.

This is the static form of the pattern, the same form used in the course example. In the classic form from the Gang of Four book, the factory method is declared in a base class and each subclass overrides it to create a different product.

## Builder

**Definition:** separates the construction of a complex object from the object itself, so the object is built step by step and only handed out once it is complete and valid.

`Car` now has six fields that are set only through its constructor, and it has no setters, so a car cannot be changed after it is created:

```csharp
public Car(string title, int year, FuelType fuelType, int mileage, int engineCc, decimal price)
```

Calling this constructor directly is easy to get wrong. `new Car("Honda Civic", 2019, FuelType.Diesel, 20000, 1800, 20000)` has three numbers in a row, and nothing stops you from swapping the mileage and the price. `CarBuilder` gives every value a name, and each method returns the builder itself, so the calls can be chained:

```csharp
new CarBuilder()
    .WithTitle("Honda Civic")
    .WithYear(2019)
    .WithFuelType(FuelType.Diesel)
    .WithMileage(20000)
    .WithEngineCc(1800)
    .WithPrice(20000)
    .Build();
```

Steps can be skipped when a default makes sense. The Tesla is built without `WithEngineCc`, because an electric car has no engine volume and the default is 0.

`Build()` is the only place where a `Car` is created, so it is also the place to check that the car makes sense:

```csharp
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
```

The last check matters for this domain. The excise is the engine volume times a rate, so a diesel car without an engine size would silently get 0 lei excise. The builder refuses to create it instead, as the last line of the output shows.

## The Other Creational Patterns

* **Prototype:** creates new objects by copying an existing one instead of building them from scratch. Here it could clone a car and change only the year or the price, to compare the import cost of the same model at different ages. In C# this is usually a `Clone()` method based on `MemberwiseClone()`.
* **Object Pool:** keeps a set of ready objects that are expensive to create and lends them out instead of creating new ones, like a pool of database connections. If the project loaded real auction listings from a database, the connections would be a good fit.
* **Abstract Factory:** creates whole families of related objects that must be used together. Here it could be one factory per country, where each factory creates that country's excise rule, customs fee and currency converter, so a Moldovan rule is never mixed with a Romanian fee.

## Output

```
Settings: 1 EUR = 19.9863 MDL, shipping 900 EUR, procedure fee 0.4%
Same settings object everywhere: True

Toyota Camry (2020), Petrol, Mileage: 15000 km, Engine: 2500 cc, Price: 25000 EUR
Excise: 77,850.00 lei
Total: 29,895.17 EUR

Honda Civic (2019), Diesel, Mileage: 20000 km, Engine: 1800 cc, Price: 20000 EUR
Excise: 56,052.00 lei
Total: 23,784.52 EUR

Tesla Model 3 (2021), Electric, Mileage: 5000 km, Engine: 0 cc, Price: 35000 EUR
Excise: 0.00 lei
Total: 36,040.00 EUR

Toyota RAV4 Hybrid (2022), Hybrid, Mileage: 30000 km, Engine: 2487 cc, Price: 28000 EUR
Excise: 58,083.89 lei
Total: 31,918.18 EUR

Builder rejected the car: Volkswagen Golf: a Diesel car needs an engine size
```

The first three totals are the same as in Lab1, so the patterns changed how objects are created without changing any result.

## How to Run

From the repository root, with the .NET 10 SDK installed:

```
dotnet run --project Lab2
```

## Conclusion

Each pattern took over the creation of one kind of object. The Singleton keeps one shared copy of the settings, the Factory Method hides which excise class is created for a fuel type, and the Builder creates cars step by step and refuses invalid ones. The calculator got shorter, because it no longer stores constants or searches a list of rules, and invalid cars are now stopped when they are created instead of producing a wrong tax later.
