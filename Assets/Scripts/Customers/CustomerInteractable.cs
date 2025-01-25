using UnityEngine;

public class CustomerInteractable : MonoBehaviour, IInteractable
{
    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        Debug.Log("Can I get uhh burger.");
    }
}
