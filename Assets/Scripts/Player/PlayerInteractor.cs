using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour, IInteractable
{
    private IInteractable currentInteractable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            currentInteractable = interactable;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IInteractable>(out var _))
        {
            currentInteractable = null;
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        // Only interact when releasing the interact button.
        if (!context.canceled)
            return;

        if (currentInteractable != null && currentInteractable.CanInteract(this))
        {
            currentInteractable.Interact(this);
        }
    }

    public bool CanInteract(IInteractable interactor)
    {
        return false;
    }

    public void Interact(IInteractable interactor)
    {
        // Don't do anything.
    }
}
