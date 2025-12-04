using UnityEngine;
using UnityEngine.UI;

public class ToppingUIItem : MonoBehaviour
{
  [SerializeField] private Image iconImage;
  [SerializeField] private GameObject selectionOutline;
  [SerializeField] private Color selectedColor = Color.white;
  [SerializeField] private Color unselectedColor = new(0.5f, 0.5f, 0.5f, 0.5f); // Dimmed

  public void SetToppingData(ToppingSO toppingSO)
  {
    SpriteRenderer prefabImageComponent = toppingSO.prefab.GetComponent<SpriteRenderer>();
    iconImage.sprite = prefabImageComponent.sprite;
  }

  public void SetAsExitButton(Sprite exitSprite)
  {
    iconImage.sprite = exitSprite;
  }

  public void SetSelected(bool isSelected)
  {
    if (selectionOutline != null)
    {
      selectionOutline.SetActive(isSelected);
    }

    iconImage.color = isSelected ? selectedColor : unselectedColor;
    transform.localScale = isSelected ? Vector3.one * 1.2f : Vector3.one;
  }
}