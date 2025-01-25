using System;

public class SmokeInteractable : InteractableBehaviour
{
    public static event Action OnSmokeBreakStarted;

    public override bool CanInteract(InteractableBehaviour interactor)
    {
        return true;
    }

    public override void Interact(InteractableBehaviour interactor)
    {
        OnSmokeBreakStarted?.Invoke();
    }
}
