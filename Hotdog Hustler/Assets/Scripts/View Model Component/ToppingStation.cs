using UnityEngine;
using System;

public class ToppingStation : MonoBehaviour, IInteractable
{
  public static readonly StaticGameEvent<KitchenObjectEventArgs> OnToppingStationActivated = new();

  [SerializeField] private PreparedDishSO preparedDishSO;

  public void Interact(Player player)
  {
    if (player.HasKitchenObject())
    {
      KitchenObject playerKitchenObject = player.GetKitchenObject();
      if (playerKitchenObject.GetPreparedDishSO() != null) 
      {

        Debug.Log("Player put Hotdog on Topping Station."); //message for hotdog right now. can be any prepared dish in the future.

        OnToppingStationActivated.Invoke(this,
          new KitchenObjectEventArgs() { kitchenObject = playerKitchenObject }
        );
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