using System.Collections;
using UnityEngine;

public class InitDayState : BaseGameState
{
  public override void Enter()
  {
    base.Enter();
    StartCoroutine(Init());
  }

  private IEnumerator Init()
  {
    int currentDay = ProgressionManager.GetDay();

    Debug.Log("starting Day " + currentDay);
    AudioManager.StartDay();

    OrderPanelController OrderPanelController = CustomerManager.GetOrderPanelController();
    int tutorialDay = 1; //activate tutorial on day 1
    bool needTutorial = currentDay == tutorialDay;
    if (needTutorial) 
    {
      TutorialManager.Activate(Grillstation, ToastStation, ServingStation, ToppingStation, OrderPanelController, ToppingMenuPanelController);
    }
    else
    {
      if (TutorialManager.IsActive()) TutorialManager.Deactivate();
      ProgressionManager.IncreaseDay();
      DayManager.Activate(); //when the tutorial is active, the daymanager will be activated later.
    }

    CustomerManager.Activate(ProgressionManager.GetUnlockedToppings(), currentDay, needTutorial);

    ProgressionManager.SaveGame();

    yield return null;
    owner.ChangeState<CookingServingState>();
  }
}
