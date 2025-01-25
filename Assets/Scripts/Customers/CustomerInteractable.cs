using System;
using UnityEngine;

public class CustomerInteractable : MonoBehaviour, IInteractable
{
    public static event Action OnSmokeBreakStarted;

    public bool CanInteract(IInteractable interactor)
    {
        return true;
    }

    public void Interact(IInteractable interactor)
    {
        Debug.Log("Can I get uhh burger.");

        OnSmokeBreakStarted?.Invoke();
    }
}
