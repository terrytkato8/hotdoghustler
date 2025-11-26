using UnityEngine;

public class TrashCan : MonoBehaviour, IInteractable
{
  public void Interact(Player player)
  {
    if (player.HasKitchenObject())
    {
      player.GetKitchenObject().DestroySelf();
      Debug.Log("Player threw the item in the trash.");
    }
  }
}
