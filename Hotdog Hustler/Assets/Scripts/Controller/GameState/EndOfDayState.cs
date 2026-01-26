using UnityEngine;
using System;

public class EndOfDayState : BaseGameState
{
  public override void Enter()
  {
    base.Enter();
    Debug.Log("end of day reached!!");
    AudioManager.EndDay();

    int day = ProgressionManager.GetDay();
    DayManager.ShowEndOfDayPanel(day);

    double totalMoneyPaid = DayManager.GetTotalMoneyPaid();
    ProgressionManager.AddMoney(totalMoneyPaid);

    if (TutorialManager.IsActive())
    {
      TutorialManager.OnEndOfDayScreenReached();
    }
  }

  protected override void OnRegularInteraction()
  {
    DayManager.Deactivate();
    owner.ChangeState<ShoppingState>();
  }
}
