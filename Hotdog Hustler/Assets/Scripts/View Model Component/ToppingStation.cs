using UnityEngine;
using System;

public class ToppingStation : MonoBehaviour, IInteractable
{
  public static event EventHandler OnToppingStationActivated;

  [SerializeField] private KitchenObjectSO hotdogKitchenObjectSO;

  public void Interact(Player player)
  {
    if (player.HasKitchenObject())
    {

      if (player.GetKitchenObject().GetKitchenObjectSO() == hotdogKitchenObjectSO) //hardcoded check for now. Later will check for a "toppable dish".
      {

        Debug.Log("Player put Hotdog on Topping Station.");

        OnToppingStationActivated?.Invoke(this, EventArgs.Empty);
      }
      else
      {
        Debug.Log("You can only add toppings to a Hotdog!");
      }
    }
    else
    {
      Debug.Log("You need to hold a Hotdog first.");
    }
  }
}