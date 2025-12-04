using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DayClockPanelController : MonoBehaviour
{
  [Header("UI References")]
  [SerializeField] private TextMeshProUGUI timerText;
  [SerializeField] private Image clockFillImage; // Optional: For a radial pie-chart timer

  [Header("Settings")]
  [SerializeField] private Color normalColor = Color.white;
  [SerializeField] private Color panicColor = Color.red;
  [SerializeField] private float panicThreshold = 10f; // Seconds remaining to turn red

  public event EventHandler OnDayTimeIsUp;
  private bool hasInvokedEvent;

  private float timeRemaining;
  private float maxTime;

  public void Show(float maxTime)
  {
    gameObject.SetActive(true);
    hasInvokedEvent = false;
    this.maxTime = maxTime;
    timeRemaining = this.maxTime;
  }

  public void Hide()
  {
    gameObject.SetActive(false);
  }

  private void Update()
  {
    UpdateClock();
    if (timeRemaining <= 0 && !hasInvokedEvent) 
    {
      OnDayTimeIsUp?.Invoke(this, EventArgs.Empty);
      hasInvokedEvent= true;
    }
  }

  public void UpdateClock()
  {
    timeRemaining = Mathf.Max(0, timeRemaining -= Time.deltaTime);

    float minutes = Mathf.FloorToInt(timeRemaining / 60);
    float seconds = Mathf.FloorToInt(timeRemaining % 60);

    timerText.text = string.Format("{0:0}:{1:00}", minutes, seconds);

    if (clockFillImage != null)
    {
      clockFillImage.fillAmount = timeRemaining / maxTime;
    }

    if (timeRemaining <= panicThreshold)
    {
      timerText.color = panicColor;
      if (clockFillImage != null) clockFillImage.color = panicColor;
    }
    else
    {
      timerText.color = normalColor;
      if (clockFillImage != null) clockFillImage.color = normalColor;
    }
  }
}
