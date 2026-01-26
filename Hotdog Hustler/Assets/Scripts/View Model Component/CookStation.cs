using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CookStation : KitchenObjectParent, IInteractable 
{
  [SerializeField] private CookingRecipeSO cookingRecipe;
  [SerializeField] private RecipeSO preparedDishRecipe;

  private enum State { Idle, Cooking, Cooked, Burnt }
  private State state;
  private float timer;

  public static readonly StaticGameEvent OnStartedCooking = new();
  public static readonly StaticGameEvent OnFinishedCooking = new();
  public static readonly StaticGameEvent OnCreatedPreparedDish = new();
  public static readonly StaticGameEvent OnKitchenObjectRemoved = new();

  private void Start()
  {
    state = State.Idle;
  }

  private void Update()
  {
    if (HasKitchenObject())
    {
      switch (state)
      {
        case State.Idle:
          break;
        case State.Cooking:
          timer += Time.deltaTime;

          if (timer >= cookingRecipe.cookingTimeMax)
          {
            state = State.Cooked;
            timer = 0f; // Reset for burning phase

            // Destroy Cooked, Spawn Burnt
            if (kitchenObject != null)
            {
              Destroy(kitchenObject.gameObject); // Simple destroy for now
              kitchenObject = null; // Clear reference
            }

            // Spawn Cooked Food
            SpawnItem(cookingRecipe.cookedOutput);
            Debug.Log("Food Cooked!");
            OnFinishedCooking.Invoke(this, EventArgs.Empty);
          }
          break;
        case State.Cooked:
          timer += Time.deltaTime;

          if (timer >= cookingRecipe.burningTimeMax)
          {
            state = State.Burnt;

            // Destroy Cooked, Spawn Burnt
            if (kitchenObject != null)
            {
              Destroy(kitchenObject.gameObject); // Simple destroy for now
              kitchenObject = null; // Clear reference
            }

            SpawnItem(cookingRecipe.burntOutput);
            Debug.Log("Food Burnt!");
          }
          break;
        case State.Burnt:
          break;
      }
    }
  }

  public void Interact(Player player)
  {
    if (!HasKitchenObject())
    {
      StartCooking();
      return;
    }

    if (!player.HasKitchenObject())
    {
      TryPickupFood(player);
      return;
    }
    else
    {
      TryCombineIngredients(player);
    }
  }

  private void StartCooking()
  {
    SpawnItem(cookingRecipe.rawInput);

    timer = 0f;
    state = State.Cooking;

    Debug.Log("Started Cooking...");
    OnStartedCooking.Invoke(this, EventArgs.Empty);
  }

  private void TryPickupFood(Player player)
  {
    if (kitchenObject.GetFoodState() == FoodState.Raw)
    {
      return;
    }

    kitchenObject.SetKitchenObjectParent(player);

    state = State.Idle;

    Debug.Log("Player picked up food.");
    OnKitchenObjectRemoved.Invoke(this, EventArgs.Empty);
  }

  private void TryCombineIngredients(Player player)
  {
    KitchenObject playerItem = player.GetKitchenObject();
    KitchenObject stationItem = this.GetKitchenObject();

    if (IsValidCombination(playerItem.GetKitchenObjectSO(), stationItem.GetKitchenObjectSO()))
    {
      PerformCombination(player, playerItem, stationItem);
    }
  }

  private bool IsValidCombination(KitchenObjectSO itemA, KitchenObjectSO itemB)
  {
    if (preparedDishRecipe.ingredients.Count != 2) return false;

    bool hasItemA = preparedDishRecipe.ingredients.Contains(itemA);
    bool hasItemB = preparedDishRecipe.ingredients.Contains(itemB);

    bool isDistinct = itemA != itemB;

    return hasItemA && hasItemB && isDistinct;
  }

  private void PerformCombination(Player player, KitchenObject playerItem, KitchenObject stationItem)
  {
    playerItem.DestroySelf();
    stationItem.DestroySelf();

    player.SpawnItem(preparedDishRecipe.preparedDish); // Assuming this extension/method exists

    state = State.Idle;

    Debug.Log("Player combined ingredients into a dish.");
    OnCreatedPreparedDish.Invoke(this, EventArgs.Empty);
  }

  public void SetIdle()
  {
    if (HasKitchenObject()) 
    {
      kitchenObject.DestroySelf();
    }
    state = State.Idle;
  }
}