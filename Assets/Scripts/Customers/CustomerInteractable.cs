using UnityEngine;

public class CustomerInteractable : MonoBehaviour, IInteractable
{
    public bool CanInteract(IInteractable interactor)
    {
        return true;
    }

    public void Interact(IInteractable interactor)
    {
        Debug.Log("Can I get uhh burger.");
    }
}
