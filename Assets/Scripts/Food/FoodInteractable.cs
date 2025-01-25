using UnityEngine;

public class FoodInteractable : MonoBehaviour, IInteractable
{
    public static System.Action<RestaurantMenuItem> OnInteracted;

    [SerializeField] private RestaurantMenuItem[] menuItems;
    [SerializeField] private Vector2 preparationTimeRange;

    private SpriteRenderer spriteRenderer;

    private float currentTime;
    private bool isPreparing = true;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        StartPreparation();
    }

    private void Update()
    {
        if (!isPreparing)
            return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            StopPreparation();
        }
    }

    // Already called from the player interactor. So no need to implement ourselves.
    public bool CanInteract(IInteractable interactor)
    {
        return !isPreparing;
    }

    public void Interact(IInteractable interactor)
    {
        StartPreparation();
    }

    private void StartPreparation()
    {
        currentTime = Random.Range(preparationTimeRange.x, preparationTimeRange.y);
        spriteRenderer.gameObject.SetActive(false);
     
        isPreparing = true;
    }

    private void StopPreparation()
    {
        var preparedMenuItem = menuItems[Random.Range(0, menuItems.Length)];
        isPreparing = false;

        spriteRenderer.gameObject.SetActive(true);
        spriteRenderer.sprite = preparedMenuItem.ItemSprite;

        OnInteracted?.Invoke(preparedMenuItem);
    }
}
