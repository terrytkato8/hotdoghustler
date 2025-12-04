using UnityEngine;
using System;

public class OnCustomerServedEventArgs : EventArgs
{
  public Order order;
  public bool wasCustomerHappy; //will later be changed to a double Accuracy parameter
}
