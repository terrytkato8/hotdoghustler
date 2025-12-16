using NUnit.Framework;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class ShopMenuPanelController : ToppingMenuPanelController
{
  [SerializeField] private TextMeshProUGUI moneyText;

  public void Show(double money, List<ToppingSO> lockedToppings)
  {
    Show(lockedToppings);
    moneyText.gameObject.SetActive(true);
    UpdateMoneyVisual(money);
  }

  public override void Hide() 
  {
    base.Hide();
    moneyText.gameObject.SetActive(false);
  }


  public void BuyTopping(ToppingSO topping, double newBalance)
  {
    toppingList.Remove(topping);
    UpdateMoneyVisual(newBalance);
    InitializeButtons();
  }

  public void UpdateMoneyVisual(double money)
  {
    moneyText.text = "Cash: $" + money;
  }
}
