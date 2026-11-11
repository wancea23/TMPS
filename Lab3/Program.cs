using Lab3.Auction;
using Lab3.Checks;
using Lab3.Models;
using Lab3.Services;
using Lab3.Shipping;

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
        .WithTitle("Skoda Octavia")
        .WithYear(2019)
        .WithFuelType(FuelType.Diesel)
        .WithMileage(240000)
        .WithEngineCc(1598)
        .WithPrice(11000)
        .Build(),
    new CarBuilder()
        .WithTitle("Volkswagen Passat")
        .WithYear(2011)
        .WithFuelType(FuelType.Diesel)
        .WithMileage(280000)
        .WithEngineCc(1968)
        .WithPrice(6500)
        .Build(),
};

var calculator = new ImportCostCalculator(new TruckShipping());

var checks = new AgeCheck(10);
checks.SetNext(new MileageCheck(200000))
      .SetNext(new BudgetCheck(calculator, 30000));

var feed = new AuctionFeed();
feed.Subscribe(new CostPrinter(calculator));
feed.Subscribe(new DealChecker(checks));

foreach (var car in cars)
{
    feed.AddCar(car);
}

var camry = cars[0];
var shippingOptions = new List<IShippingStrategy> { new TruckShipping(), new DriveHomeShipping() };

Console.WriteLine($"Shipping options for the {camry.Title}:");
foreach (var shipping in shippingOptions)
{
    var shippingCalculator = new ImportCostCalculator(shipping);
    Console.WriteLine($"{shipping.Name}: {shipping.CalculateShipping(camry):N2} EUR, total {shippingCalculator.CalculateTotal(camry):N2} EUR");
}
