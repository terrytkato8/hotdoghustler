using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public abstract class BaseGameState : State 
{
  protected GameManager owner;
  protected ToppingMenuPanelController ToppingMenuPanelController { get { return owner.GetToppingMenuPanelController(); }}
  protected ShopMenuPanelController ShopMenuPanelController { get { return owner.GetShopMenuPanelController(); }}
  protected GameInput GameInput { get { return owner.GetGameInput(); }}
  protected Player Player { get { return owner.GetPlayer(); }}
  protected CustomerManager CustomerManager { get { return owner.GetCustomerManager(); }}
  protected DayManager DayManager { get { return owner.GetDayManager(); }}
  protected ProgressionManager ProgressionManager { get { return owner.GetProgressionManager(); }}
  protected CookStation Grillstation { get { return owner.GetGrillStation(); }}
  protected CookStation ToastStation { get { return owner.GetToastStation(); }}
  protected ToppingStation ToppingStation { get { return owner.GetToppingStation(); } }
  protected ServingStation ServingStation { get { return owner.GetServingStation(); } }
  protected TutorialManager TutorialManager { get { return owner.GetTutorialManager(); }}
  protected MainMenuPanelController MainMenuPanelController { get { return owner.GetMainMenuPanelController(); }}
  protected MainAudio MainAudio { get { return owner.GetAudioManager(); }}
  protected int CurrentDay { get { return owner.currentDay; } set { owner.currentDay = value; }}

  public override void Enter()
  {
    owner = GetComponent<GameManager>();
    base.Enter();
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
    if (TutorialManager.IsWaitingForInput())
    {
      TutorialManager.AdvanceTutorial();
    }
    else
    {
      OnRegularInteraction();
    }
  }

  protected virtual void OnRegularInteraction()
  {

  }

  protected virtual void OnMove(object sender, OnMovementEventArgs e)
  {

  }

  protected virtual void OnMoveStop(object sender, EventArgs e)
  {

  }
}
