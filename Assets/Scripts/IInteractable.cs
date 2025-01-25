public interface IInteractable
{
    public bool CanInteract(IInteractable interactor);

    public void Interact(IInteractable interactor);
}
