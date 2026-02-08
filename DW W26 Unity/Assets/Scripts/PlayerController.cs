using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [field: SerializeField] public int PlayerNumber { get; private set; }
    [field: SerializeField] public Rigidbody2D Rigidbody2D { get; private set; }
    [field: SerializeField] public float MoveSpeed { get; private set; } = 5f;
    [field: SerializeField] public float JumpForce { get; private set; } = 5f;

    private PlayerInput PlayerInput;
    private InputAction InputActionMove;
    private InputAction InputActionJump;

    public void InitializePlayer(PlayerInput playerInput)
    {
        // Record our player input (ie controller).
        PlayerInput = playerInput;
        // Find the references to the "Move" and "Jump" actions inside the player input's action map
        // Here I specify "Player/" but it in not required if assigning the action map in PlayerInput inspector.
        InputActionMove = playerInput.actions.FindAction($"Player/Move");
        InputActionJump = playerInput.actions.FindAction($"Player/Jump");
    }

    // Assign player number on spawn
    public void AssignPlayerNumber(int playerNumber)
    {
        this.PlayerNumber = playerNumber;
    }

    public void Update()
    {
        //var InputActionMove = InputSystem.actions.FindAction($"Player/Move");
        //var x = Gamepad.all[]
    }


    // This function gets called because the action map "Player" is selected in the PlayerInput script
    // and because the action "Move" is defined in the Player action map.
    public void OnMove(InputValue value)
    {
        // Read the "Move" action value, which is a 2D vector
        Vector2 moveValue = value.Get<Vector2>();
        // Here we're only using the X axis to move.
        float moveForce = moveValue.x * MoveSpeed;
        // Apply fraction of force each frame
        Rigidbody2D.AddForceX(moveForce, ForceMode2D.Force);


    }

    public void OnJump(InputValue button)
    {
        // Get button pressed
        //bool isPressed = button.performed;
        //bool isReleased = button.canca;
        //bool isDown = button.started;

        bool isDown = InputActionJump.IsPressed();
        bool isUp = !InputActionJump.IsPressed();
        bool isPressed = InputActionJump.WasPressedThisFrame();
        bool isReleased = InputActionJump.WasReleasedThisFrame();

        if (isPressed)
        {
            // Apply all force immediately
            Rigidbody2D.AddForceY(JumpForce, ForceMode2D.Impulse);
        }
    }

    void FixedUpdate()
    {
        // Read the "Move" action value, which is a 2D vector
        Vector2 moveValue = InputActionMove.ReadValue<Vector2>();
        // Here we're only using the X axis to move.
        float moveForce = moveValue.x * MoveSpeed;
        // Apply fraction of force each frame
        Rigidbody2D.AddForceX(moveForce, ForceMode2D.Force);

        // Read the "Jump" action state, which is a boolean value
        if (InputActionJump.WasPressedThisFrame())
        {
            // Apply all force immediately
            Rigidbody2D.AddForceY(JumpForce, ForceMode2D.Impulse);
        }
    }

    private void OnValidate()
    {
        Reset();
    }

    private void Reset()
    {
        if (Rigidbody2D == null)
            Rigidbody2D = GetComponent<Rigidbody2D>();
    }
}
