using System;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
  [SerializeField] private TutorialPanelController tutorialPanelController;

  private CookStation grillStation;
  private CookStation toastStation;
  private ServingStation servingStation;
  private ToppingStation toppingStation;
  private OrderPanelController orderPanelController;
  private ToppingMenuPanelController toppingMenuPanelController;

  private enum TutorialStep
  {
    WaitingToStart,
    Welcome,
    ExplainOrder,
    CookBunAndSausage,
    WaitForCook,
    CombineToHotdog,
    GoToToppingStation,
    ExplainTopping,
    ExplainServing,
    ExplainRestOfTheDay,
    ExplainEndOfDayScreen,
    ExplainShoppingScreen,
    Completed
  }
  private TutorialStep currentStep;

  private bool isActive;
  private bool isWaitingForInput;

  private bool grillStationStartedCooking = false;
  private bool toastStationStartedCooking = false;
  private bool grillStationFinishedCooking = false;
  private bool toastStationFinishedCooking = false;

  private KitchenObject playerKitchenObject;

  public static readonly StaticGameEvent OnTutorialOrderServed = new();

  public void Activate(CookStation grillStation, CookStation toastStation, ServingStation servingStation, ToppingStation toppingStation, OrderPanelController orderPanelController, ToppingMenuPanelController toppingMenuPanelController)
  {
    this.grillStation = grillStation;
    this.toastStation = toastStation;
    this.servingStation = servingStation;
    this.toppingStation = toppingStation;
    this.orderPanelController = orderPanelController;
    this.toppingMenuPanelController = toppingMenuPanelController;

    isActive = true;
    currentStep = TutorialStep.WaitingToStart;

    CookStation.OnStartedCooking.AddListener(CookStation_OnStartedCooking);
    CookStation.OnFinishedCooking.AddListener(CookStation_OnFinishedCooking);
    CookStation.OnCreatedPreparedDish.AddListener(CookStation_OnCreatedPreparedDish);
    ToppingStation.OnToppingStationActivated.AddListener(ToppingStation_OnActivated);
    CustomerManager.OnCustomerSpawned.AddListener(CustomerManager_OnCustomerSpawned);
    CustomerManager.OnCustomerServed.AddListener(CustomerManager_OnCustomerServed);
  }

  public void Deactivate()
  {
    isActive = false;
    CookStation.OnStartedCooking.RemoveListener(CookStation_OnStartedCooking);
    CookStation.OnFinishedCooking.RemoveListener(CookStation_OnFinishedCooking);
    CookStation.OnCreatedPreparedDish.RemoveListener(CookStation_OnCreatedPreparedDish);
    ToppingStation.OnToppingStationActivated.RemoveListener(ToppingStation_OnActivated);
    CustomerManager.OnCustomerSpawned.RemoveListener(CustomerManager_OnCustomerSpawned);
    CustomerManager.OnCustomerServed.RemoveListener(CustomerManager_OnCustomerServed);
  }

  public void AdvanceTutorial()
  {
    isWaitingForInput = false;
    ResumeGame();

    switch (currentStep)
    {
      case TutorialStep.WaitingToStart:
        SetStep(TutorialStep.Welcome);
        break;

      case TutorialStep.ExplainOrder:
        // Now guide them to the cooking station
        SetStep(TutorialStep.CookBunAndSausage);
        break;

      case TutorialStep.ExplainTopping:
        if (playerKitchenObject.GetToppings().Count != 0)
        {
          SetStep(TutorialStep.ExplainServing);
        }
        else
        {
          ResumeGame();
        }
        break;

      default:
        // in most case we simply resume the game
        ResumeGame();
        break;
    }
  }

  private void SetStep(TutorialStep step)
  {
    currentStep = step;
    isWaitingForInput = true;
    PauseGame(); // Freeze time whenever a popup appears

    switch (step)
    {
      case TutorialStep.Welcome:
        tutorialPanelController.ShowTutorial(
          "Welcome to Hotdog Hustler! Let's learn how to cook.",
          null, false);
        break;

      case TutorialStep.ExplainOrder:
        tutorialPanelController.ShowTutorial(
          "A customer appeared! Look at their Order at the top. It has a dish and up to two toppings.",
          orderPanelController.GetBackgroundTransform(), false); // Highlighting UI
        break;

      case TutorialStep.CookBunAndSausage:
        tutorialPanelController.ShowTutorial(
          "We need a Bun and a Sausage. Walk to the Toaster and the Grill and interact with them (Press Space). They will automatically start cooking!",
          toastStation.transform, grillStation.transform, true); // Highlighting World Obj
        break;

      case TutorialStep.WaitForCook:
        tutorialPanelController.ShowTutorial(
          "Great! Now we wait for them to cook.",
          null, false);
        break;

      case TutorialStep.CombineToHotdog:
        tutorialPanelController.ShowTutorial(
          "Both the Bun and the Sausage are done cooking. Pick one of them up and while holding it, pick up the other one. It will automatically combine to a Hotdog!",
          toastStation.transform, grillStation.transform, true);
        break;

      case TutorialStep.GoToToppingStation:
        tutorialPanelController.ShowTutorial(
          "While holding the Hotdog, interact with the Topping Station to add the Toppings.",
          toppingStation.transform, true);
        break;

      case TutorialStep.ExplainTopping:
        tutorialPanelController.ShowTutorial(
          "Now choose the toppings the customer wants! Make sure you put them on in the right order!",
          toppingMenuPanelController.GetVisualPanelTransform(), false);
        break;

      case TutorialStep.ExplainServing:
        tutorialPanelController.ShowTutorial(
          "Take the finished Hotdog to the Window to get paid! The more accurate the served Dish is, the more money you get!",
          servingStation.transform, true);
        break;

      case TutorialStep.ExplainRestOfTheDay:
        tutorialPanelController.ShowTutorial(
          "Awesome! Now make sure to serve the customers for the rest of the day!",
          null, false);
        break;

      case TutorialStep.ExplainEndOfDayScreen:
        tutorialPanelController.ShowTutorial(
          "The day ended! At the end of the day you will be able to see how many Orders you served, how accurate you were and how much money you made.",
          null, false);
        break;

      case TutorialStep.ExplainShoppingScreen:
        tutorialPanelController.ShowTutorial(
          "This is the Shopping screen. Here you can use your earned Money to unlock new Toppings.",
          null, false);
        break;

      case TutorialStep.Completed:
        tutorialPanelController.ShowTutorial(
          "That's it! Have fun with Hotdog Hustler!",
          null, false);
        break;
    }
  }

  private void PauseGame()
  {
    Time.timeScale = 0f;
  }

  private void ResumeGame()
  {
    Time.timeScale = 1f;
    tutorialPanelController.Hide();
  }

  private void CustomerManager_OnCustomerSpawned(object sender, EventArgs e)
  {
    if (currentStep == TutorialStep.Welcome)
    {
      SetStep(TutorialStep.ExplainOrder);
    }
  }

  private void CookStation_OnStartedCooking(object sender, EventArgs e)
  {
    if (currentStep == TutorialStep.CookBunAndSausage)
    {
      var station = sender as CookStation;
      if (station == grillStation) grillStationStartedCooking = true;
      if (station == toastStation) toastStationStartedCooking = true;

      if (grillStationStartedCooking && toastStationStartedCooking)
      {
        SetStep(TutorialStep.WaitForCook);
      }
    }
  }

  private void CookStation_OnFinishedCooking(object sender, EventArgs e)
  {
    if (currentStep == TutorialStep.WaitForCook)
    {
      var station = sender as CookStation;
      if (station == grillStation) grillStationFinishedCooking = true;
      if (station == toastStation) toastStationFinishedCooking = true;

      if (grillStationFinishedCooking && toastStationFinishedCooking)
      {
        SetStep(TutorialStep.CombineToHotdog);
      }
    }
  }

  private void CookStation_OnCreatedPreparedDish(object sender, EventArgs e)
  {
    if (currentStep == TutorialStep.CombineToHotdog)
    {
      SetStep(TutorialStep.GoToToppingStation);
    }
  }

  private void ToppingStation_OnActivated(object sender, KitchenObjectEventArgs e)
  {
    if (currentStep == TutorialStep.GoToToppingStation)
    {
      playerKitchenObject = e.kitchenObject;
      SetStep(TutorialStep.ExplainTopping);
    }
  }

  private void CustomerManager_OnCustomerServed(object sender, OnCustomerServedEventArgs e)
  {
    if (currentStep == TutorialStep.ExplainServing)
    {
      SetStep(TutorialStep.ExplainRestOfTheDay);
      OnTutorialOrderServed.Invoke(this);
    }
  }

  public void OnEndOfDayScreenReached()
  {
    SetStep(TutorialStep.ExplainEndOfDayScreen);
  }

  public void OnShoppingScreenReached()
  {
    SetStep(TutorialStep.ExplainShoppingScreen);
  }

  public void OnShoppingScreenExited()
  {
    SetStep(TutorialStep.Completed);
  }

  public bool IsActive()
  {
    return isActive;
  }

  public bool IsWaitingForInput()
  {
    return isActive && isWaitingForInput;
  }
}
