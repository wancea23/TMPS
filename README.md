# TMPS

Semester project for the Software Design Techniques and Mechanisms course (TMPS) at the Technical University of Moldova.

## About

A C# console application that estimates the cost of importing a used car from the EU into Moldova. It takes a car's price and adds the Moldovan excise tax, which depends on the fuel type and the engine size, plus shipping and the customs procedure fee.

The project grows during the semester. Each lab builds on the same code and applies a new set of design principles or patterns. The plan is to run it on real listings collected by my own car auction scrapers.

## Labs

| Lab | Topic | Status | Report |
|-----|-------|--------|--------|
| Lab1 | SOLID principles | Done | [Lab1/README.md](Lab1/README.md) |

## Structure

```
Lab1/
  Models/     car data (Car, FuelType)
  Taxes/      excise rules, one class per fuel type
  Services/   ImportCostCalculator
  Program.cs
```

## Running

Requires the .NET 10 SDK.

```
dotnet run --project Lab1
```
