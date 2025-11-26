using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Topping SO")]
public class ToppingSO : ScriptableObject
{
  public GameObject prefab;
  public string toppingName;
  public Sprite icon;
  public Sprite sprite;
  public ToppingQuality quality;
  public decimal price;
}
