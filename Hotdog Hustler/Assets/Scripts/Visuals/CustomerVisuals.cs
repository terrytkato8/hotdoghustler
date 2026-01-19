using System;
using UnityEngine;

public class CustomerVisuals : MonoBehaviour
{
  [SerializeField] private Customer customer;
  [SerializeField] private SpriteRenderer spriteRenderer;
  [SerializeField] private Sprite[] possibleSprites;

  private static Sprite lastUsedSprite;

  // Update is called once per frame
  private void Awake()
  {
    customer.OnSetup += Customer_OnSetup;
  }

  private void OnDestroy()
  {
    customer.OnSetup -= Customer_OnSetup;
  }

  private void Customer_OnSetup(object sender, EventArgs e)
  {
    Sprite chosen;
    do
    {
      chosen = possibleSprites[UnityEngine.Random.Range(0, possibleSprites.Length)];
    }
    while (chosen == lastUsedSprite);

    spriteRenderer.sprite = lastUsedSprite = chosen;
  }
}
