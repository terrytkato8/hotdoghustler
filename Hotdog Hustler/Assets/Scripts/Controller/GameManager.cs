using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : StateMachine 
{
  [Header("References")]
  [SerializeField] private GameInput gameInput;
  [SerializeField] private Player player;
  [SerializeField] private ToppingMenuPanelController toppingMenuPanelController;
  [SerializeField] private CustomerManager customerManager;
  [SerializeField] private DayClockPanelController dayClockPanelController;
  private Day day = new();

  void Start ()
  {
    //ChangeState<MainMenuState>(); //when we have a mainmenustate, the game starts with that
    ChangeState<InitState>();
  }

  //GETTER
  public GameInput GetGameInput() { return gameInput; }
  public Player GetPlayer() { return player; }
  public ToppingMenuPanelController GetToppingMenuPanelController() { return toppingMenuPanelController; }
  public CustomerManager GetCustomerManager() { return customerManager ;}
  public DayClockPanelController GetDayClockPanelController() { return dayClockPanelController; }
  public Day GetDay() { return day; }
  public void SetDay(Day day) { this.day = day; }
}
