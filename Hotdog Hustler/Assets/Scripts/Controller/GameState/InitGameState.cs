using System.Collections;
using UnityEngine;

public class InitGameState : BaseGameState
{
  public override void Enter()
  {
    base.Enter();
    StartCoroutine(Init());
  }

  public override void Exit()
  {
    base.Exit();
    MainAudio.PlayMainMenuMusic();
  }

  private IEnumerator Init()
  {
    StaticGameEvent.ResetAllStaticEvents();

    yield return null;
    owner.ChangeState<MainMenuStartState>();
  }
}
