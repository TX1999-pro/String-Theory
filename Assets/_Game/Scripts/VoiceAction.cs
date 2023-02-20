using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows.Speech;
using UnityEngine.InputSystem;

public class VoiceAction : MonoBehaviour
{
    //
    private KeywordRecognizer keywordRecognizer;
    public Dictionary<string, Action> actions = new();

    // character & player control
    [SerializeField] private PlayerController playerController;
    public bool voiceActionEnabled = false;
    private float horizontalInput;
    private Rigidbody player;

    public float moveSpeed = 3.0f;
    public PlayerInputActions1 playerControls;
    private InputAction toggleMicrophone;

    private void Awake()
    {
        playerControls = GetComponent<PlayerController>().playerControls;

    }
    private void OnEnable()
    {
        toggleMicrophone = playerControls.Player.ToggleMic;
        toggleMicrophone.Enable();

    }

    private void OnDisable()
    {
        toggleMicrophone.Disable();
    }

    private void Start()
    {
        player = GetComponent<Rigidbody>();
        playerController = gameObject.GetComponent<PlayerController>();

        // add voice actions to the dict
        actions.Add("Forward", Forward);
        actions.Add("Stop", Stop);
        actions.Add("Right", Forward);
        actions.Add("Back", Back);
        actions.Add("Left", Back);
        actions.Add("Go left", Back);
        actions.Add("Jump", Jump);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();
    }

    private void FixedUpdate()
    {
        if (voiceActionEnabled)
        {
            player.velocity = new Vector3(horizontalInput * playerController.moveSpeed, player.velocity.y, 0);
        }
        
    }

    public void ToggleMic(InputAction.CallbackContext context)
    {
        voiceActionEnabled = !voiceActionEnabled;
        if (voiceActionEnabled)
        {
            Debug.Log("Mic On, listening");
        }
        else
        {
            Debug.Log("Mic Off.");
        }
    }

    private void OnDestroy()
    {
        if (keywordRecognizer != null)
        {
            keywordRecognizer.Stop();
            keywordRecognizer.Dispose();
        }
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        Debug.Log(speech.text);
        actions[speech.text].Invoke();
    }
    public void Jump()
    {
        if (IsGrounded())
        {
            Debug.Log("Jumped!");
            if (playerController.superJumpsRemain > 0)
            {
                playerController.jumpForce = 2 * playerController.setJumpForce;
                playerController.superJumpsRemain--;
            }
            else
            {
                playerController.jumpForce = playerController.setJumpForce;
            }
            player.AddForce(Vector3.up * playerController.jumpForce, ForceMode.VelocityChange);
        }
    }

    private bool IsGrounded()
    {
        return Physics.OverlapSphere(playerController.groundCheckCollider.position, 0.1f,
            playerController.groundLayer).Length != 0;
        // checking if the groundchecktransform is colliding except with the player

    }
    private void Stop()
    {
        // stop from continuous dash
        horizontalInput = 0;
        Debug.Log("Stopped!");
    }
    private void Forward()
    {
        player.AddForce(Vector3.right * playerController.moveSpeed, ForceMode.Impulse);
        Debug.Log("Forward Action Done");
    }
    private void Back()
    {
        player.AddForce(Vector3.left * playerController.moveSpeed, ForceMode.Impulse);
        Debug.Log("Back Action Done");
    }
}