using Lab2.Models;
using Lab2.Services;

var settings = ImportSettings.Instance;
Console.WriteLine($"Settings: 1 EUR = {settings.MdlPerEur} MDL, shipping {settings.ShippingEur} EUR, procedure fee {settings.ProcedureFeeRate:P1}");
Console.WriteLine($"Same settings object everywhere: {ReferenceEquals(settings, ImportSettings.Instance)}");
Console.WriteLine();

var cars = new List<Car>
{
    new CarBuilder()
        .WithTitle("Toyota Camry")
        .WithYear(2020)
        .WithFuelType(FuelType.Petrol)
        .WithMileage(15000)
        .WithEngineCc(2500)
        .WithPrice(25000)
        .Build(),
    new CarBuilder()
        .WithTitle("Honda Civic")
        .WithYear(2019)
        .WithFuelType(FuelType.Diesel)
        .WithMileage(20000)
        .WithEngineCc(1800)
        .WithPrice(20000)
        .Build(),
    new CarBuilder()
        .WithTitle("Tesla Model 3")
        .WithYear(2021)
        .WithFuelType(FuelType.Electric)
        .WithMileage(5000)
        .WithPrice(35000)
        .Build(),
    new CarBuilder()
        .WithTitle("Toyota RAV4 Hybrid")
        .WithYear(2022)
        .WithFuelType(FuelType.Hybrid)
        .WithMileage(30000)
        .WithEngineCc(2487)
        .WithPrice(28000)
        .Build(),
};

var calculator = new ImportCostCalculator();

foreach (var car in cars)
{
    Console.WriteLine($"{car.Title} ({car.Year}), {car.FuelType}, Mileage: {car.Mileage} km, Engine: {car.EngineCc} cc, Price: {car.Price} EUR");
    Console.WriteLine($"Excise: {calculator.CalculateExcise(car):N2} lei");
    Console.WriteLine($"Total: {calculator.CalculateTotal(car):N2} EUR");
    Console.WriteLine();
}

try
{
    new CarBuilder()
        .WithTitle("Volkswagen Golf")
        .WithYear(2018)
        .WithFuelType(FuelType.Diesel)
        .WithPrice(12000)
        .Build();
}
catch (InvalidOperationException e)
{
    Console.WriteLine($"Builder rejected the car: {e.Message}");
}
