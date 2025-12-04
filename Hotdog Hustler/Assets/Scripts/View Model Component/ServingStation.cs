using System;
using UnityEngine;

public class ServingStation : MonoBehaviour, IInteractable
{
  public static event EventHandler<OnServeEventArgs> OnObjectServed;
  public void Interact(Player player)
  {
    if (player.HasKitchenObject())
    {
      OnObjectServed?.Invoke(this, new OnServeEventArgs
      {
        servedObject = player.GetKitchenObject()
      });
    }
  }
}
