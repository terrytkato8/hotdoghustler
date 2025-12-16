using System.Collections.Generic;
using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
  [SerializeField] private ToppingListSO masterToppingListSO; // All possible items
  [SerializeField] private ToppingListSO startingToppingListSO; // Items owned at Day 1.

  private List<ToppingSO> unlockedToppingList;
  private double currentBalance; //this and the unlockedToppingList will be loaded from a json file when we have a save system.
  private int day;

  public void Init()
  {
    day = 0;
    currentBalance = 0;
    unlockedToppingList = new(startingToppingListSO.toppingList);
  }

  public List<ToppingSO> GetUnlockedToppings()
  {
    return unlockedToppingList;
  }

  public List<ToppingSO> GetLockedToppings()
  {
    List<ToppingSO> locked = new();

    foreach (var item in masterToppingListSO.toppingList)
    {
      if (!unlockedToppingList.Contains(item))
      {
        locked.Add(item);
      }
    }
    return locked;
  }

  public void UnlockTopping(ToppingSO topping)
  {
    if (!unlockedToppingList.Contains(topping))
      unlockedToppingList.Add(topping);
    else
      Debug.LogError("topping is already unlocked");
  }

  public void AddMoney(double amount)
  {
    currentBalance += amount;
  }

  public double GetCurrentBalance() { return currentBalance; }
  public void SetCurrentBalance(double currentBalance) { this.currentBalance = currentBalance; }

  public void IncreaseDay()
  {
    day++;
  }

  public int GetDay() { return day; }
}
