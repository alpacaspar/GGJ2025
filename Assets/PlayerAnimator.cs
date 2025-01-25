using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetMoveDirection(Vector2 direction)
    {
        animator.SetFloat("MoveSpeed", direction.magnitude);

        if (direction.x != 0)
        {
            spriteRenderer.flipX = direction.x > 0;
        }
    }

    public void TriggerInteract()
    {
        animator.SetTrigger("OnInteract");
    }

    public void TriggerPickup()
    {
        animator.SetTrigger("OnPickup");
    }
}
