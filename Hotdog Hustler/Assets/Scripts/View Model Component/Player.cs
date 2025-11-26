using System;
using UnityEngine;

public class Player : KitchenObjectParent
{
  [SerializeField] private float moveSpeed = 7f;
  [SerializeField] private LayerMask countersLayerMask;

  private bool isWalking; //will be used later for animations. Will also need a getter for the visual script.
  private Vector2 lastInputVector;
  private Vector2 movementVector;

  public event EventHandler<OnPlayerMoveEventArgs> OnPlayerMove;
  public class OnPlayerMoveEventArgs : EventArgs
  {
    public Vector2 direction;
  }

  public void Update()
  {
    if (movementVector != Vector2.zero)
    {
      HandleMovement();
    }
  }

  public void HandleInteractions()
  {
    float interactDistance = 2f;

    Debug.DrawRay(transform.position, lastInputVector * interactDistance, Color.red);

    RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, lastInputVector, interactDistance, countersLayerMask);

    if (raycastHit.collider != null)
    {
      if (raycastHit.transform.TryGetComponent(out IInteractable interactable))
      {
        interactable.Interact(this);
      }
    }
  }

  public void HandleMovement()
  {
    Vector3 moveDir = new Vector3(movementVector.x, movementVector.y, 0f);

    transform.position += moveDir * moveSpeed * Time.deltaTime;

    if (movementVector != lastInputVector)
    {
      OnPlayerMove?.Invoke(this, new OnPlayerMoveEventArgs
      {
        direction = movementVector
      });
      lastInputVector = movementVector;
    }

    isWalking = moveDir != Vector3.zero;
  }

  public void SetMovementVector(Vector2 movementVector)
  {
    this.movementVector = movementVector;
  }
}