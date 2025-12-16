using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "ScriptableObjects/ToppingList SO")]
public class ToppingListSO : ScriptableObject
{
    public List<ToppingSO> toppingList;
}
