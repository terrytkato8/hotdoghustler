using System;
using UnityEngine;

public class AnimationDurationEventArgs : EventArgs
{
  public Action<float> SetDurationCallback;
}
