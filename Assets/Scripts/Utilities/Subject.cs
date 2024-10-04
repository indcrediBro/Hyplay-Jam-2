using System.Collections.Generic;
using UnityEngine;

public class Subject : MonoBehaviour
{
    private List<IObserver> observers = new List<IObserver>();

    public void RegisterObserver(IObserver observer)
    {
        observers.Add(observer);
    }

    public void UnregisterObserver(IObserver observer)
    {
        observers.Remove(observer);
    }

    protected void NotifyObservers(int newHealth, int newShield = -1)
    {
        foreach (var observer in observers)
        {
            observer.OnHealthChanged(newHealth);
            if (newShield >= 0)
            {
                observer.OnShieldChanged(newShield);
            }
        }
    }
}
