using System.Collections.Generic;
using UnityEngine;

public class ToppingMenuPanelController : MonoBehaviour
{
  [Header("Configuration")]
  [SerializeField] private GameObject visualPanel;
  [SerializeField] private Sprite exitIconSprite;

  [Header("UI References")]
  [SerializeField] private Transform iconsContainer;
  [SerializeField] private GameObject itemTemplate;
  private List<ToppingUIItem> uiItems = new();

  protected List<ToppingSO> toppingList;
  private int selection;

  private void Start()
  {
    Hide(); // Start hidden
  }

  public void Show(List<ToppingSO> toppingList)
  {
    visualPanel.SetActive(true);
    this.toppingList = toppingList;
    selection = 0;
    InitializeButtons();
  }

  public virtual void Hide()
  {
    visualPanel.SetActive(false);
  }

  private void InitializeButtons()
  {
    foreach (Transform child in iconsContainer)
    {
      if (child.gameObject == itemTemplate) continue;
      Destroy(child.gameObject);
    }
    uiItems.Clear();

    foreach (ToppingSO topping in toppingList)
    {
      CreateButton(topping);
    }

    //Create Exit Button (Last item)
    CreateButton(exitIconSprite);

    UpdateVisuals();
  }

  private void CreateButton(ToppingSO topping)
  {
    ToppingUIItem uiItem = InstantiateToppUIItem();

    uiItem.SetToppingData(topping);

    uiItems.Add(uiItem);
  }

  private void CreateButton(Sprite sprite)
  {
    ToppingUIItem uiItem = InstantiateToppUIItem();

    uiItem.SetOtherSprite(sprite);

    uiItems.Add(uiItem);
  }

  private ToppingUIItem InstantiateToppUIItem()
  {
    GameObject btnTransform = Instantiate(itemTemplate, iconsContainer, false);
    gameObject.SetActive(true);

    return btnTransform.GetComponent<ToppingUIItem>();
  }

  public void Navigate(Vector2 direction)
  {
    if (direction.x > 0)
    {
      selection++;
    }
    else if (direction.x < 0)
    {
      selection--;
    }

    if (selection >= uiItems.Count)
    {
      selection = 0;
    }
    else if (selection < 0)
    {
      selection = uiItems.Count - 1;
    }

    UpdateVisuals();
  }

  private void UpdateVisuals()
  {
    for (int i = 0; i < uiItems.Count; i++)
    {
      uiItems[i].SetSelected(i == selection);
    }
  }

  public ToppingSO GetSelectedTopping()
  {
    // If selection is the last item (Exit button)
    if (selection == toppingList.Count)
    {
      return null;
    }
    return toppingList[selection];
  }

  public int GetSelection()
  {
    return selection;
  }

  public Transform GetVisualPanelTransform()
  {
    return visualPanel.transform;
  }
}
