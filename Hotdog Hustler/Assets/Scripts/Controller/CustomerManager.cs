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

  [Header("Spawn Settings")]
  [SerializeField] private float minSpawnDelay = 2f;
  [SerializeField] private float maxSpawnDelay = 7f;
  [SerializeField] private float maxSpawnDelayDayStart = 4f;

  public event EventHandler<OnCustomerServedEventArgs> OnCustomerServed;

  private bool isActive;

  private void Awake()
  {
    preparedDishList = preparedDishListSO.preparedDishList; //when we have multiple dishes, this will also be set in the Activate() method.
  }

  private void Start()
  {
    ServingStation.OnObjectServed += OnObjectServed;
  }

  public void Activate(List<ToppingSO> toppingList)
  {
    spawnTimer = UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelayDayStart);
    this.toppingList = toppingList;
    isActive = true;
  }

  public void Deactivate()
  {
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
      spawnTimer -= Time.deltaTime;

      if (spawnTimer <= 0f)
      {
        SpawnCustomer();
        spawnTimer = UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelay);
      }
    }
  }

  private void OrderLogic()
  {
    if (GetFrontCustomer() != null)
    {
      if (orderTimerInSeconds > 0)
      {
        orderTimerInSeconds -= Time.deltaTime;
        orderPanelController.UpdateVisuals(orderTimerInSeconds);
      }
      else
      {
        Debug.Log("customer leaves");
        ServedOrder servedOrder = new(null, 0, 0);
        OnCustomerServed?.Invoke(this, new OnCustomerServedEventArgs{servedOrder = servedOrder});
        CustomerLeaves(frontCustomer);
      }
    }
  }

  private void SpawnCustomer()
  {
    int queueIndex = customersInLine.Count;
    Vector3 targetPos = queuePoints[queueIndex].position;

    GameObject customerGameObject = Instantiate(customerPrefab, targetPos, Quaternion.identity);
    Customer newCustomer = customerGameObject.GetComponent<Customer>();

    newCustomer.Setup(GetRandomOrder());
    customersInLine.Add(newCustomer);

    if (queueIndex == 0) //if this customer spawns at the frontspot, update the Order Panel
    {
      frontCustomer = newCustomer;
      orderPanelController.ShowOrderPanel(frontCustomer.GetOrder());
      orderTimerInSeconds = frontCustomer.GetPatienceTime();
    }
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
    for (int i = 0; i < customersInLine.Count; i++)
    {
      CustomerLeaves(customersInLine[i]);
    }
  }

  public Order GetRandomOrder()
  {
    //int wantedToppingAmount = UnityEngine.Random.Range(0, 2); //will be added in the future so every order can have more than one topping
    int dishIndex = UnityEngine.Random.Range(0, preparedDishList.Count);
    PreparedDishSO wantedDish = preparedDishList[dishIndex];
    int toppingIndex = UnityEngine.Random.Range(0, toppingList.Count);
    ToppingSO wantedTopping = toppingList[toppingIndex];
    List<ToppingSO> wantedToppingList = new() {wantedTopping};

    Order wantedOrder = new(wantedDish, wantedToppingList);
    return wantedOrder;
  }

  private double CalculateMoneyPaid(Order order, double accuracy)
  {
    double totalPrice = order.GetTotalPrice();
    return Math.Round(totalPrice * accuracy, 2);
  }

  private void OnObjectServed(object sender, OnServeEventArgs e)
  {
    if (frontCustomer != null)
    {
      KitchenObject playerKitchenObject = e.servedObject;

      Order playerPlate = new(playerKitchenObject.GetPreparedDishSO(), playerKitchenObject.GetToppings());
      double accuracy = frontCustomer.ValidateOrder(playerPlate);
      double moneyPaid = CalculateMoneyPaid(playerPlate, accuracy);

      ServedOrder servedOrder = new(playerPlate, accuracy, moneyPaid);
      OnCustomerServed?.Invoke(this, new OnCustomerServedEventArgs
      {
        servedOrder = servedOrder
      });

      playerKitchenObject.DestroySelf();
      CustomerLeaves(frontCustomer);
    }
  }
}
