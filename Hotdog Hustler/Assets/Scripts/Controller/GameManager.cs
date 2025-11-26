using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : StateMachine 
{
  [Header("References")]
  [SerializeField] private GameInput gameInput;
  [SerializeField] private Player player;
  [SerializeField] private ToppingMenuPanelController toppingMenuPanelController;

  void Start ()
  {
    //ChangeState<MainMenuState>(); //when we have a mainmenustate, the game starts with that
    ChangeState<CookingServingState>();
  }

  //GETTER
  public GameInput GetGameInput() { return gameInput; }
  public Player GetPlayer() { return player; }
  public ToppingMenuPanelController GetToppingMenuPanelController() { return toppingMenuPanelController; }
}
