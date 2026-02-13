using JetBrains.Annotations;
using System.Collections;
//using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [field: SerializeField] public int PlayerNumber { get; private set; }
    [field: SerializeField] public Color PlayerColor { get; private set; }
    [field: SerializeField] public SpriteRenderer SpriteRenderer { get; private set; }
    [field: SerializeField] public Rigidbody2D Rigidbody2D { get; private set; }
    [field: SerializeField] public float MoveSpeed { get; private set; } = 10f;
    [field: SerializeField] public float JumpForce { get; private set; } = 5f;


    //Trapper Variables
    [field: SerializeField] public bool isTrapper { get; private set; } = false;
    [field: SerializeField] public Sprite sprTrapperHand { get; private set; }
    [field: SerializeField] public int interactionRange { get; private set; } = 25;
    public bool trapperInteract;
    [field: SerializeField] public LayerMask trapperInteractableMask { get; private set; }


    //Various Vexing Variables
    public bool DoJump { get; private set; }
    public bool canControl = true;
    public GameObject player2Jail, player3Jail, player4Jail, player5Jail, player6Jail;
    public GameManager gameManager;

    public PlayPointData[] player2Points, player3Points, player4Points, player5Points, player6Points;
    public int score;

    // Player input information
    private PlayerInput PlayerInput;
    private InputAction InputActionMove;
    private InputAction InputActionJump;
    private InputAction InputActionStartGame;

    //Jump Test
    public bool canJump = true;
    public bool jumping { get; private set; }
    public bool falling { get; private set; }
    public bool grounded { get; private set; } = true;
    public bool doneRoom = false;
    public Vector3 maxSize = new Vector3(2f, 2f, 2f);
    public Vector3 minSize = new Vector3(0.75f, 0.75f, 0.75f);

    //Temp
    public PitTrap pitTrapTest;

    //Audio
    public AudioSource sfxSource;
    public AudioClip jumpSFX;
    public AudioClip thudSFX;
    public AudioClip powerUpSfx;
    public AudioClip powerDownSfx;
    public AudioClip deathSfx;
    public AudioClip buttonSfx;
    public AudioClip leverSfx;
    public bool hasPlayed;



    //Timers
    //Button
    public float buttonCooldown = 3f;
    public float buttonTimer = 3f;
    public bool buttonOnCooldown = false;
    //Switch
    public float switchCooldown;
    public float switchTimer;
    public bool switchOnCooldown = false;
    //Lever
    public float leverCooldown;
    public float leverTimer;
    public bool leverOnCooldown = false;

    public void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        gameManager.playerControllers.Add(this);
        score = 0;
        hasPlayed = true;
        canJump = true;
    }

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
        InputActionStartGame = playerInput.actions.FindAction($"Player/StartGame");
    }

    // Assign player number on spawn
    public void AssignPlayerNumber(int playerNumber)
    {
       this.PlayerNumber  = playerNumber;
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
        if (!canControl) return;
        if(!gameManager.gameStarted)
        {
            if (isTrapper )
            {
                if (InputActionStartGame.WasPressedThisFrame())
                {
                    gameManager.StartGame();
                }
            } 
        }
        switch (isTrapper)
        {
            case true:
                if (InputActionJump.WasPressedThisFrame())
                {
                    trapperInteract = true;
                }
                break;
            case false:
                // Read the "Jump" action state, which is a boolean value
                if (InputActionJump.WasPressedThisFrame() && canJump)
                {
                    // Buffer input becuase I'm controlling the Rigidbody through FixedUpdate
                    // and checking there we can miss inputs.
                    DoJump = true;
                    jumping = true;
                    canJump = false;
                    sfxSource.PlayOneShot(jumpSFX);
                }
                break;
        } 
    }

    // Runs each phsyics update
    void FixedUpdate()
    {
        if (!canControl) return;
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
                canJump = true;
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
                    //Debug.Log("Button on CD");
                    trapperInteract = false;
                }
                else
                {
                    //Debug.Log("Interacted with Button");
                    Animator buttonAnim = hit.collider.GetComponent<Animator>();
                    this.SpriteRenderer.enabled = false;
                    buttonAnim.SetBool("pressingButton", true);
                    trapperInteract = false;
                    buttonTimer = 3f;
                    canControl = false;
                    sfxSource.PlayOneShot(buttonSfx);
                    //buttonOnCooldown = true;
                }
                    
            }
            else if (hit.collider.gameObject.CompareTag("Switch"))
            {
                //Switch Interaction Logic
                if(switchOnCooldown)
                {
                    trapperInteract = false;
                }
                else
                {
                    //TrigggerSwitchTraps
                    Animator switchAnim = hit.collider.GetComponent<Animator>();
                    this.SpriteRenderer.enabled = false;
                    switchAnim.SetBool("pullingSwitch", true);
                    trapperInteract = false;
                    switchTimer = 6f;
                    canControl = false;
                    sfxSource.PlayOneShot(leverSfx);
                }
            }
            else if (hit.collider.gameObject.CompareTag("Lever"))
            {
                if(leverOnCooldown)
                {
                    trapperInteract = false;
                }
                else
                {
                    Animator leverAnim = hit.collider.GetComponent<Animator>();
                    this.SpriteRenderer.enabled = false;
                    leverAnim.SetBool("pullingLever", true);
                    trapperInteract = false;
                    leverTimer = 3f;
                    canControl = false;
                    hasPlayed = false;
                    sfxSource.PlayOneShot(powerDownSfx);
                }
            }
            else
            {
                trapperInteract = false;
                //Debug.Log(hit.collider.gameObject);
            }
        }
        
    }

    private void TrapperTimers()
    {
        ButtonTimer();
        SwitchTimer();
        LeverTimer();
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
            //Debug.Log(buttonTimer);
        }
    }

    private void SwitchTimer()
    {

        if (switchTimer <= 0)
        {
            switchTimer = switchCooldown;
            switchOnCooldown = false;
        }
        if (switchOnCooldown)
        {
            switchTimer -= Time.deltaTime;
        }
    }

    private void LeverTimer()
    {

        if (leverTimer <= 0)
        {
            leverTimer = leverCooldown;
            leverOnCooldown = false;
            if(!hasPlayed)
            {
                sfxSource.PlayOneShot(powerUpSfx);
                hasPlayed= true;
            }
        }
        if (leverOnCooldown)
        {
            leverTimer -= Time.deltaTime;
        }
    }

    public void GoToNextRoom(int roomNumber)
    {
        if (isTrapper) return;
        Rigidbody2D.linearVelocity = Vector3.zero;
        Rigidbody2D.angularVelocity = 0;
        Debug.Log("current room: " +roomNumber);
        switch (PlayerNumber)
        {
            case 1:
                Debug.Log("Trapper moved by runner logic");
                break;
            case 2:
                this.transform.position = player2Points[roomNumber].roomStartPos;
                break;
            case 3:
                this.transform.position = player3Points[roomNumber].roomStartPos;
                break;
            case 4:
                this.transform.position = player4Points[roomNumber].roomStartPos;
                break;
            case 5:
                this.transform.position = player5Points[roomNumber].roomStartPos;
                break;
            case 6:
                this.transform.position = player6Points[roomNumber].roomStartPos;
                break;

        }
        canControl = false;
        doneRoom = false;
        
    }

    public void OnCollisionEnter2D(UnityEngine.Collision2D collision)
    {
        Debug.Log("Collided with " + collision.gameObject);
        //Debug.Log(grounded);
        if (collision.gameObject.CompareTag("Car"))
        {
            RunnerDie();
            sfxSource.PlayOneShot(thudSFX);
        }
       
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Fire"))
        {
            RunnerDie();
            sfxSource.PlayOneShot(deathSfx);

        }
        else if (collider.gameObject.CompareTag("DoneRoom"))
        {
            canControl = false;
            doneRoom = true;
            Rigidbody2D.linearVelocity = Vector3.zero;
            Rigidbody2D.angularVelocity = 0;
            Rigidbody2D.transform.position = Vector3.zero;
            if(collider.gameObject.GetComponent<EndRoomBox>().first == true)
            {
                score += 3;
            }
            else
            {
                score += 1;
            }
        }
        else if (collider.gameObject.CompareTag("Victory"))
        {
            Debug.Log("Player " + this.PlayerNumber + " wins!");
        }
    }
    public void OnTriggerStay2D(Collider2D collider)
    {
        //Debug.Log("OnTriggerStay2D");
        if(collider.gameObject.CompareTag("ConveyerBelt"))
        {
            //Debug.Log("in conveyer belt");
            if (grounded)
            {
                ConveyerBelt belt = collider.transform.parent.gameObject.GetComponent<ConveyerBelt>();
                if (belt != null)
                {
                    Rigidbody2D.AddForce(belt.GetRot() * 5, ForceMode2D.Force);
                    Debug.Log(belt.GetRot());
                    //Debug.Log("Adding force!");
                }
                else
                {
                    //Debug.Log("Belt is null");
                }
            }
        }
        //Pittrap
        //Debug.Log("Collided with " + collider.gameObject);
        //Debug.Log(grounded);
        if (collider.gameObject.CompareTag("PitTrap"))
        {
            if (grounded)
            {
                if(collider.bounds.Contains(this.GetComponent<Collider2D>().bounds.center))
                {
                    RunnerDie();
                    sfxSource.PlayOneShot(deathSfx);
                }

            }
        }
        if (collider.gameObject.CompareTag("Lava"))
        {
            if (grounded)
            {
                if (collider.bounds.Contains(this.GetComponent<Collider2D>().bounds.center))
                {
                    RunnerDie();
                    sfxSource.PlayOneShot(deathSfx);
                }

            }
        }
    }

    public void RunnerDie()
    {
        this.Rigidbody2D.angularVelocity = 0;
        this.Rigidbody2D.linearVelocity = Vector3.zero;
        doneRoom = true;
        switch (this.PlayerNumber)
        {
            case 1:
                Debug.Log("Trapper died to runner logic!");
                return;
            case 2:
                this.transform.position = player2Jail.transform.position;               
                return;
            case 3:
                this.transform.position = player3Jail.transform.position;
                return;
            case 4:
                this.transform.position = player4Jail.transform.position;
                return;
            case 5:
                this.transform.position = player5Jail.transform.position;
                return;
            case 6:
                this.transform.position = player6Jail.transform.position;
                return;
            default:
                Debug.Log("Failed to send dead player to jail.");
                return;

        }
        //SpriteRenderer.color = Color.white;
        //canControl = false;
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
