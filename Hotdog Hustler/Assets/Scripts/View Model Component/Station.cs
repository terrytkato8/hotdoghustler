using UnityEngine;

public class Station : MonoBehaviour, IInteractable
{
  public void Interact(Player player)
  {
    Debug.Log("Interacted with Station!");
  }
}
