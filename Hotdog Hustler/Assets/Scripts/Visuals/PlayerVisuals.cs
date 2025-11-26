using UnityEngine;

public class PlayerVisuals : MonoBehaviour
{
  [SerializeField] private Player player;
  [SerializeField] private SpriteRenderer spriteRenderer;

  [Header("Direction Sprites")]
  [SerializeField] private Sprite spriteUp;
  [SerializeField] private Sprite spriteDown;
  [SerializeField] private Sprite spriteLeft;
  [SerializeField] private Sprite spriteRight;

  private void Start()
  {
    player.OnPlayerMove += Player_OnPlayerMove;
  }

  private void Player_OnPlayerMove(object sender, Player.OnPlayerMoveEventArgs e)
  {
    UpdateVisuals(e.direction);
  }

  private void UpdateVisuals(Vector2 dir)
  {
    if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
    {
      // Horizontal
      if (dir.x > 0) spriteRenderer.sprite = spriteRight;
      else spriteRenderer.sprite = spriteLeft;
    }
    else
    {
      // Vertical
      if (dir.y > 0) spriteRenderer.sprite = spriteUp;
      else spriteRenderer.sprite = spriteDown;
    }
  }
}
