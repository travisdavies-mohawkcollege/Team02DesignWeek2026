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
    [field: SerializeField] public GroundCheck groundCheckObject;


    //Trapper Variables
    [field: SerializeField] public bool isTrapper { get; private set; } = false;
    [field: SerializeField] public Sprite sprTrapperHand { get; private set; }
    [field: SerializeField] public int interactionRange { get; private set; } = 25;
    public bool trapperInteract;
    [field: SerializeField] public LayerMask trapperInteractableMask { get; private set; }


    //Various Vexing Variables
    public bool DoJump { get; private set; }
    public bool canControl { get; private set; }


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

    //Temp
    public PitTrap pitTrapTest;

    //Timers
    public float buttonCooldown = 20f;
    public float buttonTimer = 20f;
    public bool buttonOnCooldown = false;


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

    public void AssignTrapperSprite()
    {
        SpriteRenderer.sprite = sprTrapperHand;

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
        this.isTrapper = isTrapper;
        if (Rigidbody2D == null)
        {
            Debug.Log($"{name}'s {nameof(PlayerController)}.{nameof(Rigidbody2D)} is null.");
            return;
        }
        Rigidbody2D.linearDamping = 2;
        Rigidbody2D.freezeRotation = true;
        MoveSpeed = 15f;
    }

    // Runs each frame
    public void Update()
    {
        switch (isTrapper)
        {
            case true:
                if (InputActionJump.WasPressedThisFrame())
                {
                    trapperInteract = true;
                }
                return;
            case false:
                // Read the "Jump" action state, which is a boolean value
                if (InputActionJump.WasPressedThisFrame() && !jumping)
                {
                    // Buffer input becuase I'm controlling the Rigidbody through FixedUpdate
                    // and checking there we can miss inputs.
                    DoJump = true;
                    jumping = true;
                }
                return;
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

        switch (isTrapper)
        {
            case true:
                TrapperInteract();
                TrapperMove();
                TrapperTimers();
                return;
            case false:
                RunnerJump();
                RunnerMove();
                return;
        }  
    }

    private void RunnerJump()
    {
        // JUMP - review Update()
        if (DoJump)
        {
            if (jumping)
            {
                grounded = false;
                transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
                if (transform.localScale.x >= maxSize.x)
                {
                    jumping = false;
                    falling = true;
                }
            }
            else if (falling)
            {
                transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f);
                if (transform.localScale.x <= minSize.x)
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

    private void RunnerMove()
    {
        // MOVE
        // Read the "Move" action value, which is a 2D vector
        Vector2 moveValue = InputActionMove.ReadValue<Vector2>();
        // Move on both axis
        float moveForceX = moveValue.x * MoveSpeed;
        float moveForceY = moveValue.y * MoveSpeed;
        // Apply fraction of force each frame
        Rigidbody2D.AddForceX(moveForceX, ForceMode2D.Force);
        Rigidbody2D.AddForceY(moveForceY, ForceMode2D.Force);
    }

    private void TrapperMove()
    {
        // MOVE
        // Read the "Move" action value, which is a 2D vector
        Vector2 moveValue = InputActionMove.ReadValue<Vector2>();
        // Move on both axis
        float moveForceX = moveValue.x * MoveSpeed;
        float moveForceY = moveValue.y * MoveSpeed;
        // Apply fraction of force each frame
        Rigidbody2D.AddForceX(moveForceX, ForceMode2D.Force);
        Rigidbody2D.AddForceY(moveForceY, ForceMode2D.Force);
    }

    private void TrapperInteract()
    {
        
        if (trapperInteract)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, interactionRange, trapperInteractableMask);
            Debug.Log("Interacted");
            if (!hit)
            {
                trapperInteract = false;
                Debug.DrawRay(this.transform.position, -Vector3.up * interactionRange, Color.red);
                Debug.Log("hit nothing");
            }
            else if (hit.collider.gameObject.CompareTag("Button"))
            {
                if(buttonOnCooldown)
                {
                    Debug.Log("Button on CD");
                    trapperInteract = false;
                }
                else
                {
                    Debug.Log("Interacted with Button");
                    Animator buttonAnim = hit.collider.GetComponent<Animator>();
                    this.SpriteRenderer.enabled = false;
                    buttonAnim.SetBool("pressingButton", true);
                    trapperInteract = false;
                    buttonTimer = 10f;
                    //buttonOnCooldown = true;
                }
                    
            }
            else
            {
                trapperInteract = false;
                Debug.Log(hit.collider.gameObject);
            }
        }
        
    }

    private void TrapperTimers()
    {
        ButtonTimer();
    }

    private void ButtonTimer()
    {
        
        if (buttonTimer <= 0)
        {
            buttonTimer = buttonCooldown;
            buttonOnCooldown = false;
        }
        if (buttonOnCooldown)
        {
            buttonTimer -= Time.deltaTime;
            Debug.Log(buttonTimer);
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log("Collided with " + collider.gameObject);
        Debug.Log(grounded);
        if (collider.gameObject.CompareTag("PitTrap"))
        {
            if (grounded)
            {
                //Take Damage
                RunnerDie();
            }
        }
    }

    public void RunnerDie()
    {
        SpriteRenderer.color = Color.white;
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
