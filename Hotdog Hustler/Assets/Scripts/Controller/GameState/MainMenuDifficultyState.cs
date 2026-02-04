using UnityEngine;

public class MainMenuDifficultyState : MainMenuState
{
  public override void Enter()
  {
    base.Enter();

    MainMenuPanelController.ShowDifficulty();
  }

  protected override void OnRegularInteraction()
  {
    if (!MainMenuPanelController.PlayerChoseFirstOption())
    {
      ProgressionManager.SetDifficultyMode(DifficultyMode.Easy);
    }
    else
    {
      ProgressionManager.SetDifficultyMode(DifficultyMode.Hard);
    }
    ChangeState();
  }
}
