using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.VisualScripting;

public class OrderPanelController : MonoBehaviour
{
  [SerializeField] private GameObject panelContent;
  [SerializeField] private Image orderIconImage;
  [SerializeField] private TextMeshProUGUI timerText;

  public void ShowOrderPanel(Order order)
  {
    panelContent.SetActive(true);

    if (order.wantedDish.prefab.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
    {
      SpriteRenderer prefabSpriteComponent = spriteRenderer;
      orderIconImage.sprite = prefabSpriteComponent.sprite;
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
