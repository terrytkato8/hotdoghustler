using UnityEngine;

public abstract class BasePanelController : MonoBehaviour
{
  [Header("Base Configuration")]
  [SerializeField] protected GameObject visualPanel;

  public virtual void Show()
  {
    visualPanel.SetActive(true);
  }

  public virtual void Hide()
  {
    visualPanel.SetActive(false);
  }

  public bool IsVisible()
  {
    return visualPanel.activeSelf;
  }
}