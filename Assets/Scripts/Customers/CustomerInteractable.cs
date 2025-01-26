using UnityEngine;

public class CustomerInteractable : InteractableBehaviour
{
    [SerializeField] private TypingEffect typingEffect;

    public override bool CanInteract(InteractableBehaviour interactor)
    {
        return true;
    }

    public override void Interact(InteractableBehaviour interactor)
    {
        typingEffect.PopBubble();
    }
}
