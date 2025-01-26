using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractor : InteractableBehaviour
{
    [SerializeField] private PlayerAnimator playerAnimator;

    private List<InteractableBehaviour> interactables = new();

    // Can be used to check on the customer if the player has the correct menu item.
    private RestaurantMenuItem currentCarriedItem;
    public RestaurantMenuItem CurrentcarriedItem
    {
        get => currentCarriedItem;

        set
        {
            if (value == null)
            {
                foodObject.SetActive(false);
            }
            currentCarriedItem = CurrentcarriedItem;
        }
    }

    [SerializeField] private GameObject foodObject;

    private void OnEnable()
    {
        FoodInteractable.OnInteracted += FoodInteractable_OnInteracted;
        SmokeInteractable.OnSmokeBreakFinished += SmokeInteractable_OnSmokeBreakFinished;
    }

    private void SmokeInteractable_OnSmokeBreakFinished()
    {
        var uhh = interactables.Where((i) => i is SmokeInteractable).ToList();
        for (int i = uhh.Count() - 1; i >= 0; i--)
        {
            interactables.Remove(uhh[i]);
        }
    }

    private void OnDisable()
    {
        FoodInteractable.OnInteracted -= FoodInteractable_OnInteracted;
        SmokeInteractable.OnSmokeBreakFinished += SmokeInteractable_OnSmokeBreakFinished;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<InteractableBehaviour>(out var interactable))
        {
            interactables.Add(interactable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<InteractableBehaviour>(out var interactable))
        {
            interactables.Remove(interactable);
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        // Only interact when releasing the interact button.
        if (!context.action.WasReleasedThisFrame())
            return;

        interactables.RemoveAll((i) => (i.transform.position - transform.position).sqrMagnitude > 2 * 2);
        interactables.RemoveAll((i) => i == null || !i.gameObject.activeSelf);
        if (!interactables.Any())
            return;

        InteractableBehaviour currentInteractable = interactables.First();
        if (interactables != null && currentInteractable.CanInteract(this))
        {
            currentInteractable.Interact(this);
            playerAnimator.TriggerInteract();
        }
    }

    public override bool CanInteract(InteractableBehaviour interactor)
    {
        return false;
    }

    public override void Interact(InteractableBehaviour interactor)
    {
        // Don't do anything.
    }

    private void FoodInteractable_OnInteracted(RestaurantMenuItem item)
    {
        currentCarriedItem = item;
        foodObject.GetComponent<SpriteRenderer>().sprite = currentCarriedItem.ItemSprite;
        foodObject.transform.localScale = Vector3.one * 1.5f;
        foodObject.SetActive(true);
    }
}
