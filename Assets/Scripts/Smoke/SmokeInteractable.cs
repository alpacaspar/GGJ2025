using System;
using UnityEngine;

public class SmokeInteractable : MonoBehaviour, IInteractable
{
    public static event Action OnSmokeBreakStarted;

    public bool CanInteract(IInteractable interactor)
    {
        return true;
    }

    public void Interact(IInteractable interactor)
    {
        OnSmokeBreakStarted?.Invoke();
    }
}
