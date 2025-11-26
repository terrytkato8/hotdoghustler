using UnityEngine;

public class ToppingMenuPanelController : MonoBehaviour
{
  [SerializeField] private GameObject visualPanel;
  [SerializeField] private int selection;

  public void Show()
  {
    visualPanel.SetActive(true);
  }

  public void Hide()
  {
    visualPanel.SetActive(false);
  }

  public void Navigate(Vector2 direction)
  {
    Debug.Log($"Navigating Topping Menu: {direction}");
  }

  public void SelectCurrentOption()
  {
    Debug.Log("Selected Topping!");
  }

  public int GetSelection()
  {
    return selection;
  }
}
