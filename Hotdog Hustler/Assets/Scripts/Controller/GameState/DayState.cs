using UnityEngine;
using System;
using System.Collections;

public abstract class DayState : BaseGameState
{
  protected override void AddListeners()
  {
    base.AddListeners();
    DayManager.OnDayTimeIsUp.AddListener(OnDayTimeIsUp);
  }

  protected override void RemoveListeners()
  {
    base.RemoveListeners();
    DayManager.OnDayTimeIsUp.RemoveListener(OnDayTimeIsUp);
  }

  private void OnDayTimeIsUp(object sender, EventArgs e)
  {
    CustomerManager.Deactivate(() =>
    {
      Debug.Log("Day ended.");

      DayManager.Deactivate();
      Grillstation.SetIdle();
      ToastStation.SetIdle();

      KitchenObject playerKitchenObject = Player.GetKitchenObject();
      if (playerKitchenObject != null)
      {
        playerKitchenObject.DestroySelf();
      }

      owner.ChangeState<EndOfDayState>();
    });
  }
}
