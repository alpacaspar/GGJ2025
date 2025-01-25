using UnityEngine;

public abstract class InteractableBehaviour : MonoBehaviour
{
    public abstract bool CanInteract(InteractableBehaviour interactor);

    public abstract void Interact(InteractableBehaviour interactor);
}
