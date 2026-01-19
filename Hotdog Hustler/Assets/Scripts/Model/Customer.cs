using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public class Customer : MonoBehaviour
{
  [SerializeField] private float patienceTime = 15f;
  [SerializeField] private SpriteRenderer spriteRenderer;
  private Order wantedOrder;

  public event EventHandler OnSetup;

  public void Setup(Order order)
  {
    wantedOrder = order;

    OnSetup?.Invoke(this, EventArgs.Empty);
  }

  public Order GetOrder()
  {
    return wantedOrder;
  }

  public double ValidateOrder(Order playerPlate)
  {
    if (wantedOrder.wantedDish != playerPlate.wantedDish)
      return 0;

    double accuracy = 1.0;

    int maxCount = Math.Max(wantedOrder.wantedToppings?.Count ?? 0, playerPlate.wantedToppings?.Count ?? 0);
    bool firstError = true;

    for (int i = 0; i < maxCount; i++)
    {
      ToppingSO myTopping = (i < (wantedOrder.wantedToppings?.Count ?? 0)) ? wantedOrder.wantedToppings[i] : null;
      ToppingSO otherTopping = (i < (playerPlate.wantedToppings?.Count ?? 0)) ? playerPlate.wantedToppings[i] : null;

      bool isSame = myTopping == otherTopping;

      if (!isSame)
      {
        if (firstError)
        {
          accuracy = 0.5;
          firstError = false;
        }
        else
        {
          accuracy -= 0.1;
        }
      }
    }

    return Math.Max(accuracy, 0);
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
