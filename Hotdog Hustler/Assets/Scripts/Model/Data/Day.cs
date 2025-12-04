using UnityEngine;
using System.Collections.Generic;

public class Day
{
  public double moneyMade = 0;
  public List<(Order, bool)> AccuracyPerOrder = new(); //bool will later be changed to a double Accuracy parameter

  public void AddCustomerServed(Order order, bool wasCustomerHappy)
  {
    AccuracyPerOrder.Add((order, wasCustomerHappy));
    moneyMade = moneyMade + order.wantedDish.price;
    Debug.Log("moneyMade: " + moneyMade);
    Debug.Log("orders: " + AccuracyPerOrder.Count);
  }

  public int GetCustomersServed()
  {
    return AccuracyPerOrder.Count;
  }

  public double GetTotalAccuracy()
  {
    double totalAccuracy = 0;
    foreach (var entry in AccuracyPerOrder)
    {
      if (entry.Item2) //this will also change from a bool check to an accuracy Get
        totalAccuracy ++;
    }
    return totalAccuracy;
  }
}
