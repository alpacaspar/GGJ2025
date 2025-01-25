using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : MonoBehaviour, IInteractable
{
    [SerializeField] private PlayerAnimator playerAnimator;

    private List<IInteractable> interactables = new();

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
            interactables.Add(interactable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            interactables.Remove(interactable);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        // Only interact when releasing the interact button.
        if (!context.action.WasReleasedThisFrame())
            return;

        if (!interactables.Any())
            return;
        
        IInteractable currentInteractable = interactables.First();
        if (interactables != null && currentInteractable.CanInteract(this))
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
