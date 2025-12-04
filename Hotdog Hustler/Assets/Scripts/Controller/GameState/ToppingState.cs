using System;
using UnityEngine;
using static UnityEngine.Rendering.GPUSort;

public class ToppingState : DayState
{
  public override void Enter()
  {
    base.Enter();

    // Show UI
    ToppingMenuPanelController.Show();
    Debug.Log("Entered Topping State");
  }

  public override void Exit()
  {
    base.Exit();

    // Hide UI 
    ToppingMenuPanelController.Hide();
  }

  protected override void OnInteraction(object sender, EventArgs e)
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
    ToppingMenuPanelController.Navigate(e.inputVector);
  }
}
