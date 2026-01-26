using UnityEngine;
using UnityEngine.UI;

public class ToppingUIItem : MonoBehaviour
{
  [SerializeField] protected Image iconImage;
  [SerializeField] private GameObject selectionOutline;
  //[SerializeField] private Color selectedColor = Color.white;
  //[SerializeField] private Color unselectedColor = new(0.5f, 0.5f, 0.5f, 0.5f); // Dimmed

  public virtual void SetToppingData(ToppingSO toppingSO)
  {
    SpriteRenderer prefabImageComponent = toppingSO.prefab.GetComponent<SpriteRenderer>();
    iconImage.sprite = prefabImageComponent.sprite;
  }

  public void SetOtherSprite(Sprite otherSprite)
  {
    iconImage.sprite = otherSprite;
  }

  public void SetSelected(bool isSelected, bool isExitButton)
  {
    if (selectionOutline != null)
    {
      selectionOutline.SetActive(isSelected);
    }

    if (isExitButton)
      transform.localScale = Vector3.one;
    else
      transform.localScale = isSelected ? Vector3.one * 0.6f : Vector3.one * 0.5f;
  }
}