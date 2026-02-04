using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPanelController : BaseNavigablePanelController
{
  protected override int StepX => 0;
  protected override int StepY => 1;

  [Header("Configuration")]
  [SerializeField] private UIItem newGameButton;
  [SerializeField] private UIItem continueButton;
  [SerializeField] private UIItem easyButton;
  [SerializeField] private UIItem difficultButton;

  private void Start()
  {
    Hide(); // Start hidden
  }

  public void ShowStart()
  {
    uiItems.Clear();

    UpdateMainMenuButtons(newGameButton, continueButton, easyButton, difficultButton);

    Show();
  }

  public void ShowDifficulty()
  {
    uiItems.Clear();

    UpdateMainMenuButtons(easyButton, difficultButton, newGameButton, continueButton);

    Show();
  }

  private void UpdateMainMenuButtons(UIItem showButton1, UIItem showButton2, UIItem hideButton1, UIItem hideButton2)
  {
    uiItems.Clear();
    showButton1.gameObject.SetActive(true);
    showButton2.gameObject.SetActive(true);
    hideButton1.gameObject.SetActive(false);
    hideButton2.gameObject.SetActive(false);
    uiItems.Add(showButton1);
    uiItems.Add(showButton2);
  }

  public bool PlayerChoseFirstOption()
  {
    if (selection == 0)
      return false;
    else return true;
  }
}
