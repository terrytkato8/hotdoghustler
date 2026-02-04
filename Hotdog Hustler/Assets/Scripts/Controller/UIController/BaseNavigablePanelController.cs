using System.Collections.Generic;
using UnityEngine;

public abstract class BaseNavigablePanelController : BasePanelController
{
  protected int selection;
  protected List<UIItem> uiItems = new();

  protected abstract int StepX { get; } 
  protected abstract int StepY { get; }

  public override void Show()
  {
    base.Show();
    selection = 0;
    UpdateVisuals();
  }

  public virtual void Navigate(Vector2 direction)
  {
    if (uiItems.Count == 0) return;

    int prevSelection = selection;

    if (direction.x != 0 && StepX > 0)
    {
      if (direction.x > 0) selection += StepX; // Right
      else selection -= StepX;                 // Left
    }

    if (direction.y != 0 && StepY > 0)
    {
      if (direction.y < 0) selection += StepY; // Down
      else selection -= StepY;                 // Up
    }

    HandleWrapping(prevSelection, direction);
    UpdateVisuals();
  }

  protected virtual void HandleWrapping(int prev, Vector2 dir)
  {
    if (selection >= uiItems.Count) selection = 0;
    if (selection < 0) selection = uiItems.Count - 1;
  }

  protected void UpdateVisuals()
  {
    for (int i = 0; i < uiItems.Count; i++)
    {
      uiItems[i].SetSelected(i == selection);
    }
  }
}
