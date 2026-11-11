# TMPS

Semester project for the Software Design Techniques and Mechanisms course (TMPS) at the Technical University of Moldova.

## About

A C# console application that estimates the cost of importing a used car from the EU into Moldova. It takes a car's price and adds the Moldovan excise tax, which depends on the fuel type and the engine size, plus shipping and the customs procedure fee.

The project grows during the semester. Each lab builds on the same code and applies a new set of design principles or patterns. The plan is to run it on real listings collected by my own car auction scrapers.

## Labs

| Lab | Topic | Status | Report |
|-----|-------|--------|--------|
| Lab1 | SOLID principles | Done | [Lab1/README.md](Lab1/README.md) |
| Lab2 | Creational patterns (Singleton, Factory Method, Builder) | Done | [Lab2/README.md](Lab2/README.md) |
| Lab3 | Behavioral patterns (Strategy, Observer, Chain of Responsibility) | Done | [Lab3/README.md](Lab3/README.md) |

## Structure

Each lab is its own project in its own folder and starts as a copy of the previous one.

```
Lab3/
  Models/     car data (Car, FuelType) and CarBuilder
  Taxes/      excise rules, one class per fuel type, and ExciseRuleFactory
  Services/   ImportSettings and ImportCostCalculator
  Shipping/   shipping strategies
  Checks/     chain of import checks
  Auction/    auction feed and its observers
  Program.cs
```

## Running

Requires the .NET 10 SDK.

```
dotnet run --project Lab1
dotnet run --project Lab2
dotnet run --project Lab3
```
