using UnityEngine;

public class FoodInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private MenuType menuType;
    [SerializeField] private Sprite[] menuItems;

    [SerializeField] private Vector2 preparationTimeRange;

    private SpriteRenderer spriteRenderer;

    private float currentTime;
    private bool isPreparing = true;

    private void Awake()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

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
    public bool CanInteract()
    {
        return !isPreparing;
    }

    public void Interact()
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
        spriteRenderer.gameObject.SetActive(true);
        spriteRenderer.sprite = menuItems[Random.Range(0, menuItems.Length)];

        isPreparing = false;
    }
}
