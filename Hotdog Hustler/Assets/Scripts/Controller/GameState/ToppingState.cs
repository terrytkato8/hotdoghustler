using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.GPUSort;

public class ToppingState : DayState
{
  public override void Enter()
  {
    base.Enter();

    // Show UI
    List<ToppingSO> unlockedToppings = ProgressionManager.GetUnlockedToppings();
    ToppingMenuPanelController.Show(unlockedToppings);
    Debug.Log("Entered Topping State");
  }

  public override void Exit()
  {
    base.Exit();

    // Hide UI 
    ToppingMenuPanelController.Hide();
  }

  protected override void OnRegularInteraction()
  {
    ToppingSO topping = ToppingMenuPanelController.GetSelectedTopping();
    if (topping != null)
    {
      Player.GetKitchenObject().AddTopping(topping);
    }
    else
    {
      owner.ChangeState<CookingServingState>();
    }
  }

  protected override void OnMove(object sender, OnMovementEventArgs e)
  {
    if (!TutorialManager.IsWaitingForInput())
    {
      ToppingMenuPanelController.Navigate(e.inputVector);
    }
  }
}
