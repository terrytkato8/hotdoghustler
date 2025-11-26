using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class GameState : State 
{
  protected GameManager owner;
  public ToppingMenuPanelController ToppingMenuPanelController { get { return owner.GetToppingMenuPanelController(); }}
  public GameInput GameInput { get { return owner.GetGameInput(); }}
  public Player Player { get { return owner.GetPlayer(); }}

  protected virtual void Awake()
  {
    owner = GetComponent<GameManager>();
  }

  protected override void AddListeners ()
  {
    GameInput.OnInteractAction += OnInteraction;
    GameInput.OnMovementPerformed += OnMove;
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
