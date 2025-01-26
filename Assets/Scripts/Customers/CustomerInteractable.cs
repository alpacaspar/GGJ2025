using System.Linq;
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
        if ((interactor as PlayerInteractor).CurrentcarriedItem == null)
        {
            typingEffect.PopBubble();
            return;
        }

        Customer customer = GetComponent<Customer>();

        foreach (RestaurantMenuItem menu in customer.orderList)
        {
            if ((interactor as PlayerInteractor).CurrentcarriedItem.MenuType == menu.MenuType)
            {
                (interactor as PlayerInteractor).CurrentcarriedItem = null;
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
