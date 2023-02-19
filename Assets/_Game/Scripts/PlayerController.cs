using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private bool jumpKeyWasPressed;
    private float horizontalInput;
    private Rigidbody player;
    [SerializeField] private bool isGrounded;
    public float setJumpForce =5.0f;
    [HideInInspector] public float jumpForce;
    public int superJumpsRemain = 0;
    public Transform groundCheckCollider;
    public LayerMask groundLayer;

    public PlayerInputActions playerControls;
    public float moveSpeed = 5;

    private InputAction move;
    private InputAction jump;


    private void Awake()
    {
        playerControls = new PlayerInputActions();

        //playerControls.Player.HorizontalMove.performed += ctx =>
        //{
        //    var v2 = ctx.ReadValue<Vector2>();
        //    movement = new Vector3(v2.x, 0, 0); // only care for x-axis
        //}
    }

    private void OnEnable()
    {
        //playerControls.Enable();
        
        // by events
        move = playerControls.Player.Move;
        move.Enable();
        jump = playerControls.Player.Jump;
        jump.Enable();
    }

    private void OnDisable()
    {
        //playerControls.Disable();
        move.Disable();
        jump.Disable();
    }

    void Start()
    {
        player = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        // oringial implementation
        //if (Input.GetButtonDown("Jump"))
        //{
        //    jumpKeyWasPressed = true;
        //}

        //horizontalInput = Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        // update player movement
        player.velocity = new Vector3(horizontalInput * moveSpeed, player.velocity.y, 0);
        // restart if below certain position
        if (player.position.y < -5f)
        {
            FindObjectOfType<GameManager>().EndGame();
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<Vector2>().x;
        //test update
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



}
