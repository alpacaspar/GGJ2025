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
        Customer customer = GetComponent<Customer>();
        foreach (RestaurantMenuItem menu in customer.orderList)
        {
            if ((interactor as PlayerInteractor).CurrentCarriedItem.MenuType == menu.MenuType)
            {
                (interactor as PlayerInteractor).CurrentCarriedItem = null;
                customer.orderList.Remove(menu);
                if (customer.orderList.Count <= 0)
                {
                    typingEffect.PopBubble();
                }
                break;
            }
        }
    }
}
