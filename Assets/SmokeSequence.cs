using UnityEngine;

public class SmokeSequence : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        SmokeInteractable.OnSmokeBreakStarted += SmokeInteractable_OnSmokeBreakStarted;
    }

    private void OnDisable()
    {
        SmokeInteractable.OnSmokeBreakStarted -= SmokeInteractable_OnSmokeBreakStarted;
    }

    private void SmokeInteractable_OnSmokeBreakStarted()
    {
        animator.SetTrigger("StartSmokeBreak");
    }
}
