using NUnit.Framework;
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

            // Spawn Cooked Food
            SpawnItem(cookingRecipe.cookedOutput);
            Debug.Log("Food Cooked!");
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
      SpawnItem(cookingRecipe.rawInput);
      timer = 0f;
      state = State.Cooking;
      Debug.Log("Started Cooking...");
    }
    else
    {
      if (!player.HasKitchenObject())
      {
        kitchenObject.SetKitchenObjectParent(player);
        state = State.Idle;
        Debug.Log("Player picked up food.");
      }
      else
      {
        KitchenObject playerItem = player.GetKitchenObject();
        List<KitchenObjectSO> KitchenObjectSOList = new() {kitchenObject.GetKitchenObjectSO(), playerItem.GetKitchenObjectSO() };
        if (KitchenObjectSOList.All(preparedDishRecipe.ingredients.Contains)) //this works for now, if the recipes keep having only 2 items
        {
          playerItem.DestroySelf();
          kitchenObject.DestroySelf();
          player.SpawnItem(preparedDishRecipe.preparedDish);
          Debug.Log("Player combined ingredients into a dish.");
        }
      }
    }
  }
}