using NUnit.Framework;
using System.Collections.Generic;

public class Order
{
  public PreparedDishSO wantedDish;
  public List<ToppingSO> wantedToppings;

  public Order(PreparedDishSO wantedDish)
  {
    this.wantedDish = wantedDish;
  }

  public Order(PreparedDishSO wantedDish, List<ToppingSO> wantedToppings) : this(wantedDish)
  {
    this.wantedToppings = wantedToppings;
  }

  public double GetTotalPrice()
  {
    double total = 0;

    if (wantedDish != null)
    {
      total += wantedDish.price;
    }

    if (wantedToppings != null)
    {
      foreach (var topping in wantedToppings)
      {
        if (topping != null)
          total += topping.price;
      }
    }

    return total;
  }
}
