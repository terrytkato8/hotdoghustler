using FMOD.Studio;
using FMODUnity;
using System;
using UnityEngine;

public class CookStationAudio : FmodAudioBehaviour
{
  CookStation cookStation;

  [Header("Audio Setup")]
  [SerializeField] private EventReference Loop;
  [SerializeField] private EventReference Start;
  [SerializeField] private EventReference Ready;

  private EventInstance loopInstance;
  private EventInstance startInstance;
  private EventInstance readyInstance;

  private void Awake()
  {
    cookStation = GetComponentInParent<CookStation>();

    CookStation.OnStartedCooking.AddListener(CookStation_OnStartedCooking);
    CookStation.OnKitchenObjectRemoved.AddListener(CookStation_OnStoppedCooking);
    CookStation.OnCreatedPreparedDish.AddListener(CookStation_OnStoppedCooking);
    CookStation.OnFinishedCooking.AddListener(CookStation_OnFinishedCooking);

    // Initialize
    loopInstance = CreateManagedInstance(Loop);
    startInstance = CreateManagedInstance(Start);
    readyInstance = CreateManagedInstance(Ready);
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    CookStation.OnStartedCooking.RemoveListener(CookStation_OnStartedCooking);
    CookStation.OnKitchenObjectRemoved.RemoveListener(CookStation_OnStoppedCooking);
  }

  public void CookStation_OnStoppedCooking(object sender, EventArgs e)
  {
    if (cookStation == (CookStation)sender)
    {
      loopInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    }
  }

  public void CookStation_OnStartedCooking(object sender, EventArgs e)
  {
    if (cookStation == (CookStation)sender)
    {
      startInstance.start();
      loopInstance.start();
    }
  }

  public void CookStation_OnFinishedCooking(object sender, EventArgs e)
  {
    if (cookStation == (CookStation)sender)
    {
      readyInstance.start();
    }
  }
}
