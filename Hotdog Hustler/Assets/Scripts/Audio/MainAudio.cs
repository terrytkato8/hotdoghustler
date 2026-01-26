using FMOD.Studio;
using FMODUnity;
using System;
using UnityEngine;

public class MainAudio : FmodAudioBehaviour
{
  [Header("Audio Setup")]
  [SerializeField] private EventReference StreetAmbience;
  [SerializeField] private EventReference Level1Music;
  [SerializeField] private EventReference MainMenuMusic;
  [SerializeField] private EventReference ItemPickup;
  [SerializeField] private EventReference ItemDrop;
  [SerializeField] private EventReference Purchase;

  private EventInstance streetAmbienceInstance;
  private EventInstance level1MusicInstance;
  private EventInstance mainMenuMusicInstance;
  private EventInstance itemPickupInstance;
  private EventInstance itemDropInstance;
  private EventInstance purchaseInstance;

  private void Awake()
  {
    CookStation.OnStartedCooking.AddListener(OnItemDrop);
    CookStation.OnKitchenObjectRemoved.AddListener(OnItemPickup);

    // Initialize
    streetAmbienceInstance = CreateManagedInstance(StreetAmbience);
    level1MusicInstance = CreateManagedInstance(Level1Music);
    mainMenuMusicInstance = CreateManagedInstance(MainMenuMusic);
    itemPickupInstance = CreateManagedInstance(ItemPickup);
    itemDropInstance = CreateManagedInstance(ItemDrop);
    purchaseInstance = CreateManagedInstance(Purchase);
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    CookStation.OnStartedCooking.RemoveListener(OnItemDrop);
    CookStation.OnKitchenObjectRemoved.RemoveListener(OnItemPickup);
  }

  public void StartDay()
  {
    streetAmbienceInstance.start();
    level1MusicInstance.start();
  }

  public void EndDay()
  {
    streetAmbienceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
    level1MusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
  }

  public void PlayMainMenuMusic()
  {
    mainMenuMusicInstance.start();
  }

  public void StopMainMenuMusic()
  {
    mainMenuMusicInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
  }

  public void PurchaseItem()
  {
    purchaseInstance.start();
  }

  public void OnItemPickup(object sender, EventArgs e)
  {
    itemPickupInstance.start();
  }

  public void OnItemDrop(object sender, EventArgs e)
  {
    itemDropInstance.start();
  }
}
