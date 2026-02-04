using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : StateMachine 
{
  [Header("References")]
  [SerializeField] private GameInput gameInput;
  [SerializeField] private Player player;
  [SerializeField] private CustomerManager customerManager;
  [SerializeField] private DayManager dayManager;
  [SerializeField] private ProgressionManager progressionManager;
  [SerializeField] private ToppingMenuPanelController toppingMenuPanelController;
  [SerializeField] private ShopMenuPanelController shopMenuPanelController;
  [SerializeField] private CookStation grillstation;
  [SerializeField] private CookStation toastStation;
  [SerializeField] private ToppingStation toppingStation;
  [SerializeField] private ServingStation servingStation;
  [SerializeField] private TutorialManager tutorialManager;
  [SerializeField] private MainMenuPanelController mainMenuPanelController;
  [SerializeField] private MainAudio audioManager;
  public int currentDay;

  void Start ()
  {
    ChangeState<InitGameState>();
  }

  //GETTER
  public GameInput GetGameInput() { return gameInput; }
  public Player GetPlayer() { return player; }
  public ToppingMenuPanelController GetToppingMenuPanelController() { return toppingMenuPanelController; }
  public ShopMenuPanelController GetShopMenuPanelController() { return shopMenuPanelController; }
  public CustomerManager GetCustomerManager() { return customerManager ;}
  public DayManager GetDayManager() { return dayManager; }
  public ProgressionManager GetProgressionManager() { return progressionManager; }
  public CookStation GetGrillStation() { return grillstation; }
  public CookStation GetToastStation() { return toastStation; }
  public ToppingStation GetToppingStation() { return toppingStation; }
  public ServingStation GetServingStation() { return servingStation; }
  public TutorialManager GetTutorialManager() { return tutorialManager; }
  public MainMenuPanelController GetMainMenuPanelController() { return  mainMenuPanelController; }
  public MainAudio GetAudioManager() { return audioManager; }
}
