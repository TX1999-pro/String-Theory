using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private float horizontalInput;
    private Rigidbody player;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private bool isGrounded;

    public float setJumpForce =5.0f;
    [HideInInspector] public float jumpForce;
    public int superJumpsRemain = 0;
    public Transform groundCheckCollider;
    public LayerMask groundLayer;

    public PlayerInputActions1 gameInput;
    public float moveSpeed = 5;

    private InputAction move;
    private InputAction jump;
    private InputAction fire;
    private InputAction toggleMic; // enable/disable voice action
    private InputAction keepMoving; // move to the next destination

    public bool voiceActionEnabled = false;

    private void Awake()
    {
        gameInput = new PlayerInputActions1();
        gameInput.Player.Fire.performed += context => Fire();

        //playerControls.Player.HorizontalMove.performed += ctx =>
        //{
        //    var v2 = ctx.ReadValue<Vector2>();
        //    movement = new Vector3(v2.x, 0, 0); // only care for x-axis
        //}
    }

    private void OnEnable()
    {
        //gameInput.Enable();
        
        // by specific actions for debugging
        move = gameInput.Player.Move;
        jump = gameInput.Player.Jump;
        fire = gameInput.Player.Fire;
        toggleMic = gameInput.Player.ToggleMic;
        keepMoving = gameInput.Player.KeepMoving;

        move.Enable();
        jump.Enable();
        toggleMic.Enable();
        keepMoving.Enable();
        fire.Enable();

    }

    private void OnDisable()
    {
        //gameInput.Disable();
        move.Disable();
        jump.Disable();
        toggleMic.Disable();
        keepMoving.Disable();
        fire.Disable();
    }

    void Start()
    {
        player = this.GetComponent<Rigidbody>();
        gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
       
        // update player movement
        if (!gameManager.isLevelCompleted)
        {
            // update movement if level isn't completed
            player.velocity = new Vector3(horizontalInput * moveSpeed, player.velocity.y, 0);
        } 
        else
        {
            player.velocity = new Vector3(0, 0, 0); // freeze the player once the level completion is triggered
        }
        
        // restart if below certain position
        if (player.position.y < -5f)
        {
            FindObjectOfType<GameManager>().EndGame();
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<Vector2>().x;
        //Debug.Log(horizontalInput);
    }
    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            //Debug.Log("Jumped!");
            if (superJumpsRemain > 0)
            {
                jumpForce = 2* setJumpForce;
                superJumpsRemain--;
            }
            else
            {
                jumpForce = setJumpForce;
            }
            player.velocity += Vector3.up * jumpForce;
            //player.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            //player.velocity = new Vector3(horizontalInput, player.velocity.y, 0);
        }
    }

    public void ToggleMic(InputAction.CallbackContext btn)
    {

        if (btn.started)
        {
            // turn on
            voiceActionEnabled = !voiceActionEnabled;
            Debug.Log("Voice Action Control: " + voiceActionEnabled.ToString());
        }
    }
    public void Fire()
    {
        // do something to destroy the obstacle in front
        // send a shock wave?
        Debug.Log("Fire!");
    }


    public void KeepMoving(InputAction.CallbackContext btn)
    {

        if (btn.started)
        {
            // player will move forward(right) with constant velocity
            horizontalInput = btn.ReadValue<Vector2>().x;
            Debug.Log("Keep moving " + horizontalInput.ToString());
        }
    }

    private bool IsGrounded()
    {
        isGrounded = Physics.OverlapSphere(groundCheckCollider.position, 0.1f, groundLayer).Length != 0;
        return isGrounded;
        // checking if the groundchecktransform is colliding except with the player

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 10) // layer 10 is coin
        {
            Destroy(other.gameObject);
            superJumpsRemain += 1;
            //GameManager.instance.IncreaseScore(1);
        }

        if (other.gameObject.tag == "Portal")
        {
            Debug.Log("Entering the portal");
        }

    }

    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        if (collision.gameObject.tag == "Obstacles")
        {
            horizontalInput = 0; // set horizontal velocity to 0
            Debug.Log("Hit an obstacle");
        }
    }


}
