using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TutorialPanelController : MonoBehaviour
{
  [Header("UI References")]
  [SerializeField] private GameObject contentParent;
  [SerializeField] private TextMeshProUGUI explanationText;
  //[SerializeField] private RectTransform firstHighlightFrame;
  //[SerializeField] private RectTransform secondHighlightFrame;

  //private Transform firstCurrentTarget;
  //private Transform secondCurrentTarget;
  private Transform currentTarget;
  private bool isTargetWorldObject;

  public void ShowTutorial(string text, Transform targetToHighlight, bool isWorldObj)
  {
    ShowTutorial(text, targetToHighlight, null, isWorldObj);

    contentParent.SetActive(true);
    explanationText.text = text;

    currentTarget = targetToHighlight;
    isTargetWorldObject = isWorldObj;

    //UpdateHighlightPosition();
  }

  public void ShowTutorial(string text, Transform firstTargetToHighlight, Transform secondTargetToHighlight, bool isWorldObj)
  {
    contentParent.SetActive(true);
    explanationText.text = text;

    //firstCurrentTarget = firstTargetToHighlight;
    //secondCurrentTarget = secondTargetToHighlight;
    isTargetWorldObject = isWorldObj;

    //UpdateHighlightPosition();
  }

  public void Hide()
  {
    contentParent.SetActive(false);
  }

  /*private void UpdateHighlightPosition()
  {
    if (isTargetWorldObject)
    {
      Vector3 firstScreenPos = Camera.main.WorldToScreenPoint(currentTarget.position);
      Vector3 secondScreenPos = Camera.main.WorldToScreenPoint(secondCurrentTarget.position);
      firstHighlightFrame.position = firstScreenPos;
      secondHighlightFrame.position = secondScreenPos;
    }
    else
    {
      firstHighlightFrame.position = firstCurrentTarget.position;
      secondHighlightFrame.position = secondCurrentTarget.position;
    }
  }*/
}
