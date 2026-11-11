using Lab3.Models;
namespace Lab3.Auction;

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
