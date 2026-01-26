using System.Collections.Generic;
using UnityEngine;

public class MainMenuState : BaseGameState
{
  public override void Enter()
  {
    base.Enter();

    // Show UI
    MainMenuPanelController.Show();
    AudioManager.PlayMainMenuMusic();
  }

  public override void Exit()
  {
    base.Exit();

    // Hide UI 
    MainMenuPanelController.Hide();
  }

  protected override void OnRegularInteraction()
  {
    if (!MainMenuPanelController.PlayerChoseContinue())
    {
      ProgressionManager.StartNewGame();
    }
    else
    {
      ProgressionManager.LoadGame();
    }
    AudioManager.StopMainMenuMusic();
    owner.ChangeState<InitDayState>();
  }

  protected override void OnMove(object sender, OnMovementEventArgs e)
  {
    MainMenuPanelController.Navigate(e.inputVector);
  }
}
