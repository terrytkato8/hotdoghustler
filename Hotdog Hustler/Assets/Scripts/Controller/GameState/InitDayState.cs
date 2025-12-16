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
    CustomerManager.Activate(ProgressionManager.GetUnlockedToppings());
    DayManager.Activate();
    ProgressionManager.IncreaseDay();

    yield return null;
    owner.ChangeState<CookingServingState>();
  }
}
