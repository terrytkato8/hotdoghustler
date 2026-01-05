using System;
using System.Collections.Generic;
using UnityEngine;

public class DayManager : MonoBehaviour
{
  [SerializeField] private DayClockPanelController dayClockPanelController;
  [SerializeField] private EndOfDayPanelController endOfDayPanelController;
  private List<ServedOrder> servedOrders;

  [SerializeField] private float dayTimer = 30f;
  private float timeRemaining;
  private bool isActive = false;

  public event EventHandler OnDayTimeIsUp;
  private bool hasInvokedEvent;

  public void Activate()
  {
    isActive = true;
    hasInvokedEvent = false;
    timeRemaining = dayTimer;
    servedOrders = new List<ServedOrder>();
    dayClockPanelController.Show(dayTimer);
  }

  public void Deactivate()
  {
    isActive = false;
    hasInvokedEvent = false;
    dayClockPanelController.Hide();
    endOfDayPanelController.Hide();
  }

  public void Update()
  {
    if (isActive)
    {
      timeRemaining = Mathf.Max(0, timeRemaining -= Time.deltaTime);
      dayClockPanelController.UpdateClock(timeRemaining);
      if (timeRemaining <= 0 && !hasInvokedEvent)
      {
        OnDayTimeIsUp?.Invoke(this, EventArgs.Empty);
        Debug.Log("day time is up!");
        hasInvokedEvent = true;
      }
    }
  }

  public void AddCustomerServed(ServedOrder servedOrder)
  {
    servedOrders.Add(servedOrder);
    Debug.Log("order: " + servedOrder.order);
    Debug.Log("accuracy: " + servedOrder.accuracy);
    Debug.Log("moneyPaid: " + servedOrder.moneyPaid);
  }

  public int GetCustomersServedCount()
  {
    return servedOrders.Count;
  }

  public double GetTotalAccuracyPercantage()
  {
    double totalAccuracy = 0;
    foreach (var servedOrder in servedOrders)
    {
      totalAccuracy += servedOrder.accuracy;
    }
    return totalAccuracy == 0 ? 0 : (totalAccuracy / GetCustomersServedCount()) * 100;
  }

  public double GetTotalMoneyPaid()
  {
    double totalMoneyPaid = 0;
    foreach (var servedOrder in servedOrders)
    {
      totalMoneyPaid += servedOrder.moneyPaid;
    }
    return totalMoneyPaid;
  }

  public void ShowEndOfDayPanel(int day)
  {
    endOfDayPanelController.Show(
      day,
      GetCustomersServedCount(), 
      GetTotalAccuracyPercantage(), 
      GetTotalMoneyPaid()
    );
  }
}
