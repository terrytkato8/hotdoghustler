using Mono.Cecil.Cil;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

public abstract class StaticBaseGameEvent
{
  protected static readonly List<StaticBaseGameEvent> AllStaticEvents = new ();

  public StaticBaseGameEvent()
  {
    AllStaticEvents.Add(this);
  }

  public abstract void RemoveAllListeners();

  public static void ResetAllStaticEvents()
  {
    foreach (var evt in AllStaticEvents)
    {
      evt.RemoveAllListeners();
    }
  }
}

public class StaticGameEvent : StaticBaseGameEvent
{
  private Action<object, EventArgs> _action;

  public void AddListener(Action<object, EventArgs> listener)
  {
    _action -= listener;
    _action += listener;
  }

  public void RemoveListener(Action<object, EventArgs> listener)
  {
    _action -= listener;
  }

  public void Invoke(object sender, EventArgs e = null)
  {
    _action?.Invoke(sender, e ?? EventArgs.Empty);
  }

  public override void RemoveAllListeners()
  {
    _action = null;
  }
}

public class StaticGameEvent<T> : StaticBaseGameEvent where T : EventArgs
{

  private Action<object, T> _action;

  public void AddListener(Action<object, T> listener)
  {
    _action -= listener;
    _action += listener;
  }

  public void RemoveListener(Action<object, T> listener)
  {
    _action -= listener;
  }

  public void Invoke(object sender, T e)
  {
    _action?.Invoke(sender, e);
  }

  public override void RemoveAllListeners()
  {
    _action = null;
  }
}