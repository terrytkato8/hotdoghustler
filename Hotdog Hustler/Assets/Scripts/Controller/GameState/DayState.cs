using UnityEngine;
using System;
using System.Collections;

public abstract class DayState : BaseGameState
{
  protected override void AddListeners()
  {
    base.AddListeners();
    CustomerManager.OnCustomerServed += OnCustomerServed;
    DayManager.OnDayTimeIsUp += OnDayTimeIsUp;
  }

  protected override void RemoveListeners()
  {
    base.RemoveListeners();
    CustomerManager.OnCustomerServed -= OnCustomerServed;
    DayManager.OnDayTimeIsUp -= OnDayTimeIsUp;
  }

  private void OnCustomerServed(object sender, OnCustomerServedEventArgs e)
  {
    DayManager.AddCustomerServed(e.servedOrder);
  }

  private void OnDayTimeIsUp(object sender, EventArgs e)
  {
    CustomerManager.Deactivate();
    DayManager.Deactivate();
    Grillstation.SetIdle();
    ToastStation.SetIdle();

    KitchenObject playerKitchenObject = Player.GetKitchenObject();
    if (playerKitchenObject != null)
      Player.GetKitchenObject().DestroySelf();

    owner.ChangeState<EndOfDayState>();
  }
}
