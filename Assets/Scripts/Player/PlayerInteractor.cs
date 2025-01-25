using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerAnimator playerAnimator;

    private IInteractable currentInteractable;

    // Can be used to check on the customer if the player has the correct menu item.
    private RestaurantMenuItem currentCarriedItem;

    private void OnEnable()
    {
        FoodInteractable.OnInteracted += FoodInteractable_OnInteracted;
    }

    private void OnDisable()
    {
        FoodInteractable.OnInteracted -= FoodInteractable_OnInteracted;
    }

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
            playerAnimator.TriggerInteract();
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

    private void FoodInteractable_OnInteracted(RestaurantMenuItem item)
    {
        currentCarriedItem = item;
    }
}
