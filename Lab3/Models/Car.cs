namespace Lab3.Models;

public class Car
{
    public string Title { get; }
    public int Year { get; }
    public FuelType FuelType { get; }
    public int Mileage { get; }
    public int EngineCc { get; }
    public decimal Price { get; }

    public Car(string title, int year, FuelType fuelType, int mileage, int engineCc, decimal price)
    {
        Title = title;
        Year = year;
        FuelType = fuelType;
        Mileage = mileage;
        EngineCc = engineCc;
        Price = price;
    }
}
