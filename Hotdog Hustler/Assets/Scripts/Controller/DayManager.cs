using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class DayManager : MonoBehaviour
{
  private class DayTimeEvent
  {
    public string name;
    public float timeThreshold;
    public StaticGameEvent eventToFire;
    public bool hasFired;

    public DayTimeEvent(string name, float threshold, StaticGameEvent evt)
    {
      this.name = name;
      timeThreshold = threshold;
      eventToFire = evt;
      hasFired = false;
    }
  }

  [Header("UI References")]
  [SerializeField] private DayClockPanelController dayClockPanelController;
  [SerializeField] private EndOfDayPanelController endOfDayPanelController;

  [Header("Day Timing")]
  [SerializeField] private float dayDuration = 30f;

  [Header("Meal Time Thresholds (Percentage of Day Time left.)")]
  [SerializeField] private float lunchStartTime = 0.7f;
  [SerializeField] private float lunchEndTime = 0.5f;
  [SerializeField] private float dinnerStartTime = 0.3f;
  [SerializeField] private float dinnerEndTime = 0.15f;

  // Runtime State
  private float timeRemaining;
  private bool isActive;
  private List<ServedOrder> servedOrders;
  private List<DayTimeEvent> scheduledEvents;


  // Global Events
  public static readonly StaticGameEvent OnLunchStart = new();
  public static readonly StaticGameEvent OnLunchEnd = new();
  public static readonly StaticGameEvent OnDinnerStart = new();
  public static readonly StaticGameEvent OnDinnerEnd = new();
  public static readonly StaticGameEvent On10SecondsLeft = new();
  public static readonly StaticGameEvent OnDayTimeIsUp = new();

  public void Activate()
  {
    isActive = true;
    SetupDayEvents();
    servedOrders = new List<ServedOrder>();
    dayClockPanelController.Show(dayDuration);
  }

  public void Deactivate()
  {
    isActive = false;
    dayClockPanelController.Hide();
    endOfDayPanelController.Hide();
  }

  private void Update()
  {
    if (!isActive) return;

    timeRemaining = Mathf.Max(0, timeRemaining -= Time.deltaTime);
    dayClockPanelController.UpdateClock(timeRemaining);

    foreach (var dayTimeEvent in scheduledEvents)
    {
      if (!dayTimeEvent.hasFired && timeRemaining <= dayTimeEvent.timeThreshold)
      {
        dayTimeEvent.eventToFire?.Invoke(this, EventArgs.Empty);
        dayTimeEvent.hasFired = true;
        Debug.Log(dayTimeEvent.name);
      }
    }
  }

  private void SetupDayEvents()
  {
    timeRemaining = dayDuration;
    scheduledEvents = new()
    {
      // 1. Lunch (e.g., starts at 70% remaining, ends at 50%)
      new DayTimeEvent("Lunch Start", dayDuration * lunchStartTime, OnLunchStart),
      new DayTimeEvent("Lunch End", dayDuration * lunchEndTime, OnLunchEnd),

      // 2. Dinner (e.g., starts at 30% remaining, ends at 15%)
      new DayTimeEvent("Dinner Start", dayDuration * dinnerStartTime, OnDinnerStart),
      new DayTimeEvent("Dinner End", dayDuration * dinnerEndTime, OnDinnerEnd),

      // 3. Global Warnings
      new DayTimeEvent("10 Seconds", 10f, On10SecondsLeft),
      new DayTimeEvent("Day Over", 0f, OnDayTimeIsUp)
    };
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
}
