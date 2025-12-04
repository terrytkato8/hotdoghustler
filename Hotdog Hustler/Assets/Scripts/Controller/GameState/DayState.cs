using UnityEngine;
using System;

public abstract class DayState : BaseGameState
{
  public override void Enter()
  {
    base.Enter();
    if (CustomerManager.IsActive())
      CustomerManager.Activate();
  }

  protected override void AddListeners()
  {
    base.AddListeners();
    CustomerManager.OnCustomerServed += OnCustomerServed;
    DayClockPanelController.OnDayTimeIsUp += OnDayTimeIsUp;
  }

  protected override void RemoveListeners()
  {
    base.RemoveListeners();
    CustomerManager.OnCustomerServed -= OnCustomerServed;
  }

  private void OnCustomerServed(object sender, OnCustomerServedEventArgs e)
  {
    Day.AddCustomerServed(e.order, e.wasCustomerHappy);
  }

  private void OnDayTimeIsUp(object sender, EventArgs e)
  {
    CustomerManager.Deactivate();
    DayClockPanelController.Hide();
    owner.ChangeState<EndOfDayState>();
  }
}
