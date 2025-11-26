using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/CookingRecipe SO")]
public class CookingRecipeSO : ScriptableObject
{
  public KitchenObjectSO rawInput;

  public KitchenObjectSO cookedOutput;
  public float cookingTimeMax;

  public KitchenObjectSO burntOutput;
  public float burningTimeMax;         
}
