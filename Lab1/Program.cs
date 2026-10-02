using Lab1.Models;
using Lab1.Taxes;
using Lab1.Services;

var cars = new List<Car>
{
    new Car { Title = "Toyota Camry", Year = 2020, FuelType = FuelType.Petrol, Mileage = 15000, EngineCc = 2500, Price = 25000 },
    new Car { Title = "Honda Civic", Year = 2019, FuelType = FuelType.Diesel, Mileage = 20000, EngineCc = 1800, Price = 20000 },
    new Car { Title = "Tesla Model 3", Year = 2021, FuelType = FuelType.Electric, Mileage = 5000, EngineCc = 0, Price = 35000 },
};

var exciseRules = new List<IExciseRule>
{
    new PetrolExcise(),
    new DieselExcise(),
    new ElectricExcise(),
    new HybridExcise()
};

var calculator = new ImportCostCalculator(exciseRules);

foreach (var car in cars)
{
    Console.WriteLine($"{car.Title} ({car.Year}), {car.FuelType}, Mileage: {car.Mileage} km, Engine: {car.EngineCc} cc, Price: {car.Price} EUR");
    Console.WriteLine($"Excise: {calculator.CalculateExcise(car):N2} lei");
    Console.WriteLine($"Total: {calculator.CalculateTotal(car):N2} EUR");
    Console.WriteLine();
}
