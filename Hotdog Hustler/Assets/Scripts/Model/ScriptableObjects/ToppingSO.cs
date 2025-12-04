using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Topping SO")]
public class ToppingSO : ScriptableObject
{
  public GameObject prefab;
  public string toppingName;
  public ToppingQuality quality;
  public double price;
}
