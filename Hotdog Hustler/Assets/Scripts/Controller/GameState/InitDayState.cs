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
    CurrentDay = ProgressionManager.GetDay();

    Debug.Log("starting Day " + CurrentDay);
    MainAudio.StartDay();

    OrderPanelController OrderPanelController = CustomerManager.GetOrderPanelController();
    int tutorialDay = 1; //activate tutorial on day 1
    bool needTutorial = CurrentDay == tutorialDay;
    if (needTutorial) 
    {
      TutorialManager.Activate(Grillstation, ToastStation, ServingStation, ToppingStation, OrderPanelController, ToppingMenuPanelController);
    }
    else
    {
      if (TutorialManager.IsActive()) TutorialManager.Deactivate();
      ProgressionManager.IncreaseDay();
      DayManager.Activate(CurrentDay); //when the tutorial is active, the daymanager will be activated later.
    }

    DailyDifficulty currentDifficulty = ProgressionManager.GetDifficultyForDay(CurrentDay);
    CustomerManager.Activate(ProgressionManager.GetUnlockedToppings(), needTutorial, currentDifficulty);

    ProgressionManager.SaveGame();

    yield return null;
    owner.ChangeState<CookingServingState>();
  }
}
