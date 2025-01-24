using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour, PlayerInputActionsAsset.IPlayerActions
{
    [SerializeField] private float moveSpeed = 3.0f;

    private CharacterController characterController;

    private PlayerInputActionsAsset inputActionAsset;
    private Vector2 moveInput;

    private void Awake()
    {
        inputActionAsset = new PlayerInputActionsAsset();
        inputActionAsset.Enable();

        characterController = GetComponent<CharacterController>();
    }

    public void OnEnable()
    {
        inputActionAsset.Player.AddCallbacks(this);
    }

    private void OnDisable()
    {
        inputActionAsset.Player.RemoveCallbacks(this);
    }

    private void Update()
    {
        var direction = new Vector3(moveInput.x, 0, moveInput.y);
        characterController.Move(direction * (moveSpeed * Time.deltaTime));
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
    }
}
