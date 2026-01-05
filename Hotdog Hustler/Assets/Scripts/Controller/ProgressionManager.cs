using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ProgressionManager : MonoBehaviour
{
  [SerializeField] private ToppingListSO masterToppingListSO; // All possible items
  [SerializeField] private ToppingListSO startingToppingListSO; // Items owned at Day 1.

  private List<ToppingSO> unlockedToppingList = new();
  private double currentBalance; //this and the unlockedToppingList will be loaded from a json file when we have a save system.
  private int day;

  private const string SAVE_FILE_NAME = "hotdog_save.json";

  public void Init()
  {
    LoadGame();
  }

  public void SaveGame()
  {
    SaveData data = new()
    {
      money = currentBalance,
      day = day,

      unlockedToppingNames = new List<string>()
    };
    foreach (var topping in unlockedToppingList)
    {
      data.unlockedToppingNames.Add(topping.toppingName);
    }

    string json = JsonUtility.ToJson(data, true);
    string path = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);

    File.WriteAllText(path, json);
    Debug.Log($"Game Saved to: {path}");
  }

  public void LoadGame()
  {
    string path = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);

    if (File.Exists(path))
    {
      string json = File.ReadAllText(path);
      SaveData data = JsonUtility.FromJson<SaveData>(json);

      currentBalance = data.money;
      day = data.day;

      unlockedToppingList.Clear();

      foreach (string loadedName in data.unlockedToppingNames)
      {
        ToppingSO match = masterToppingListSO.toppingList.Find(t => t.toppingName == loadedName);

        if (match != null)
        {
          unlockedToppingList.Add(match);
        }
        else
        {
          Debug.LogWarning($"Could not find topping with name: {loadedName}. Has the SO changed?");
        }
      }

      Debug.Log("Game Loaded!");
    }
    else
    {
      Debug.Log("No Save File found. Starting New Game.");
      StartNewGame();
    }
  }

  public void StartNewGame()
  {
    currentBalance = 0;
    day = 1;
    unlockedToppingList = new (startingToppingListSO.toppingList);
    SaveGame();
  }

  [ContextMenu("Delete Save File")]
  public void DeleteSaveFile()
  {
    string path = Path.Combine(Application.persistentDataPath, SAVE_FILE_NAME);
    if (File.Exists(path))
    {
      File.Delete(path);
      Debug.Log("Save Deleted.");
    }
  }

  public List<ToppingSO> GetUnlockedToppings()
  {
    return unlockedToppingList;
  }

  public List<ToppingSO> GetLockedToppings()
  {
    List<ToppingSO> locked = new();

    foreach (var item in masterToppingListSO.toppingList)
    {
      if (!unlockedToppingList.Contains(item))
      {
        locked.Add(item);
      }
    }
    return locked;
  }

  public void UnlockTopping(ToppingSO topping)
  {
    if (!unlockedToppingList.Contains(topping))
      unlockedToppingList.Add(topping);
    else
      Debug.LogError("topping is already unlocked");
  }

  public void AddMoney(double amount)
  {
    currentBalance += amount;
  }

  public double GetCurrentBalance() { return currentBalance; }
  public void SetCurrentBalance(double currentBalance) { this.currentBalance = currentBalance; }

  public void IncreaseDay()
  {
    day++;
  }

  public int GetDay() { return day; }
}
