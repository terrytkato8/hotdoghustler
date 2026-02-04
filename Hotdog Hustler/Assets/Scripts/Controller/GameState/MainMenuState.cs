using System.Collections.Generic;
using UnityEngine;

public class MainMenuState : BaseGameState
{
  protected void ChangeState()
  {
    MainAudio.StopMainMenuMusic();
    MainMenuPanelController.Hide();
    owner.ChangeState<InitDayState>();
  }

  protected override void OnMove(object sender, OnMovementEventArgs e)
  {
    MainMenuPanelController.Navigate(e.inputVector);
  }
}
