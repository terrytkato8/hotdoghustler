using UnityEngine;
using System;
using System.Collections.Generic;

public class ShoppingState : BaseGameState
{
  private double currentBalance;

  public override void Enter()
  {
    base.Enter();
    Debug.Log("entered shopping State");

    List<ToppingSO> lockedToppings = ProgressionManager.GetLockedToppings();
    currentBalance = ProgressionManager.GetCurrentBalance();
    ShopMenuPanelController.Show(currentBalance, lockedToppings);

    ProgressionManager.SaveGame();
  }

  public override void Exit()
  {
    base.Exit();

    ShopMenuPanelController.Hide();
  }

  protected override void OnInteraction(object sender, EventArgs e)
  {
    ToppingSO topping = ShopMenuPanelController.GetSelectedTopping();
    if (topping != null)
    {
      if (topping.unlockPrice <= currentBalance)
      {
        currentBalance -= topping.unlockPrice;
        ProgressionManager.UnlockTopping(topping);
        ShopMenuPanelController.BuyTopping(topping, currentBalance);
      }
    }
    else
    {
      ProgressionManager.SetCurrentBalance(currentBalance);
      owner.ChangeState<InitDayState>();
    }
  }

  protected override void OnMove(object sender, OnMovementEventArgs e)
  {
    ShopMenuPanelController.Navigate(e.inputVector);
  }
}
