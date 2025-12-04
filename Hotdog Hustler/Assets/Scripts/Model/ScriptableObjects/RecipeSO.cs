using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Recipe SO")]
public class RecipeSO : ScriptableObject
{
  public List<KitchenObjectSO> ingredients;
  public PreparedDishSO preparedDish;
}
