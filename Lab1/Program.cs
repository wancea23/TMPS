using Lab1.Models;
using Lab1.Taxes;
using Lab1.Services;

var car = new List<Car>
{
    new Car { Title = "Toyota Camry", Year = 2020, FuelType = FuelType.Petrol, Mileage = 15000, Enginecc = 2500, Price = 25000 },
    new Car { Title = "Honda Civic", Year = 2019, FuelType = FuelType.Diesel, Mileage = 20000, Enginecc = 1800, Price = 20000 },
    new Car { Title = "Tesla Model 3", Year = 2021, FuelType = FuelType.Electric, Mileage = 5000, Enginecc = 0, Price = 35000 },
};

var exciseRule = new List<IExciseRule>
{
    new PetrolExcise(),
    new DieselExcise(),
    new ElectricExcise(),
    new HybridExcise()
};

var calculator = new ImportCostCalculator(exciseRule);

foreach (var c in car)
{
    Console.WriteLine($"{c.Title} ({c.Year}) - {c.FuelType}, Mileage: {c.Mileage} km, Engine: {c.Enginecc} cc, Price: ${c.Price}");
    Console.WriteLine($"Excise: {calculator.CalculateExcise(c)} lei");
    Console.WriteLine($"Total: {calculator.CalculateTotal(c):N2} EUR");
}