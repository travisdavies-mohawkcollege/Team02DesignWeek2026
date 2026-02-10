using System.Collections;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [field: SerializeField] public int PlayerNumber { get; private set; }
    [field: SerializeField] public Color PlayerColor { get; private set; }
    [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }
    [field: SerializeField] public Rigidbody2D Rigidbody2D { get; private set; }
    [field: SerializeField] public float MoveSpeed { get; private set; } = 10f;
    [field: SerializeField] public float JumpForce { get; private set; } = 5f;
    [field: SerializeField] public bool isTrapper { get; private set; } = false;

    public bool DoJump { get; private set; }

    // Player input information
    private PlayerInput PlayerInput;
    private InputAction InputActionMove;
    private InputAction InputActionJump;

    //Jump Test
    public bool jumping { get; private set; }
    public bool falling { get; private set; }
    public bool grounded { get; private set; } = true;
    public Vector3 maxSize = new Vector3(2f, 2f, 2f);
    public Vector3 minSize = new Vector3(0.75f, 0.75f, 0.75f);


    // Assign color value on spawn from main spawner
    public void AssignColor(Color color)
    {
        // record color
        PlayerColor = color;

        // Assign to sprite renderer
        if (SpriteRenderer == null)
            Debug.Log($"Failed to set color to {name} {nameof(PlayerController)}.");
        else
            SpriteRenderer.color = color;
    }

    // Set up player input
    public void AssignPlayerInputDevice(PlayerInput playerInput)
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

    public void AssignTrapperRole(bool isTrapper)
    {

    }

    // Runs each frame
    public void Update()
    {
        switch (isTrapper)
        {
            case true:
                return;
        }
        
        // Read the "Jump" action state, which is a boolean value
        if (InputActionJump.WasPressedThisFrame())
        {
            // Buffer input becuase I'm controlling the Rigidbody through FixedUpdate
            // and checking there we can miss inputs.
            DoJump = true;
            jumping = true;
        }
    }

    // Runs each phsyics update
    void FixedUpdate()
    {
        if (Rigidbody2D == null)
        {
            Debug.Log($"{name}'s {nameof(PlayerController)}.{nameof(Rigidbody2D)} is null.");
            return;
        }

        // MOVE
        // Read the "Move" action value, which is a 2D vector
        Vector2 moveValue = InputActionMove.ReadValue<Vector2>();
        // Here we're only using the X axis to move.
        float moveForceX = moveValue.x * MoveSpeed;
        float moveForceY = moveValue.y * MoveSpeed;
        // Apply fraction of force each frame
        Rigidbody2D.AddForceX(moveForceX, ForceMode2D.Force);
        Rigidbody2D.AddForceY(moveForceY, ForceMode2D.Force);

        // JUMP - review Update()
        if (DoJump)
        {
            if (jumping)
            {
                transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
                if(transform.localScale.x >= maxSize.x)
                {
                    jumping = false;
                    falling = true;
                    grounded = false;
                }
            }
            else if (falling)
            {
                transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                if(transform.localScale.x <= minSize.x)
                {
                    falling = false;
                }
            }
            else
            {
                DoJump = false;
                grounded = true;
            }
                
            
        }
    }

    // OnValidate runs after any change in the inspector for this script.
    private void OnValidate()
    {
        Reset();
    }

    // Reset runs when a script is created and when a script is reset from the inspector.
    private void Reset()
    {
        // Get if null
        if (Rigidbody2D == null)
            Rigidbody2D = GetComponent<Rigidbody2D>();
        if (SpriteRenderer == null)
            SpriteRenderer = GetComponent<SpriteRenderer>();
    }
}
