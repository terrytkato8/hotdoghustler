using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Customer : MonoBehaviour
{
  [SerializeField] private float patienceTime = 15f;
  private Order wantedOrder;

  public void Setup(PreparedDishSO wantedDish)
  {
    wantedOrder = new(wantedDish);
  }

  public void Setup(Order order)
  {
    wantedOrder = order;
  }

  public Order GetOrder()
  {
    return wantedOrder;
  }

  public bool ValidateOrder(Order playerPlate)
  {
    bool isDishCorrect;
    //For now a simple check for the right dish. Later there will be also a comparison for the right toppings.
    if (wantedOrder.wantedDish == playerPlate.wantedDish)
    {
      isDishCorrect = true;
    }
    else
      isDishCorrect =  false;

    PlayReaction(isDishCorrect);
    return isDishCorrect;
  }

  public void PlayReaction(bool isReactionPositive) //this will later be based on percantage and the CustomerReaction Enum, instead of a bool. Also it will be a coroutine.
  {
    if (isReactionPositive)
      Debug.Log("Customer Happy!");
    else
      Debug.Log("Customer not happy and disappointed...");
  }

  public void SetPosition (Vector3 position)
  {
    transform.position = position;
  }

  public float GetPatienceTime()
  {
    return patienceTime;
  }

}
