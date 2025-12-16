using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class EndOfDayPanelController : MonoBehaviour
{
  [SerializeField] TextMeshProUGUI dayValue;
  [SerializeField] TextMeshProUGUI customersServedValue;
  [SerializeField] TextMeshProUGUI accuracyValue;
  [SerializeField] TextMeshProUGUI moneyMadeValue;

  public void Show(int day, int customersServed, double accuracy, double moneyMade)
  {
    gameObject.SetActive(true);

    dayValue.text = "Day " + day + ": End";
    customersServedValue.text = customersServed.ToString();
    accuracyValue.text = String.Format("{0:0.00}", accuracy) + "%";
    moneyMadeValue.text = "$" + moneyMade.ToString();
  }

  public void Hide()
  {
    gameObject.SetActive(false);
  }
}
