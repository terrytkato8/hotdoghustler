using System.Collections;
using UnityEngine;

public class InitState : BaseGameState
{
  private float dayTime = 30f; //For testing purposes a day is 30 seconds

  public override void Enter()
  {
    base.Enter();
    StartCoroutine(Init());
  }

  private IEnumerator Init()
  {
    CustomerManager.Activate();
    DayClockPanelController.Show(dayTime);

    yield return null;
    owner.ChangeState<CookingServingState>();
  }
}
