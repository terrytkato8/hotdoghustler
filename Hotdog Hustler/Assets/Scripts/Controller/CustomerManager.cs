using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
  [Header("References")]
  [SerializeField] private GameObject customerPrefab;
  [SerializeField] private Transform[] queuePoints;
  //[SerializeField] private Transform spawnPoint;    // this will be needed later, when the customers spawn outside the screen and walk in. For now customer spawn at the queue points, when there is a free one.
  [SerializeField] private OrderPanelController orderPanelController;
  [SerializeField] private OrderGenerator orderGenerator;
  [SerializeField] private PreparedDishListSO preparedDishListSO;

  [Header("Settings")]
  [SerializeField] private float minSpawnDelay;
  [SerializeField] private float maxSpawnDelay;
  [SerializeField] private float rushHourSpawnMultiplier = 1.5f;
  [SerializeField] private float maxSpawnDelayDayStart = 4f;
  [SerializeField] private float minSpawnDelayDayStart = 1f;

  //State
  private List<Customer> customersInLine = new();
  private Customer frontCustomer;
  private float spawnTimer;
  private float currentSpawnMultiplier = 1;
  private float customerPatienceMultiplier;
  private bool isServingCustomer = false;
  private bool isSpawningPaused;
  private bool isActive;

  //Tutorial
  private Order tutorialOrder;

  //Events
  public static readonly StaticGameEvent<OnCustomerServedEventArgs> OnCustomerServed = new();
  public static readonly StaticGameEvent OnCustomerSpawned = new();


  // --- SETUP ---

  public void Activate(List<ToppingSO> toppingList, bool needTutorial, DailyDifficulty dailyDifficulty)
  {
    orderGenerator = new(preparedDishListSO.preparedDishList, toppingList);

    SetupTutorial(needTutorial);
    SetupDifficulty(dailyDifficulty);

    spawnTimer = UnityEngine.Random.Range(minSpawnDelayDayStart, maxSpawnDelayDayStart);
    isSpawningPaused = false;
    isActive = true;

    ServingStation.OnObjectServed.AddListener(OnObjectServed);
    DayManager.OnLunchStart.AddListener(OnRushHourStart);
    DayManager.OnDinnerStart.AddListener(OnRushHourStart);
    DayManager.OnLunchEnd.AddListener(OnRushHourEnd);
    DayManager.OnDinnerEnd.AddListener(OnRushHourEnd);
    DayManager.On10SecondsLeft.AddListener(On10SecondsLeft);
  }

  public void Deactivate(Action OnCleanupComplete)
  {
    isActive = false;
    StartCoroutine(ClearCustomerQueue(OnCleanupComplete));

    ServingStation.OnObjectServed.RemoveListener(OnObjectServed);
    DayManager.OnLunchStart.RemoveListener(OnRushHourStart);
    DayManager.OnDinnerStart.RemoveListener(OnRushHourStart);
    DayManager.OnLunchEnd.RemoveListener(OnRushHourEnd);
    DayManager.OnDinnerEnd.RemoveListener(OnRushHourEnd);
    DayManager.On10SecondsLeft.RemoveListener(On10SecondsLeft);
  }

  void Update()
  {
    if (!isActive) return;
    
    HandleSpawning();
    HandleFrontCustomer();
  }

  // --- CUSTOMER SPAWN & DESPAWN LOGIC ---

  private void HandleSpawning()
  {
    //This check will be later removed, since when a sixth customer spawns, he will just walk by.
    if (customersInLine.Count >= queuePoints.Length || isSpawningPaused) return;
    
    spawnTimer -= Time.deltaTime * currentSpawnMultiplier;

    if (spawnTimer <= 0f)
    {
      SpawnCustomer();
      spawnTimer = UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelay);
    }
  }

  private void HandleFrontCustomer()
  {
    if (customersInLine.Count == 0 || tutorialOrder != null || isServingCustomer) return;

    orderPanelController.UpdateVisuals(Mathf.Max(0, frontCustomer.GetPatienceTime()));

    if (frontCustomer.IsPatienceExhausted())
    {
      StartCoroutine(ServeCustomer(frontCustomer, null));
    }
  }

  private void SpawnCustomer()
  {
    int queueIndex = customersInLine.Count;

    GameObject customerGameObject = Instantiate(customerPrefab, queuePoints[queueIndex].position, Quaternion.identity);
    Customer newCustomer = customerGameObject.GetComponent<Customer>();

    Order newOrder;
    if (tutorialOrder != null)
    {
      newOrder = tutorialOrder;
      isSpawningPaused = true;
    }
    else
    {
      newOrder = orderGenerator.GenerateRandomOrder();
    }

    newCustomer.Setup(newOrder, customerPatienceMultiplier);
    customersInLine.Add(newCustomer);

    if (queueIndex == 0) //if this customer spawns at the frontspot, update the Order Panel
    {
      ActivateFrontCustomer();
    }

    OnCustomerSpawned.Invoke(this, EventArgs.Empty);
  }

  private void ActivateFrontCustomer()
  {
    frontCustomer = customersInLine[0];
    frontCustomer.StartPatienceTimer();
    orderPanelController.ShowOrderPanel(frontCustomer.GetOrder());
  }

  private void CustomerLeaves(Customer customer)
  {
    if (!customersInLine.Contains(customer)) return;

    customersInLine.Remove(customer);
    Destroy(customer.gameObject);

    if (tutorialOrder != null)
    {
      isSpawningPaused = false;
      tutorialOrder = null;
    }

    Debug.Log("customer leaves");

    UpdateQueuePositions();
  }

  private void UpdateQueuePositions()
  {
    for (int i = 0; i < customersInLine.Count; i++)
    {
      customersInLine[i].SetPosition(queuePoints[i].position);
    }

    if (customersInLine.Count > 0)
    {
      ActivateFrontCustomer();
    }
    else
    {
      frontCustomer = null;
    }
  }

  private IEnumerator ClearCustomerQueue(Action onComplete)
  {
    List<Coroutine> activeRoutines = new();
    foreach (Customer c in customersInLine)
    {
      Coroutine routine = StartCoroutine(ServeCustomer(c, null));
      activeRoutines.Add(routine);
    }

    foreach (Coroutine routine in activeRoutines)
    {
      yield return routine;
    }

    Debug.Log("All customers have left. Invoking callback.");
    onComplete?.Invoke();
  }

  // --- SERVING LOGIC ---

  private void OnObjectServed(object sender, KitchenObjectEventArgs e)
  {
    if (frontCustomer == null) return;

    StartCoroutine(ServeCustomer(frontCustomer, e.kitchenObject));
  }

  private IEnumerator ServeCustomer(Customer customer, KitchenObject playerKitchenObject)
  {
    isServingCustomer = true;

    ServedOrder servedOrder = ProcessOrderData(customer, playerKitchenObject);

    OnCustomerServed.Invoke(this, new OnCustomerServedEventArgs
    {
      servedOrder = servedOrder
    });

    Debug.Log("IEnumerator ServeCustomer from customer manager");

    orderPanelController.HideOrderPanel();

    yield return StartCoroutine(customer.ReactToFood(servedOrder.accuracy));

    CustomerLeaves(customer);
    isServingCustomer = false;
  }

  private ServedOrder ProcessOrderData(Customer customer, KitchenObject playerKitchenObject)
  {
    Order playerPlate = null;

    if (playerKitchenObject != null)
    {
      playerPlate = new Order(
          playerKitchenObject.GetPreparedDishSO(),
          playerKitchenObject.GetToppings()
      );
      playerKitchenObject.DestroySelf();
    }

    double accuracy = customer.ValidateOrder(playerPlate);
    double moneyPaid = CalculateMoneyPaid(playerPlate, accuracy);

    return new ServedOrder(playerPlate, accuracy, moneyPaid);
  }

  // --- HELPERS ---

  private double CalculateMoneyPaid(Order order, double accuracy)
  {
    if (order == null || accuracy == 0)
      return 0;

    double totalPrice = order.GetTotalPrice();
    return Math.Round(totalPrice * accuracy, 2);
  }

  private void SetupTutorial(bool needTutorial)
  {
    if (needTutorial)
    {
      // Hardcoded tutorial: Hotdog + Ketchup + Mustard
      tutorialOrder = orderGenerator.GenerateSpecificOrder(0, new int[] { 0, 1 });
    }
    else
    {
      tutorialOrder = null;
    }
  }

  private void SetupDifficulty(DailyDifficulty dailyDifficulty)
  {
    minSpawnDelay = dailyDifficulty.MinSpawnDelay;
    maxSpawnDelay = dailyDifficulty.MaxSpawnDelay;
    customerPatienceMultiplier = dailyDifficulty.PatienceMultiplier;
  }

  // --- EVENT LISTENERS ---

  private void OnRushHourStart(object sender, EventArgs e) => currentSpawnMultiplier = rushHourSpawnMultiplier;
  private void OnRushHourEnd(object sender, EventArgs e) => currentSpawnMultiplier = 1.0f; // Reset Spawn Multiplier
  private void On10SecondsLeft(object sender, EventArgs e) => isSpawningPaused = true;

  // --- GETTER ---

  public OrderPanelController GetOrderPanelController() => orderPanelController;

}
