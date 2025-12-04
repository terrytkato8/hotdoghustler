using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class BaseGameState : State 
{
  protected GameManager owner;
  protected ToppingMenuPanelController ToppingMenuPanelController { get { return owner.GetToppingMenuPanelController(); }}
  protected GameInput GameInput { get { return owner.GetGameInput(); }}
  protected Player Player { get { return owner.GetPlayer(); }}
  protected CustomerManager CustomerManager { get {  return owner.GetCustomerManager(); }}
  protected DayClockPanelController DayClockPanelController { get {  return owner.GetDayClockPanelController(); }}
  protected Day Day { set { owner.SetDay(value); } get { return owner.GetDay(); } }


  public override void Enter()
  {
    owner = GetComponent<GameManager>();
    base.Enter();
  }

  protected override void AddListeners ()
  {
    GameInput.OnInteractAction += OnInteraction;
    GameInput.OnMovementPerformed += OnMove;
    Debug.Log("AddListeners");
  }
  
  protected override void RemoveListeners ()
  {
    GameInput.OnInteractAction -= OnInteraction;
    GameInput.OnMovementPerformed -= OnMove;
  }
  
  protected virtual void OnInteraction (object sender, EventArgs e)
  {
    
  }

  protected virtual void OnMove(object sender, OnMovementEventArgs e)
  {

  }

  protected virtual void OnMoveStop(object sender, EventArgs e)
  {

  }
}
