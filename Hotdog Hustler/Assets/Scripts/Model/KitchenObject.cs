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
    toppingSOList.Add(topping);

    float randomXCoordinate = Random.Range(-1f, 1f); //so that not all topping sprites are perfectly on top of each other
    float randomYCoordinate = Random.Range(-1f, 1f);

    GameObject toppingVisual = Instantiate(topping.prefab, transform);
    toppingVisual.transform.localPosition = new Vector3 (randomXCoordinate, randomYCoordinate, 0); // Reset position relative to hotdog
    toppingVisual.transform.localScale = new Vector3 (3f,3f,3f); // Set the scaling to look normal on the hotdog. Will be set by the topping prefabs later itself.
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

  public FoodState GetFoodState()
  {
    return kitchenObjectSO.foodState;
  }

  public void DestroySelf()
  {
    kitchenObjectParent.ClearKitchenObject();
    Destroy(gameObject);
  }
}
