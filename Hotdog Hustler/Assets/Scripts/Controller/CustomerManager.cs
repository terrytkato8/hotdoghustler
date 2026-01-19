using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
  [SerializeField] private GameObject customerPrefab;
  //[SerializeField] private Transform spawnPoint;    // this will be needed later, when the customers spawn outside the screen and walk in. For now customer spawn at the queue points, when there is a free one.
  [SerializeField] private Transform[] queuePoints;

  [SerializeField] private OrderPanelController orderPanelController;
  private float orderTimerInSeconds;

  [SerializeField] private PreparedDishListSO preparedDishListSO;
  private List<PreparedDishSO> preparedDishList;
  private List<ToppingSO> toppingList;

  private List<Customer> customersInLine = new ();
  private Customer frontCustomer;
  private float spawnTimer;

  private bool customerSpawnStop;

  [Header("Spawn Settings")]
  [SerializeField] private float minSpawnDelay = 2f;
  [SerializeField] private float maxSpawnDelay = 7f;
  [SerializeField] private float customerGrowthPerDay = 0.15f;
  [SerializeField] private float rushHourSpawnMultiplier = 1.5f;
  private float customerSpawnMultiplier = 1;
  private float maxSpawnDelayDayAdjusted;
  [SerializeField] private float maxSpawnDelayDayStart = 4f;

  private Order tutorialOrder;

  public static readonly StaticGameEvent<OnCustomerServedEventArgs> OnCustomerServed = new();
  public static readonly StaticGameEvent OnCustomerSpawned = new();

  private bool isActive;

  private void Awake()
  {
    preparedDishList = preparedDishListSO.preparedDishList; //when we have multiple dishes, this will also be set in the Activate() method.
  }

  private void AddListeners()
  {
    ServingStation.OnObjectServed.AddListener(OnObjectServed);
    DayManager.OnLunchStart.AddListener(OnRushHourStart);
    DayManager.OnDinnerStart.AddListener(OnRushHourStart);
    DayManager.On10SecondsLeft.AddListener(On10SecondsLeft);
  }

  private void RemoveListeners()
  {
    ServingStation.OnObjectServed.RemoveListener(OnObjectServed);
    DayManager.OnLunchStart.RemoveListener(OnRushHourStart);
    DayManager.OnDinnerStart.RemoveListener(OnRushHourStart);
    DayManager.On10SecondsLeft.RemoveListener(On10SecondsLeft);
  }

  public void Activate(List<ToppingSO> toppingList, int day, bool needTutorial)
  {
    AddListeners();

    spawnTimer = UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelayDayStart);
    customerSpawnStop = false;

    double customerGrowthRate = Math.Pow(1 - customerGrowthPerDay, day);
    maxSpawnDelayDayAdjusted = maxSpawnDelay * (float)customerGrowthRate;

    Debug.Log("spawn delay: " + maxSpawnDelayDayAdjusted);

    this.toppingList = toppingList;
    isActive = true;

    if (needTutorial)
    {
      SetTutorialOrder();
    }
  }

  public void Deactivate()
  {
    RemoveListeners();

    ClearCustomerQueue();
    isActive = false;
  }

  public bool IsActive() 
  {
    return isActive;
  }

  void Update()
  {
    if (isActive)
    {
      SpawnLogic();
      OrderLogic();
    }
  }

  private void SpawnLogic()
  {
    //This check will be later removed, since when a sixth customer spawns, he will just walk by.
    if (customersInLine.Count < queuePoints.Length)
    {
      spawnTimer -= Time.deltaTime * customerSpawnMultiplier;

      if (spawnTimer <= 0f && !customerSpawnStop)
      {
        SpawnCustomer();
        spawnTimer = UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelayDayAdjusted);
      }
    }
  }

  private void OrderLogic()
  {
    if (GetFrontCustomer() == null) return;

    bool isTimerPaused = tutorialOrder != null;
    if (!isTimerPaused)
    {
      orderTimerInSeconds -= Time.deltaTime;
    }

    orderPanelController.UpdateVisuals(Mathf.Max(0, orderTimerInSeconds));

    if (orderTimerInSeconds <= 0)
    {
      FailOrder();
    }
  }

  private void FailOrder()
  {
    ServedOrder failedOrder = new(null, 0, 0);
    OnCustomerServed.Invoke(this, new OnCustomerServedEventArgs { servedOrder = failedOrder });

    CustomerLeaves(frontCustomer);
  }

  private void SpawnCustomer()
  {
    int queueIndex = customersInLine.Count;
    Vector3 targetPos = queuePoints[queueIndex].position;

    GameObject customerGameObject = Instantiate(customerPrefab, targetPos, Quaternion.identity);
    Customer newCustomer = customerGameObject.GetComponent<Customer>();

    if (tutorialOrder != null)
    {
      newCustomer.Setup(tutorialOrder);
      customerSpawnStop = true;
    }
    else
    {
      newCustomer.Setup(GetRandomOrder());
    }
    customersInLine.Add(newCustomer);

    if (queueIndex == 0) //if this customer spawns at the frontspot, update the Order Panel
    {
      frontCustomer = newCustomer;
      orderPanelController.ShowOrderPanel(frontCustomer.GetOrder());
      orderTimerInSeconds = frontCustomer.GetPatienceTime();
    }

    OnCustomerSpawned.Invoke(this, EventArgs.Empty);
  }

  public Customer GetFrontCustomer()
  {
    if (customersInLine.Count > 0)
      return customersInLine[0];
    else
      return null;
  }

  public void CustomerLeaves(Customer customer)
  {
    if (customer != null && customersInLine.Contains(customer))
    {
      customersInLine.Remove(customer);
      Destroy(customer.gameObject);
      frontCustomer = null;
      orderPanelController.HideOrderPanel();
      UpdateQueuePositions();

      if (tutorialOrder != null)
      {
        customerSpawnStop = false;
        tutorialOrder = null;
      }

      Debug.Log("customer leaves");
    }
  }

  private void UpdateQueuePositions()
  {
    for (int i = 0; i < customersInLine.Count; i++)
    {
      customersInLine[i].SetPosition(queuePoints[i].position);
    }

    frontCustomer = GetFrontCustomer();
    if (frontCustomer != null)
    {
      orderPanelController.ShowOrderPanel(frontCustomer.GetOrder());
      orderTimerInSeconds = frontCustomer.GetPatienceTime();
    }
  }

  private void ClearCustomerQueue()
  {
    int customersInLineCount = customersInLine.Count;
    for (int i = 0; i < customersInLineCount; i++)
    {
      CustomerLeaves(customersInLine[0]);
    }
  }

  private Order GetRandomOrder()
  {
    int dishIndex = UnityEngine.Random.Range(0, preparedDishList.Count);
    PreparedDishSO wantedDish = preparedDishList[dishIndex];

    //int wantedToppingAmount = GetRandomToppingAmount();
    int wantedToppingAmount = 2; // set to 2 right now for testing, GetRandomToppingAmount() otherwise.
    List<ToppingSO> wantedToppingList = new();
    for (int i = 0; i < wantedToppingAmount; i++)
    {
      int toppingIndex = UnityEngine.Random.Range(0, toppingList.Count);
      ToppingSO wantedTopping = toppingList[toppingIndex];
      wantedToppingList.Add(wantedTopping);
    }

    Order wantedOrder = new(wantedDish, wantedToppingList);
    return wantedOrder;
  }

  private int GetRandomToppingAmount()
  {
    float randomValue = UnityEngine.Random.value; // Returns a float between 0.0 and 1.0

    //10% chance for no toppings, 45% chance for 1 and 2 topppings each
    if (randomValue < 0.1f)
    {
      return 0;
    }
    else if (randomValue < 0.55f)
    {
      return 1;
    }
    else
    {
      return 2;
    }
  }

  private void SetTutorialOrder()
  {
    PreparedDishSO wantedDish = preparedDishList[0]; //HotDog

    List<ToppingSO> wantedToppingList = new();
    wantedToppingList.Add(toppingList[0]); //Ketchup
    wantedToppingList.Add(toppingList[1]); //Mustard

    tutorialOrder = new(wantedDish, wantedToppingList);
  }

  private double CalculateMoneyPaid(Order order, double accuracy)
  {
    double totalPrice = order.GetTotalPrice();
    return Math.Round(totalPrice * accuracy, 2);
  }

  private void OnObjectServed(object sender, KitchenObjectEventArgs e)
  {
    if (frontCustomer == null) return;

    KitchenObject playerKitchenObject = e.kitchenObject;

    Order playerPlate = new(playerKitchenObject.GetPreparedDishSO(), playerKitchenObject.GetToppings());
    double accuracy = frontCustomer.ValidateOrder(playerPlate);
    double moneyPaid = CalculateMoneyPaid(playerPlate, accuracy);

    ServedOrder servedOrder = new(playerPlate, accuracy, moneyPaid);
    OnCustomerServed.Invoke(this, new OnCustomerServedEventArgs
    {
      servedOrder = servedOrder
    });

    playerKitchenObject.DestroySelf();
    CustomerLeaves(frontCustomer);
  }

  private void OnRushHourStart(object sender, EventArgs e)
  {
    customerSpawnMultiplier = rushHourSpawnMultiplier;
  }

  private void OnRushHourEnd(object sender, EventArgs e)
  {
    customerSpawnMultiplier = 1.0f; // Reset Spawn Multiplier
  }

  private void On10SecondsLeft(object sender, EventArgs e)
  {
    customerSpawnStop = true;
  }

  public OrderPanelController GetOrderPanelController()
  {
    return orderPanelController;
  }
}
