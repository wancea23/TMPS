# Laboratory Work 3: Behavioral Design Patterns

**Author:** Moroșanu Ion, FAF-242
**Course:** Software Design Techniques and Mechanisms (TMPS)

## Task

Implement three behavioral design patterns in the project, and know the rest. I chose:

* Strategy
* Observer
* Chain of Responsibility

## Domain

This lab continues the project from Lab2: a calculator for the cost of importing a used car from the EU into Moldova.

```
total = price + excise / 19.9863 + shipping + 0.4% of the price (customs procedure fee)
```

Lab3 is a copy of Lab2 with the three patterns added. Creational patterns were about how objects are created. Behavioral patterns are about how objects talk to each other and share the work. In this lab:

* the shipping cost can be calculated in different ways (Strategy),
* an auction feed tells other objects when a new car appears (Observer),
* each new car goes through a line of checks that decide if it is worth importing (Chain of Responsibility).

## Project Structure

```
Lab3/
  Models/
    Car.cs                    car data, read only
    CarBuilder.cs             Builder from Lab2
    FuelType.cs
  Taxes/
    IExciseRule.cs            one excise rule per fuel type
    ExciseRuleFactory.cs      Factory Method from Lab2
    PetrolExcise.cs, DieselExcise.cs, HybridExcise.cs, ElectricExcise.cs
  Services/
    ImportSettings.cs         Singleton from Lab2
    ImportCostCalculator.cs   adds up the total, gets a shipping strategy
  Shipping/
    IShippingStrategy.cs      Strategy: common interface
    TruckShipping.cs          car transporter truck
    DriveHomeShipping.cs      drive the car home yourself
  Checks/
    CarCheck.cs               Chain of Responsibility: base handler
    AgeCheck.cs
    MileageCheck.cs
    BudgetCheck.cs
  Auction/
    ICarObserver.cs           Observer: common interface
    AuctionFeed.cs            the subject that sends the notifications
    CostPrinter.cs            observer that prints the costs
    DealChecker.cs            observer that runs the checks
  Program.cs
```

## Strategy

**Definition:** defines a family of algorithms, puts each one in its own class and makes them interchangeable. The object that uses the algorithm can switch between them without changing its own code.

A car can come from France on a car transporter truck, or the buyer can fly there and drive it home. Both answer the same question, "how much does shipping cost?", but they calculate it differently. Each way is its own class behind one interface:

```csharp
public interface IShippingStrategy
{
    string Name { get; }
    decimal CalculateShipping(Car car);
}
```

```csharp
public class TruckShipping : IShippingStrategy
{
    public string Name { get; } = "Car transporter truck";

    public decimal CalculateShipping(Car car)
    {
        return ImportSettings.Instance.ShippingEur;
    }
}
```

```csharp
public class DriveHomeShipping : IShippingStrategy
{
    private const decimal DistanceKm = 2500m;
    private const decimal FuelPerKm = 0.12m;
    private const decimal TransitPlates = 150m;

    public string Name { get; } = "Drive it home";

    public decimal CalculateShipping(Car car)
    {
        return DistanceKm * FuelPerKm + TransitPlates;
    }
}
```

In Lab2 the calculator always added the fixed 900 EUR. Now it gets a strategy through its constructor and only knows the interface:

```csharp
public class ImportCostCalculator
{
    private readonly IShippingStrategy _shipping;

    public ImportCostCalculator(IShippingStrategy shipping)
    {
        _shipping = shipping;
    }

    public decimal CalculateTotal(Car car)
    {
        var settings = ImportSettings.Instance;
        return car.Price + CalculateExcise(car) / settings.MdlPerEur + _shipping.CalculateShipping(car) + car.Price * settings.ProcedureFeeRate;
    }
}
```

`Program.cs` gives the same car to a calculator with each strategy and prints both totals. A new way to ship, for example by train, is one new class. The calculator does not change. This is also the Dependency Inversion principle from Lab1, which comes back here: the calculator depends on the interface, not on a concrete shipping class.

The excise rules from Lab1 follow the same idea: one interface, one class per fuel type, and the right one is picked for each car.

## Observer

**Definition:** an object (the subject) keeps a list of other objects (the observers) and notifies all of them automatically when something happens. The subject does not need to know what the observers do.

My scrapers collect new auction lots. When a new car appears, several parts of the program want to react to it. `AuctionFeed` is the subject. It keeps a list of observers and calls each one when a car is added:

```csharp
public interface ICarObserver
{
    void OnNewCar(Car car);
}
```

```csharp
public class AuctionFeed
{
    private readonly List<ICarObserver> _observers = new List<ICarObserver>();

    public void Subscribe(ICarObserver observer)
    {
        _observers.Add(observer);
    }

    public void Unsubscribe(ICarObserver observer)
    {
        _observers.Remove(observer);
    }

    public void AddCar(Car car)
    {
        foreach (var observer in _observers)
        {
            observer.OnNewCar(car);
        }
    }
}
```

There are two observers. `CostPrinter` prints the car with its excise and total. `DealChecker` runs the checks from the next section and prints the verdict:

```csharp
public class CostPrinter : ICarObserver
{
    private readonly ImportCostCalculator _calculator;

    public CostPrinter(ImportCostCalculator calculator)
    {
        _calculator = calculator;
    }

    public void OnNewCar(Car car)
    {
        Console.WriteLine($"New car: {car.Title} ({car.Year}), {car.FuelType}, {car.Mileage} km, {car.EngineCc} cc, {car.Price} EUR");
        Console.WriteLine($"Excise: {_calculator.CalculateExcise(car):N2} lei");
        Console.WriteLine($"Total: {_calculator.CalculateTotal(car):N2} EUR");
    }
}
```

In `Program.cs` they subscribe once, and then every new car reaches both of them:

```csharp
var feed = new AuctionFeed();
feed.Subscribe(new CostPrinter(calculator));
feed.Subscribe(new DealChecker(checks));

foreach (var car in cars)
{
    feed.AddCar(car);
}
```

`AuctionFeed` never mentions `CostPrinter` or `DealChecker`. To also save cars to a file or send a message, I would write one more observer and subscribe it. The feed stays the same.

## Chain of Responsibility

**Definition:** a request is passed along a chain of handlers. Each handler either deals with the request or passes it to the next handler in the chain.

Before importing a car, it should pass a few checks: it is not too old, it does not have too many km, and the total fits the budget. Each check is its own handler. The base class keeps a link to the next handler:

```csharp
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
```

Each check either stops the car with a reason, or calls `base.Check(car)` to pass it on:

```csharp
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
```

`AgeCheck` and `BudgetCheck` look the same, with their own condition. The chain is built in `Program.cs`. `SetNext` returns the handler it received, so the calls can be chained:

```csharp
var checks = new AgeCheck(10);
checks.SetNext(new MileageCheck(200000))
      .SetNext(new BudgetCheck(calculator, 30000));
```

The car goes Age, then Mileage, then Budget. If all three pass, the end of the chain returns "OK, worth importing". The chain stops at the first check that fails. The Volkswagen Passat is both too old and has too many km, but only "too old" is printed, because the age check stops it before the mileage check runs. The order also saves work: the cheap checks run first, and the total is only calculated for cars that passed them.

Adding a check, for example a minimum Euro norm, is one new class and one more `SetNext` call. The other checks do not change.

## The Other Behavioral Patterns

* **Command:** turns a request into an object, so it can be stored, queued or undone. Here it could be a list of actions on a saved car list (add a car, remove a car), each with an `Undo`.
* **Interpreter:** defines a small language and a class that evaluates sentences in it. Here it could read a search filter like `diesel and km < 200000 and year > 2018` and turn it into checks.
* **Iterator:** walks through a collection one element at a time without showing how the collection is stored. The `foreach` loops in this project use it: `List<Car>` gives an enumerator with `MoveNext()` and `Current`.
* **Mediator:** objects talk to one central object instead of to each other directly. Here it could be a deal desk that connects the scraper, the calculator and the notifier, so none of them knows about the others.
* **Memento:** saves the state of an object so it can be restored later without exposing its fields. Here it could save the import settings before changing the exchange rate and restore them after a "what if" calculation.
* **State:** an object changes its behavior when its internal state changes, with one class per state. Here an auction lot could be Upcoming, Live, Sold or Withdrawn, and only a Live lot accepts bids.
* **Template Method:** a base class defines the steps of an algorithm and lets subclasses fill in some of them. Here a base report class could print header, body and footer in that order, while a console report and a CSV report each write their own body.
* **Visitor:** adds a new operation to a group of classes without changing them, by putting the operation in a separate visitor class. Here a visitor could go over the excise rules and print each one's rates for a report, without adding a print method to every rule.

## Output

```
New car: Toyota Camry (2020), Petrol, 15000 km, 2500 cc, 25000 EUR
Excise: 77,850.00 lei
Total: 29,895.17 EUR
Verdict: OK, worth importing

New car: Honda Civic (2019), Diesel, 20000 km, 1800 cc, 20000 EUR
Excise: 56,052.00 lei
Total: 23,784.52 EUR
Verdict: OK, worth importing

New car: Tesla Model 3 (2021), Electric, 5000 km, 0 cc, 35000 EUR
Excise: 0.00 lei
Total: 36,040.00 EUR
Verdict: Skip, over budget (36,040.00 EUR, max 30,000.00)

New car: Skoda Octavia (2019), Diesel, 240000 km, 1598 cc, 11000 EUR
Excise: 49,761.72 lei
Total: 14,433.79 EUR
Verdict: Skip, too many km (240000 km, max 200000)

New car: Volkswagen Passat (2011), Diesel, 280000 km, 1968 cc, 6500 EUR
Excise: 61,283.52 lei
Total: 10,492.28 EUR
Verdict: Skip, too old (15 years, max 10)

Shipping options for the Toyota Camry:
Car transporter truck: 900.00 EUR, total 29,895.17 EUR
Drive it home: 450.00 EUR, total 29,445.17 EUR
```

With the truck strategy, the totals of the first three cars are the same as in Lab1 and Lab2. The age in the Passat's verdict is counted from the current year (2026 here).

## How to Run

From the repository root, with the .NET 10 SDK installed:

```
dotnet run --project Lab3
```

## Conclusion

Each pattern moved one decision into its own small classes. Strategy lets the calculator use any shipping method without knowing which one, Observer lets the auction feed notify any number of listeners without knowing what they do, and Chain of Responsibility splits the import decision into separate checks that run in order and stop at the first problem. Adding a new shipping method, a new listener or a new check now means adding one class, not editing the existing ones.
