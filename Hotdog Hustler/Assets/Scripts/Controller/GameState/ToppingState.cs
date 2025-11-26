using System;
using UnityEngine;
using static UnityEngine.Rendering.GPUSort;

public class ToppingState : GameState
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

  protected override void AddListeners()
  {
    //ToppingStation.OnToppingStationActivated += OnToppingStationExited;
  }

  protected override void RemoveListeners()
  {
    //ToppingStation.OnToppingStationActivated -= OnToppingStationExited;
  }

  protected override void OnInteraction(object sender, EventArgs e)
  {
    ToppingMenuPanelController.SelectCurrentOption();
  }

  protected override void OnMove(object sender, OnMovementEventArgs e)
  {
    ToppingMenuPanelController.Navigate(e.inputVector);
  }

  private void OnToppingStationExited(object sender, EventArgs e)
  {
    owner.ChangeState<CookingServingState>(); 
  }
}
