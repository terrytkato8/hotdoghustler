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
}
