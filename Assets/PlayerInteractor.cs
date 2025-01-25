using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour
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

        if (currentInteractable != null && currentInteractable.CanInteract())
        {
            currentInteractable.Interact();
        }
    }
}
