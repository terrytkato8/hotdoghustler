using UnityEngine;

public class MainMenuStartState : MainMenuState
{
  public override void Enter()
  {
    base.Enter();

    MainMenuPanelController.ShowStart();
  }

  protected override void OnRegularInteraction()
  {
    if (!MainMenuPanelController.PlayerChoseFirstOption())
    {
      ProgressionManager.StartNewGame();
      owner.ChangeState<MainMenuDifficultyState>();
    }
    else
    {
      ProgressionManager.LoadGame();
      ChangeState();
    }
  }
}
