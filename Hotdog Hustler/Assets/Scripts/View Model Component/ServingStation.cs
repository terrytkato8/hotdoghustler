using System;
using UnityEngine;

public class ServingStation : MonoBehaviour, IInteractable
{
  public static readonly StaticGameEvent<KitchenObjectEventArgs> OnObjectServed = new();

  public void Interact(Player player)
  {
    if (player.HasKitchenObject())
    {
      OnObjectServed?.Invoke(this, new KitchenObjectEventArgs
      {
        kitchenObject = player.GetKitchenObject()
      });
    }
  }
}
