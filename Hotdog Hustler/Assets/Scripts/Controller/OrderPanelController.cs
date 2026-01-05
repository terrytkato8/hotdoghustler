using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class OrderPanelController : MonoBehaviour
{
  [SerializeField] private GameObject panelContent;
  [SerializeField] private Image dishIcon;
  [SerializeField] private Image toppingIcon1;
  [SerializeField] private Image toppingIcon2;
  [SerializeField] private TextMeshProUGUI timerText;

  public void ShowOrderPanel(Order order)
  {
    panelContent.SetActive(true);

    if (order.wantedDish.prefab.TryGetComponent<SpriteRenderer>(out var dishSpriteRenderer))
    {
      dishIcon.sprite = dishSpriteRenderer.sprite;
    }

    toppingIcon1.gameObject.SetActive(false);
    toppingIcon2.gameObject.SetActive(false);

    if (order.wantedToppings.Count > 0)
    {
      toppingIcon1.gameObject.SetActive(true);
      if (order.wantedToppings[0].prefab.TryGetComponent<SpriteRenderer>(out var toppingSpriteRenderer1))
      {
        toppingIcon1.sprite = toppingSpriteRenderer1.sprite;
      }
    }

    if (order.wantedToppings.Count > 1)
    {
      toppingIcon2.gameObject.SetActive(true);
      if (order.wantedToppings[1].prefab.TryGetComponent<SpriteRenderer>(out var toppingSpriteRenderer2))
      {
        toppingIcon2.sprite = toppingSpriteRenderer2.sprite;
      }
    }
  }

  public void UpdateVisuals(float timerInSeconds)
  {
    int timeLeftInSeconds = Mathf.FloorToInt(timerInSeconds);
    timerText.text = timeLeftInSeconds + " seconds left";

    if (timeLeftInSeconds <= 5f)
    {
      timerText.color = Color.red;
    }
    else
    {
      timerText.color = Color.white;
    }
  }

  public void HideOrderPanel()
  {
    panelContent.SetActive(false);
  }
}
