using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class GameInput : MonoBehaviour
{
  private PlayerInputActions playerInputActions;

  private void Awake()
  {
    playerInputActions = new PlayerInputActions();
    playerInputActions.Player.Enable();
  }

  public event EventHandler OnInteractAction;
  public event EventHandler<OnMovementEventArgs> OnMovementPerformed;
  public event EventHandler OnMovementCancelled;

  private void Start()
  {
    playerInputActions.Player.Interact.performed += Interact_performed;
    playerInputActions.Player.Move.performed += Move_performed;
    playerInputActions.Player.Move.canceled += Move_canceled;
  }

  private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
  {
    OnInteractAction?.Invoke(this, System.EventArgs.Empty);
  }

  private void Move_performed(InputAction.CallbackContext obj)
  {
    OnMovementPerformed?.Invoke(this, new OnMovementEventArgs
    {
      inputVector = GetMovementVectorNormalized()
    });
  }

  private void Move_canceled(InputAction.CallbackContext obj)
  {
    OnMovementCancelled?.Invoke(this, EventArgs.Empty);
  }

  public Vector2 GetMovementVectorNormalized()
  {
    Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
    return inputVector.normalized; // Fixes diagonal movement being faster
  }

  private void OnDestroy()
  {
    playerInputActions.Player.Interact.performed -= Interact_performed;
    playerInputActions.Player.Move.performed -= Move_performed;
    playerInputActions.Player.Move.canceled -= Move_canceled;
    playerInputActions.Dispose();
  }
}
