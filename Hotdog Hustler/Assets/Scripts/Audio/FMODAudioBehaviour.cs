using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

public abstract class FmodAudioBehaviour : MonoBehaviour
{
  private readonly List<EventInstance> _managedInstances = new();

  protected virtual void OnDestroy()
  {
    foreach (EventInstance instance in _managedInstances)
    {
      if (instance.isValid())
      {
        instance.release();
      }
    }
    _managedInstances.Clear();
  }

  protected EventInstance CreateManagedInstance(EventReference eventReference)
  {
    if (eventReference.IsNull)
    {
      return new EventInstance();
    }

    EventInstance instance = RuntimeManager.CreateInstance(eventReference);

    RuntimeManager.AttachInstanceToGameObject(instance, gameObject, (Rigidbody2D)null);

    _managedInstances.Add(instance);
    return instance;
  }

  protected float GetEventDuration(EventReference eventReference)
  {
    EventDescription eventDescription = RuntimeManager.GetEventDescription(eventReference);

    eventDescription.getLength(out int lengthInMilliseconds);

    return lengthInMilliseconds / 1000f;
  }
}