# Laboratory Work 1: SOLID Principles

**Author:** Moroșanu Ion, FAF-242
**Course:** Software Design Techniques and Mechanisms (TMPS)

## Task

Create a project that applies three SOLID principles. I chose:

* Single Responsibility Principle (SRP)
* Open/Closed Principle (OCP)
* Dependency Inversion Principle (DIP)

## Domain

The project calculates how much it costs to import a used car from the EU into Moldova. I picked this domain because I already scrape EU car auctions, so later labs can run on real listings instead of made up data.

Besides the price of the car, the biggest cost is the Moldovan excise tax. It is the engine volume in cm³ multiplied by a rate in lei, and the rate depends on the fuel type and the engine size:

| Fuel | Engine size (cm³) | Rate (lei per cm³) |
|------|-------------------|--------------------|
| Petrol | up to 1000 | 9.56 |
| Petrol | up to 1500 | 12.23 |
| Petrol | up to 2000 | 18.90 |
| Petrol | up to 3000 | 31.14 |
| Petrol | over 3000 | 55.60 |
| Diesel | up to 1500 | 12.23 |
| Diesel | up to 2500 | 31.14 |
| Diesel | over 2500 | 55.60 |
| Hybrid | any | 75% of the petrol amount |
| Electric | any | exempt |

These are the rates for cars up to 2 years old. Older cars pay more, which the project does not model yet.

The total import cost in EUR is:

```
total = price + excise / 19.9863 + 900 (shipping estimate) + 0.4% of the price (customs procedure fee)
```

where 19.9863 is the MDL/EUR exchange rate.

## Project Structure

```
Lab1/
  Models/
    Car.cs                    car data
    FuelType.cs               Petrol, Diesel, Electric, Hybrid
  Taxes/
    IExciseRule.cs            interface for one excise rule
    PetrolExcise.cs
    DieselExcise.cs
    HybridExcise.cs
    ElectricExcise.cs
  Services/
    ImportCostCalculator.cs   finds the right rule and adds up the total
  Program.cs                  creates the objects, connects them and prints the results
```

## Single Responsibility Principle

**Definition:** a class should have only one reason to change.

Every class in the project has one job:

* `Car` only holds the data of a car. It does not calculate or print anything.
* Each excise class knows the tax for one fuel type and nothing else.
* `ImportCostCalculator` only does the cost math. It does not create the rules, store cars or print.
* `Program.cs` creates the objects, connects them and prints the results.

So if the diesel tax changes, only `DieselExcise.cs` changes. If the shipping price changes, only the calculator changes.

```csharp
public class DieselExcise : IExciseRule
{
    public FuelType FuelType { get; } = FuelType.Diesel;
    public decimal CalculateExcise(Car car)
    {
        if (car.EngineCc <= 1500)
        {
            return car.EngineCc * 12.23m;
        }
        else if (car.EngineCc <= 2500)
        {
            return car.EngineCc * 31.14m;
        }
        else
        {
            return car.EngineCc * 55.6m;
        }
    }
}
```

## Open/Closed Principle

**Definition:** classes should be open for extension but closed for modification.

Every fuel type has its own class that implements the same interface:

```csharp
public interface IExciseRule
{
    FuelType FuelType { get; }
    decimal CalculateExcise(Car car);
}
```

Nowhere in the code is there an if/else chain that checks the fuel type to choose a formula. The calculator asks each rule whether it is for the car's fuel and lets that rule do the calculation. To support LPG cars, I would add an `LpgExcise` class and one line to the list of rules. No existing class would change.

The hybrid rule was added the same way, as a new class. Instead of copying the petrol rates, it reuses the petrol rule, so the rates exist in one place only:

```csharp
public class HybridExcise : IExciseRule
{
    public FuelType FuelType { get; } = FuelType.Hybrid;
    public decimal CalculateExcise(Car car)
    {
        return new PetrolExcise().CalculateExcise(car) * 0.75m;
    }
}
```

## Dependency Inversion Principle

**Definition:** high level modules should not depend on low level modules. Both should depend on abstractions.

`ImportCostCalculator` is the high level class. It only knows the `IExciseRule` interface and never mentions `PetrolExcise`, `DieselExcise` or any other concrete rule. The rules are given to it through the constructor (constructor injection):

```csharp
public class ImportCostCalculator
{
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
}
```

`Program.cs` decides which rules exist and passes them in:

```csharp
var exciseRules = new List<IExciseRule>
{
    new PetrolExcise(),
    new DieselExcise(),
    new ElectricExcise(),
    new HybridExcise()
};

var calculator = new ImportCostCalculator(exciseRules);
```

Because of this, the calculator can work with a different set of rules, for example rules for older cars or a fake rule in a test, without any change to its code. The constructor also makes it impossible to create a calculator without rules, since the code does not compile without them. If a car's fuel has no rule, the calculator throws an exception instead of returning a wrong number.

## Output

```
Toyota Camry (2020), Petrol, Mileage: 15000 km, Engine: 2500 cc, Price: 25000 EUR
Excise: 77,850.00 lei
Total: 29,895.17 EUR

Honda Civic (2019), Diesel, Mileage: 20000 km, Engine: 1800 cc, Price: 20000 EUR
Excise: 56,052.00 lei
Total: 23,784.52 EUR

Tesla Model 3 (2021), Electric, Mileage: 5000 km, Engine: 0 cc, Price: 35000 EUR
Excise: 0.00 lei
Total: 36,040.00 EUR
```

## How to Run

From the repository root, with the .NET 10 SDK installed:

```
dotnet run --project Lab1
```

## Conclusion

Splitting the program into small classes made it longer than one file with an if/else chain, but now each part can be found and changed on its own. Adding a fuel type means adding a class, the calculator does not care which rules it gets, and every class has one reason to change. The next steps for this project are loading real auction listings and adding the age based excise rates, and both should fit into this structure as new classes rather than edits to the existing ones.
