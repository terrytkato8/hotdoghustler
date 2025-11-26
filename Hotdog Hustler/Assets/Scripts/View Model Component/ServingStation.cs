using UnityEngine;

public class ServingStation : MonoBehaviour, IInteractable
{
  public void Interact(Player player)
  {
    if (player.HasKitchenObject())
    {
      player.GetKitchenObject().DestroySelf();
      Debug.Log("Player served the food");
    }
  }
}
