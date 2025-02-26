using System;
using System.Collections;
using UnityEngine;

public class SmokeInteractable : InteractableBehaviour
{
    public static event Action OnSmokeBreakStarted;
    public static event Action OnSmokeBreakFinished;

    public override bool CanInteract(InteractableBehaviour interactor)
    {
        return (interactor.transform.position - transform.position).sqrMagnitude < 2 * 2;
    }

    public override void Interact(InteractableBehaviour interactor)
    {
        StartCoroutine(Co_SmokeBreak());
    }

    private IEnumerator Co_SmokeBreak()
    {
        OnSmokeBreakStarted?.Invoke();

        yield return new WaitForSeconds(20f);

        OnSmokeBreakFinished?.Invoke();
    }
}
