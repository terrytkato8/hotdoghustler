using UnityEngine;

[CreateAssetMenu(fileName = "NewDifficultyProfile", menuName = "ScriptableObjects/DifficultyProfile")]
public class DifficultyProfileSO : ScriptableObject
{
  [Header("Curves")]
  [Tooltip("Y: Seconds between customers")]
  public AnimationCurve spawnDelayCurve;

  [Tooltip("Y: Multiplier for patience (1.0 = Normal)")]
  public AnimationCurve patienceCurve;

  [Header("Base Settings")]
  public float baseDayDuration = 120f;

  public DailyDifficulty GetDailyDifficulty(int day)
  {
    float spawnVal = spawnDelayCurve.Evaluate(day);

    return new DailyDifficulty
    {
      MaxSpawnDelay = spawnVal,
      MinSpawnDelay = Mathf.Max(1f, spawnVal - 3f),
      PatienceMultiplier = patienceCurve.Evaluate(day),
      DayDuration = baseDayDuration,
    };
  }
}