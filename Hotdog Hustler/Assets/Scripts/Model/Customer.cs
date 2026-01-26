using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Player;

public class Customer : MonoBehaviour
{
  [SerializeField] private float patienceTime = 15f;
  private Order wantedOrder;

  private bool isFrontCustomer;

  public event EventHandler OnSetup;
  public event EventHandler<OnReactionEventArgs> OnReaction;

  public void Setup(Order order)
  {
    wantedOrder = order;

    OnSetup?.Invoke(this, EventArgs.Empty);
  }
  
  public void StartPatienceTimer()
  {
    isFrontCustomer = true;
  }

  private void Update()
  {
    if (isFrontCustomer)
    { 
      patienceTime -= Time.deltaTime;
    }
  }

  public double ValidateOrder(Order playerPlate)
  {
    if (playerPlate == null) return 0;

    double accuracy = 1.0;

    if (wantedOrder.wantedDish != playerPlate.wantedDish)
      accuracy -= 0.5;

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
          accuracy -= 0.5;
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

  public IEnumerator ReactToFood(double accuracy) 
  {
    bool isHappy = accuracy == 1.0;
    float reactionDuration = 0f;

    OnReaction?.Invoke(this, new OnReactionEventArgs
    {
      isHappy = isHappy,
      SetDurationCallback = (duration) => reactionDuration = duration
    });

    if (reactionDuration <= 0) reactionDuration = 1.0f;
    yield return new WaitForSeconds(reactionDuration);
  }

  public void SetPosition(Vector3 position) => transform.position = position;
  public Order GetOrder() => wantedOrder;
  public float GetPatienceTime() => patienceTime;
  public bool IsPatienceExhausted() => patienceTime <= 0;
}
