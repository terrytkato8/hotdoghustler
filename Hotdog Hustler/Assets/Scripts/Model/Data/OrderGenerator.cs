using System.Collections.Generic;
using UnityEngine;

public class OrderGenerator
{
  private List<PreparedDishSO> availableDishes;
  private List<ToppingSO> availableToppings;

  public OrderGenerator(List<PreparedDishSO> dishes, List<ToppingSO> toppings)
  {
    this.availableDishes = dishes;
    this.availableToppings = toppings;
  }

  public Order GenerateRandomOrder()
  {
    if (availableDishes.Count == 0) return null;
    PreparedDishSO dish = availableDishes[UnityEngine.Random.Range(0, availableDishes.Count)];

    int toppingCount = GetWeightedToppingCount();

    List<ToppingSO> selectedToppings = new List<ToppingSO>();

    for (int i = 0; i < toppingCount; i++)
    {
      if (availableToppings.Count > 0)
      {
        ToppingSO randomTopping = availableToppings[UnityEngine.Random.Range(0, availableToppings.Count)];
        selectedToppings.Add(randomTopping);
      }
    }

    return new Order(dish, selectedToppings);
  }

  // Helper for the Tutorial (e.g., Force "Hotdog with Ketchup and Mustard")
  public Order GenerateSpecificOrder(int dishIndex, int[] toppingIndices)
  {
    if (dishIndex >= availableDishes.Count) return null;

    PreparedDishSO dish = availableDishes[dishIndex];
    List<ToppingSO> specificToppings = new List<ToppingSO>();

    foreach (int i in toppingIndices)
    {
      if (i < availableToppings.Count)
      {
        specificToppings.Add(availableToppings[i]);
      }
    }

    return new Order(dish, specificToppings);
  }

  private int GetWeightedToppingCount()
  {
    float v = Random.value;

    if (v < 0.10f) return 0;
    if (v < 0.55f) return 1;
    return 2;
  }
}