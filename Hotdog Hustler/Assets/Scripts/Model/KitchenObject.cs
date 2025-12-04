using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
  private KitchenObjectParent kitchenObjectParent;
  private KitchenObjectSO kitchenObjectSO;
  private List<ToppingSO> toppingSOList;

  private void Awake()
  {
    toppingSOList = new List<ToppingSO>();
  }

  public void Setup(KitchenObjectSO so) 
  {
    kitchenObjectSO = so;
  }

  public void Setup(KitchenObjectSO kitchenObjectSO, List<ToppingSO> toppingSOList)
  {
    this.kitchenObjectSO = kitchenObjectSO;
    this.toppingSOList = toppingSOList;
  }

  public void SetKitchenObjectParent(KitchenObjectParent kitchenObjectParent)
  {
    if (this.kitchenObjectParent != null) 
    {
      this.kitchenObjectParent.ClearKitchenObject();
    }
    this.kitchenObjectParent = kitchenObjectParent;

    kitchenObjectParent.SetKitchenObject(this);

    transform.parent = kitchenObjectParent.GetKitchenObjectFollowTransform();
    transform.localPosition = Vector3.zero;
  }

  public void AddTopping(ToppingSO topping)
  {
    if (!toppingSOList.Contains(topping))
    {
      toppingSOList.Add(topping);

      GameObject toppingVisual = Instantiate(topping.prefab, transform);
      toppingVisual.transform.localPosition = Vector3.zero; // Reset position relative to hotdog
    }
  }

  public KitchenObjectSO GetKitchenObjectSO()
  {
    return kitchenObjectSO;
  }

  public PreparedDishSO GetPreparedDishSO()
  {
    return kitchenObjectSO as PreparedDishSO;
  }

  public List<ToppingSO> GetToppings()
  {
    return toppingSOList;
  }

  public void DestroySelf()
  {
    kitchenObjectParent.ClearKitchenObject();
    Destroy(gameObject);
  }
}
