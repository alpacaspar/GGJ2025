using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material closedMaterial;
    [SerializeField] private Material openMaterial;

    private void OnEnable()
    {
        GameManager.OnStateChanged += GameManager_OnStateChanged;
        SmokeInteractable.OnSmokeBreakStarted += SmokeInteractable_OnSmokeBreakStarted;
        SmokeInteractable.OnSmokeBreakFinished += SmokeInteractable_OnSmokeBreakStarted;
    }

    private void OnDisable()
    {
        GameManager.OnStateChanged -= GameManager_OnStateChanged;
        SmokeInteractable.OnSmokeBreakStarted -= SmokeInteractable_OnSmokeBreakStarted;
        SmokeInteractable.OnSmokeBreakFinished += SmokeInteractable_OnSmokeBreakStarted;
    }

    private void GameManager_OnStateChanged(CurrentState state)
    {
        if (state is CurrentState.InGame)
        {
            StartCoroutine(Co_OpenDoor());
        }
    }

    private void SmokeInteractable_OnSmokeBreakStarted()
    {
        StartCoroutine(Co_OpenDoor());
    }

    private IEnumerator Co_OpenDoor()
    {
        meshRenderer.sharedMaterial = openMaterial;

        yield return new WaitForSeconds(0.5f);

        meshRenderer.sharedMaterial = closedMaterial;
    }

}
