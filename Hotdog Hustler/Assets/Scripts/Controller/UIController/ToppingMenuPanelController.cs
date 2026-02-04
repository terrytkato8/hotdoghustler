using System.Collections.Generic;
using UnityEngine;

public class ToppingMenuPanelController : MonoBehaviour
{
  [Header("Configuration")]
  [SerializeField] private GameObject visualPanel;
  [SerializeField] private Sprite exitIconSprite;

  [Header("UI References")]
  [SerializeField] private List<Transform> toppingSlots;
  [SerializeField] private Transform exitButtonSlot;
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
    ClearPreviousButtons();

    int slotIndex = 0;
    foreach (ToppingSO topping in toppingList)
    {
      if (slotIndex >= toppingSlots.Count)
        break;

      CreateButton(toppingSlots[slotIndex], topping);
      slotIndex++;
    }

    //Create Exit Button (Last item)
    CreateButton(exitIconSprite);

    UpdateVisuals();
  }

  private void CreateButton(Transform slot, ToppingSO topping)
  {
    ToppingUIItem uiItem = InstantiateToppUIItem(slot);

    uiItem.SetToppingData(topping);

    uiItems.Add(uiItem);
  }

  private void CreateButton(Sprite sprite)
  {
    ToppingUIItem uiItem = InstantiateToppUIItem(exitButtonSlot);

    uiItem.SetOtherSprite(sprite);

    uiItems.Add(uiItem);
  }

  private ToppingUIItem InstantiateToppUIItem(Transform slot)
  {
    GameObject btnTransform = Instantiate(itemTemplate, slot, false);
    gameObject.SetActive(true);

    return btnTransform.GetComponent<ToppingUIItem>();
  }

  private void ClearPreviousButtons()
  {
    foreach (ToppingUIItem uiItem in uiItems)
    {
      if (uiItem != null)
        Destroy(uiItem.gameObject);
    }
    uiItems.Clear();
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
      uiItems[i].SetSelected(i == selection, i == uiItems.Count - 1);
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
