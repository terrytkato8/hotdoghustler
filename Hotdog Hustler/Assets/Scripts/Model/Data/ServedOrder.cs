using System.Transactions;
using UnityEngine;

public struct ServedOrder
{
  public Order order;
  public double accuracy;
  public double moneyPaid;

  public ServedOrder (Order order, double accuracy, double moneyPaid)
  {
    this.order = order;
    this.accuracy = accuracy;
    this.moneyPaid = moneyPaid;
  }
}
