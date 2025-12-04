using System.Collections.Generic;
using UnityEngine;

public class ToppingMenuPanelController : MonoBehaviour
{
  [Header("Configuration")]
  [SerializeField] private GameObject visualPanel;
  [SerializeField] private ToppingListSO toppingListSO;
  [SerializeField] private Sprite exitIconSprite;

  [Header("UI References")]
  [SerializeField] private Transform iconsContainer;
  [SerializeField] private GameObject itemTemplate;

  private List<ToppingUIItem> uiItems = new();
  private List<ToppingSO> toppingList;
  private int selection;

  private void Awake()
  {
    toppingList = toppingListSO.toppingList;
  }

  private void Start()
  {
    InitializeButtons();
    Hide(); // Start hidden
  }

  public void Show()
  {
    visualPanel.SetActive(true);
    selection = 0;
  }

  public void Hide()
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
      CreateButton(topping, false);
    }

    //Create Exit Button (Last item)
    CreateButton(null, true);

    UpdateVisuals();
  }

  private void CreateButton(ToppingSO topping, bool isExit)
  {
    GameObject btnTransform = Instantiate(itemTemplate, iconsContainer, false);
    gameObject.SetActive(true);

    ToppingUIItem uiItem = btnTransform.GetComponent<ToppingUIItem>();

    if (isExit)
    {
      uiItem.SetAsExitButton(exitIconSprite);
    }
    else
    {
      uiItem.SetToppingData(topping);
    }

    uiItems.Add(uiItem);
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
}
