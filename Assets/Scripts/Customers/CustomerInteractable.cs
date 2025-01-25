using System;
using UnityEngine;

public class CustomerInteractable : InteractableBehaviour
{
    public override bool CanInteract(InteractableBehaviour interactor)
    {
        return true;
    }

    public override void Interact(InteractableBehaviour interactor)
    {
        Debug.Log("Can I get uhh burger.");
    }
}
