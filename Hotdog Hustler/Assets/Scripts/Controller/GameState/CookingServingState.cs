using System;
using UnityEngine;

public class CookingServingState : DayState
{
  public override void Enter()
  {
    base.Enter();
    Debug.Log("Entered CookingServing State");
    if (TutorialManager.IsActive())
    {
      TutorialManager.AdvanceTutorial();
    }
  }

  public override void Exit() 
  {
    base.Exit();
    Player.SetMovementVector(Vector2.zero);
  }

  protected override void AddListeners()
  {
    base.AddListeners();
    GameInput.OnMovementCancelled += OnMoveStop;
    ToppingStation.OnToppingStationActivated.AddListener(ToppingStation_OnToppingStationActivated);
    CustomerManager.OnCustomerServed.AddListener(CustomerManager_OnCustomerServed);
    TutorialManager.OnTutorialOrderServed.AddListener(TutorialManager_OnTutorialOrderServed);
  }

  protected override void RemoveListeners()
  {
    base.RemoveListeners();
    GameInput.OnMovementCancelled -= OnMoveStop;
    ToppingStation.OnToppingStationActivated.RemoveListener(ToppingStation_OnToppingStationActivated);
    CustomerManager.OnCustomerServed.RemoveListener(CustomerManager_OnCustomerServed);
    TutorialManager.OnTutorialOrderServed.RemoveListener(TutorialManager_OnTutorialOrderServed);
  }

  protected override void OnRegularInteraction()
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

  private void ToppingStation_OnToppingStationActivated(object sender, KitchenObjectEventArgs e)
  {
    owner.ChangeState<ToppingState>();
  }

  private void CustomerManager_OnCustomerServed(object sender, OnCustomerServedEventArgs e)
  {
    DayManager.AddCustomerServed(e.servedOrder);
  }

  private void TutorialManager_OnTutorialOrderServed(object sender, EventArgs e)
  {
    DayManager.Activate(CurrentDay);
  }
}
