using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPanelController : MonoBehaviour
{
  [Header("Configuration")]
  [SerializeField] private GameObject visualPanel;
  [SerializeField] private UIItem newGameButton;
  [SerializeField] private UIItem continueButton;

  private int selection;

  private void Start()
  {
    Hide(); // Start hidden
  }

  public void Show()
  {
    visualPanel.SetActive(true);
    selection = 0;
    UpdateVisuals();
  }

  public void Hide()
  {
    visualPanel.SetActive(false);
  }

  public void Navigate(Vector2 direction)
  {
    if (direction.y > 0)
    {
      selection++;
    }
    else if (direction.y < 0)
    {
      selection--;
    }

    if (selection >= 1)
    {
      selection = 0;
    }
    else if (selection < 0)
    {
      selection = 1;
    }

    UpdateVisuals();
  }

  private void UpdateVisuals()
  {
    newGameButton.SetSelected(selection == 0);
    continueButton.SetSelected(selection == 1);
  }

  public bool PlayerChoseContinue()
  {
    if (selection == 0)
      return false;
    else return true;
  }
}
