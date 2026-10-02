namespace Lab1.Models;

public class Car
{
    public string Title { get; set; } = "";
    public int Year { get; set; }
    public FuelType FuelType { get; set; }
    public int Mileage { get; set; }
    public int EngineCc { get; set; }
    public decimal Price { get; set; }
}