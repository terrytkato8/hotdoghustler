using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItem : MonoBehaviour
{
  [SerializeField] protected Image iconImage;
  [SerializeField] protected TextMeshProUGUI text;
  [SerializeField] private GameObject selectionOutline;

  public void SetSelected(bool isSelected)
  {
    if (selectionOutline != null)
    {
      selectionOutline.SetActive(isSelected);
    }

    transform.localScale = isSelected ? Vector3.one * 1.2f : Vector3.one;
  }
}
