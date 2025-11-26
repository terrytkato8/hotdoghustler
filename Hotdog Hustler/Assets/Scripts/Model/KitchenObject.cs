using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
  [SerializeField] private SpriteRenderer spriteRenderer;
  private KitchenObjectParent kitchenObjectParent;
  private KitchenObjectSO kitchenObjectSO;
  private List<ToppingSO> toppingsAddedList;

  private void Awake()
  {
    toppingsAddedList = new List<ToppingSO>();
  }

  public void Setup(KitchenObjectSO so)
  {
    kitchenObjectSO = so;
    spriteRenderer.sprite = so.sprite;
  }

  public KitchenObjectSO GetKitchenObjectSO()
  {
    return kitchenObjectSO;
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
    if (!toppingsAddedList.Contains(topping))
    {
      toppingsAddedList.Add(topping);

      GameObject toppingVisual = Instantiate(topping.prefab, transform);
      toppingVisual.transform.localPosition = Vector3.zero; // Reset position relative to hotdog
    }
  }

  public List<ToppingSO> GetToppings()
  {
    return toppingsAddedList;
  }

  public void DestroySelf()
  {
    kitchenObjectParent.ClearKitchenObject();
    Destroy(gameObject);
  }
}
