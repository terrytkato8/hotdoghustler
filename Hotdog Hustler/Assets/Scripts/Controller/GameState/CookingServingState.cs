using System;
using UnityEngine;

public class CookingServingState : DayState
{
  public override void Enter()
  {
    base.Enter();
    if (CustomerManager.IsActive())
      CustomerManager.Activate();
    Debug.Log("Entered CookingServing State");
  }

  protected override void AddListeners()
  {
    base.AddListeners();
    GameInput.OnMovementCancelled += OnMoveStop;
    ToppingStation.OnToppingStationActivated += OnToppingStationActivated;
  }

  protected override void RemoveListeners()
  {
    base.RemoveListeners();
    GameInput.OnMovementCancelled -= OnMoveStop;
    ToppingStation.OnToppingStationActivated -= OnToppingStationActivated;
  }

  protected override void OnInteraction(object sender, EventArgs e)
  {
    Player.HandleInteractions();
  }

  protected override void OnMove(object sender, OnMovementEventArgs e)
  {
    Vector2 inputVector = GameInput.GetMovementVectorNormalized();
    Player.SetMovementVector(inputVector);
  }

  protected override void OnMoveStop(object sender, EventArgs e)
  {
    Player.SetMovementVector(Vector2.zero);
  }

  private void OnToppingStationActivated(object sender, EventArgs e)
  {
    owner.ChangeState<ToppingState>();
  }
}
