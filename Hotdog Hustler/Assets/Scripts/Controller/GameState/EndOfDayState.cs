using UnityEngine;

public class EndOfDayState : BaseGameState
{
  public override void Enter()
  {
    base.Enter();
    Debug.Log("end of day reached!!");
  }
}
