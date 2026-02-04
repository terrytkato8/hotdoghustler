using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayPanelController : MonoBehaviour
{
  [Header("UI References")]
  [SerializeField] private GameObject dayPanel;
  [SerializeField] private TextMeshProUGUI timerText;
  [SerializeField] private Image clockFillImage;
  [SerializeField] private TextMeshProUGUI dayNumberText;   
  [SerializeField] private TextMeshProUGUI moneyText;      
  [SerializeField] private TextMeshProUGUI customersServedText;

  [Header("Settings")]
  [SerializeField] private Color normalColor = Color.white;
  [SerializeField] private Color panicColor = Color.red;
  [SerializeField] private float panicThreshold = 10f; // Seconds remaining to turn red

  private float maxDayTime;

  public void Show(float maxDayTime, int currentDay)
  {
    dayPanel.SetActive(true);
    this.maxDayTime = maxDayTime;

    dayNumberText.text = $"DAY {currentDay}";

    UpdateCustomersServed(0, 0);
    UpdateClock(maxDayTime);
  }

  public void Hide()
  {
    dayPanel.SetActive(false);
  }

  public void UpdateCustomersServed(int count, double amount)
  {
    customersServedText.text = $"Customer served: {count}";
    moneyText.text = $"Money made Today: {amount}";
  }

  public void UpdateClock(float timeRemaining)
  {
    float minutes = Mathf.FloorToInt(timeRemaining / 60);
    float seconds = Mathf.FloorToInt(timeRemaining % 60);

    timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);

    if (maxDayTime > 0)
      clockFillImage.fillAmount = timeRemaining / maxDayTime;

    bool isPanic = timeRemaining <= panicThreshold;
    Color targetColor = isPanic ? panicColor : normalColor;

    timerText.color = targetColor;
    clockFillImage.color = targetColor;
  }
}
