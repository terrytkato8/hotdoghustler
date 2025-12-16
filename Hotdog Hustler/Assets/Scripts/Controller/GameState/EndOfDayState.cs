using UnityEngine;
using System;

public class EndOfDayState : BaseGameState
{
  public override void Enter()
  {
    base.Enter();
    Debug.Log("end of day reached!!");

    int day = ProgressionManager.GetDay();
    DayManager.ShowEndOfDayPanel(day);

    double totalMoneyPaid = DayManager.GetTotalMoneyPaid();
    ProgressionManager.AddMoney(totalMoneyPaid);
  }

  protected override void OnInteraction(object sender, EventArgs e)
  {
    DayManager.Deactivate();
    owner.ChangeState<ShoppingState>();
  }
}
