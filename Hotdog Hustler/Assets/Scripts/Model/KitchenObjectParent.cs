using UnityEngine;

public abstract class KitchenObjectParent : MonoBehaviour
{
  [SerializeField] protected Transform holdPoint;
  protected KitchenObject kitchenObject;
  public Transform GetKitchenObjectFollowTransform() { return holdPoint; }
  public void SetKitchenObject(KitchenObject kitchenObject) { this.kitchenObject = kitchenObject; }
  public KitchenObject GetKitchenObject() { return kitchenObject; }
  public void ClearKitchenObject() { kitchenObject = null; }
  public bool HasKitchenObject() { return kitchenObject != null; }

  public void SpawnItem(KitchenObjectSO itemToSpawn)
  {
    GameObject g = Instantiate(itemToSpawn.prefab, holdPoint);
    KitchenObject newObject = g.AddComponent<KitchenObject>();
    newObject.Setup(itemToSpawn);
    newObject.SetKitchenObjectParent(this);
  }
}