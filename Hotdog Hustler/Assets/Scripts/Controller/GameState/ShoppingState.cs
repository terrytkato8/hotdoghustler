using UnityEngine;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class ShoppingState : BaseGameState
{
  private double currentBalance;

  public enum ShopMenuState { Categories, Toppings }
  private ShopMenuState currentState;

  public override void Enter()
  {
    base.Enter();
    Debug.Log("entered shopping State");

    List<ToppingSO> lockedToppings = ProgressionManager.GetLockedToppings();
    currentBalance = ProgressionManager.GetCurrentBalance();
    ShopMenuPanelController.Show(currentBalance, lockedToppings);

    currentState = ShopMenuState.Categories;

    if (TutorialManager.IsActive())
    {
      TutorialManager.OnShoppingScreenReached();
    }
  }

  public override void Exit()
  {
    base.Exit();

    ProgressionManager.EndDay(currentBalance);

    ShopMenuPanelController.Hide();
  }

  protected override void OnRegularInteraction()
  {
    switch (currentState)
    {
      case ShopMenuState.Categories:
        bool notExiting = ShopMenuPanelController.ShowToppings();
        if (notExiting)
        {
          currentState = ShopMenuState.Toppings;
        }
        else
        {
          owner.ChangeState<InitDayState>();
        }
        break;
      case ShopMenuState.Toppings:
        ToppingSO topping = ShopMenuPanelController.GetSelectedTopping();

        if (topping == null)
        {
          ShopMenuPanelController.ShowCategories();
          currentState = ShopMenuState.Categories;
          return;
        }

        if (topping.unlockPrice > currentBalance)
          return;

        currentBalance -= topping.unlockPrice;
        ProgressionManager.UnlockTopping(topping);
        ShopMenuPanelController.BuyTopping(topping, currentBalance);
        MainAudio.PurchaseItem();

        if (TutorialManager.IsActive())
          TutorialManager.OnShoppingScreenExited();

        break;
    }
  }

  protected override void OnMove(object sender, OnMovementEventArgs e)
  {
    if (!TutorialManager.IsWaitingForInput())
      ShopMenuPanelController.Navigate(e.inputVector);
  }
}
