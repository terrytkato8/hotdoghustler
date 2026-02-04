using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class ShopMenuPanelController : MonoBehaviour
{
  [Header("Configuration")]
  [SerializeField] private GameObject visualPanel;
  [SerializeField] private Sprite exitIconSprite;
  [SerializeField] private Sprite folderSprite;

  [Header("UI References")]
  [SerializeField] private Transform iconsContainer;
  [SerializeField] private GameObject itemTemplate;
  private List<ToppingUIItem> uiItems = new();

  [SerializeField] private TextMeshProUGUI moneyText;

  private List<ToppingSO> basicToppingList;
  private List<ToppingSO> betterToppingList;
  private List<ToppingSO> premiumToppingList;
  private List<List<ToppingSO>> toppingCategoryList;

  private List<ToppingSO> currentToppingList;
  private int selection;

  public void Show(double money, List<ToppingSO> lockedToppings)
  {
    basicToppingList = new();
    betterToppingList = new();
    premiumToppingList = new();
    toppingCategoryList = new() { basicToppingList, betterToppingList, premiumToppingList };
    CategorizeToppings(lockedToppings);

    visualPanel.SetActive(true);
    UpdateMoneyVisual(money);
    ShowCategories();
  }

  public void Hide() 
  {
    visualPanel.SetActive(false);

  }

  public void ShowCategories()
  {
    selection = 0;
    InitializeCategoryButtons();
  }

  public bool ShowToppings()
  {
    if (selection == uiItems.Count-1)
    {
      return false;
    }

    currentToppingList = toppingCategoryList[selection];
    selection = 0;
    InitializeToppingButtons(currentToppingList);
    return true;
  }

  public ToppingSO GetSelectedTopping()
  {
    if (selection == currentToppingList.Count)
    {
      return null;
    }
    return currentToppingList[selection];
  }

  public void BuyTopping(ToppingSO topping, double newBalance)
  {
    List<ToppingSO> toppingList = new();
    switch (topping.quality)
    {
      case ToppingQuality.basic:
        basicToppingList.Remove(topping);
        toppingList = basicToppingList;
        break;
      case ToppingQuality.better:
        betterToppingList.Remove(topping);
        toppingList = betterToppingList;
        break;
      case ToppingQuality.premium:
        premiumToppingList.Remove(topping);
        toppingList = premiumToppingList;
        break;
    }
    UpdateMoneyVisual(newBalance);
    InitializeToppingButtons(toppingList);
  }

  public void Navigate(Vector2 direction)
  {
    int rowsPerColumn = 3;
    int totalItems = uiItems.Count;

    int currentColumn = selection / rowsPerColumn;
    int currentRow = selection % rowsPerColumn;

    if (direction.y != 0)
    {
      int colStartIndex = currentColumn * rowsPerColumn;
      int colEndIndex = Mathf.Min(colStartIndex + rowsPerColumn - 1, totalItems - 1);

      if (direction.y < 0) 
      {
        selection++;
        if (selection > colEndIndex)
        {
          selection = colStartIndex;
        }
      }
      else if (direction.y > 0)
      {
        selection--;
        if (selection < colStartIndex)
        {
          selection = colEndIndex;
        }
      }
    }

    if (direction.x != 0)
    {
      if (direction.x > 0)
      {
        selection += rowsPerColumn;

        if (selection >= totalItems)
        {
          selection = currentRow;
        }
      }
      else if (direction.x < 0)
      {
        selection -= rowsPerColumn;

        if (selection < 0)
        {
          int maxColumns = (totalItems - 1) / rowsPerColumn;

          int target = (maxColumns * rowsPerColumn) + currentRow;

          if (target >= totalItems)
          {
            target -= rowsPerColumn;
          }
          selection = target;
        }
      }
    }

    UpdateVisuals();
  }

  private void CategorizeToppings(List<ToppingSO> toppingList)
  {
    foreach (ToppingSO topping in toppingList)
    {
      switch (topping.quality)
      {
        case ToppingQuality.basic:
          basicToppingList.Add(topping);
          break;
        case ToppingQuality.better:
          betterToppingList.Add(topping);
          break;
        case ToppingQuality.premium:
          premiumToppingList.Add(topping);
          break;
      }
    }
  }

  private void UpdateMoneyVisual(double money)
  {
    moneyText.text = "Cash: $" + money;
  }

  private void InitializeToppingButtons(List<ToppingSO> toppingList)
  {
    ClearUIItems();

    foreach (ToppingSO topping in toppingList)
    {
      CreateButton(topping);
    }

    CreateButton(exitIconSprite);

    UpdateVisuals();
  }

  private void InitializeCategoryButtons()
  {
    ClearUIItems();

    CreateButton(ToppingQuality.basic);
    CreateButton(ToppingQuality.better);
    CreateButton(ToppingQuality.premium);

    CreateButton(exitIconSprite);

    UpdateVisuals();
  }

  private void ClearUIItems()
  {
    foreach (Transform child in iconsContainer)
    {
      if (child.gameObject == itemTemplate) continue;
      Destroy(child.gameObject);
    }
    uiItems.Clear();
  }

  private void CreateButton(ToppingSO topping)
  {
    ToppingShopUIItem uiItem = InstantiateToppUIItem();

    uiItem.SetToppingData(topping);

    uiItems.Add(uiItem);
  }

  private void CreateButton(Sprite sprite)
  {
    ToppingShopUIItem uiItem = InstantiateToppUIItem();

    uiItem.SetOtherSprite(sprite);

    uiItems.Add(uiItem);
  }

  private void CreateButton(ToppingQuality toppingQuality)
  {
    ToppingShopUIItem uiItem = InstantiateToppUIItem();

    uiItem.SetToppingCategoryData(toppingQuality, folderSprite);

    uiItems.Add(uiItem);
  }

  private ToppingShopUIItem InstantiateToppUIItem()
  {
    GameObject btnTransform = Instantiate(itemTemplate, iconsContainer, false);
    gameObject.SetActive(true);

    return btnTransform.GetComponent<ToppingShopUIItem>();
  }

  private void UpdateVisuals()
  {
    for (int i = 0; i < uiItems.Count; i++)
    {
      uiItems[i].SetSelected(i == selection, i == uiItems.Count - 1);
    }
  }
}
