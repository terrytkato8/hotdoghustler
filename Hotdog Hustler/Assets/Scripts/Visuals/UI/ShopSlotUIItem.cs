using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToppingShopUIItem : ToppingUIItem
{
  [SerializeField] private TextMeshProUGUI nameText;
  [SerializeField] private TextMeshProUGUI priceText;

  public override void SetToppingData(ToppingSO topping)
  {
    base.SetToppingData(topping);
    nameText.text = topping.toppingName;
    priceText.text = "$" + topping.unlockPrice;
  }
}
