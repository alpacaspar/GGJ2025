using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private PlayerAnimator playerAnimator;

    private CharacterController characterController;
    private PlayerInput input;

    private Vector2 moveInput;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
        input = GetComponent<PlayerInput>();

        SmokeInteractable.OnSmokeBreakStarted += SmokeInteractable_OnSmokeBreakStarted;
        SmokeInteractable.OnSmokeBreakFinished += SmokeInteractable_OnSmokeBreakFinished;
    }

    private void OnDestroy()
    {
        SmokeInteractable.OnSmokeBreakStarted -= SmokeInteractable_OnSmokeBreakStarted;
        SmokeInteractable.OnSmokeBreakFinished -= SmokeInteractable_OnSmokeBreakFinished;
    }

    private void OnEnable()
    {
        GameManager.OnStateChanged += GameOver_OnStateChanged;
    }


    private void OnDisable()
    {
        GameManager.OnStateChanged -= GameOver_OnStateChanged;
    }

    private void SmokeInteractable_OnSmokeBreakStarted()
    {
        input.enabled = false;
    }

    private void SmokeInteractable_OnSmokeBreakFinished()
    {
        input.enabled = true;
    }

    private void Update()
    {
        var direction = new Vector3(moveInput.x, 0, moveInput.y);
        characterController.Move(direction * (moveSpeed * Time.deltaTime));
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        playerAnimator.SetMoveDirection(moveInput);
    }

    private void GameOver_OnStateChanged(CurrentState state)
    {
        switch (state)
        {
            case CurrentState.Menu:
            case CurrentState.GameOver:
                input.enabled = false;
                break;
            case CurrentState.InGame:
                input.enabled = true;
                break;
        }
    }
}
