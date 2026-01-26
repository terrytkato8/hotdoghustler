using FMOD.Studio;
using FMODUnity;
using System;
using UnityEngine;

public class CustomerAudio : FmodAudioBehaviour
{
  Customer customer;

  [Header("Audio Setup")]
  [SerializeField] private EventReference CustomerApproval;
  [SerializeField] private EventReference CustomerDisapproval;

  private EventInstance customerApprovalInstance;
  private EventInstance customerDisapprovalInstance;

  private void Awake()
  {
    customer = GetComponentInParent<Customer>();

    customer.OnReaction += Customer_OnReaction;

    // Initialize
    customerApprovalInstance = CreateManagedInstance(CustomerApproval);
    customerDisapprovalInstance = CreateManagedInstance(CustomerDisapproval);
  }

  protected override void OnDestroy()
  {
    base.OnDestroy();
    customer.OnReaction -= Customer_OnReaction;
  }

  private void Customer_OnReaction(object sender, OnReactionEventArgs e)
  {
    float duration = 1;
    if (e.isHappy)
    {
      customerApprovalInstance.start();
      duration = GetEventDuration(CustomerApproval);
    }
    else
    {
      customerDisapprovalInstance.start();
      duration = GetEventDuration(CustomerDisapproval);
    }

    e.SetDurationCallback(duration);
  }
}