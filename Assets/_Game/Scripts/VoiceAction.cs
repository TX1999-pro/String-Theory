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
    //[SerializeField] private float horizontalInput;
    private Rigidbody player;

    public float moveSpeed = 10.0f;

    private void Awake()
    {
        player = GetComponent<Rigidbody>();
        playerController = gameObject.GetComponent<PlayerController>();

        // add voice actions to the dict
        actions.Add("Stop", Stop);

        actions.Add("Forward", Forward);
        actions.Add("Right", Forward);
        actions.Add("Go right", Forward);
        actions.Add("Move", Forward);
        actions.Add("Move right", Forward);

        actions.Add("Back", Back);
        actions.Add("Left", Back);
        actions.Add("Go left", Back);
        actions.Add("Move left", Back);

        actions.Add("Jump", Jump);
        actions.Add("Fire", Fire);

        keywordRecognizer = new KeywordRecognizer(actions.Keys.ToArray());
        keywordRecognizer.OnPhraseRecognized += RecognizedSpeech;
        keywordRecognizer.Start();
    }

    private void OnDestroy()
    {
        // dispose the keywordRecognizer when the player object is destroyed
        if (keywordRecognizer != null)
        {
            keywordRecognizer.Stop();
            keywordRecognizer.Dispose();
        }
    }

    private void RecognizedSpeech(PhraseRecognizedEventArgs speech)
    {
        if (playerController.voiceActionEnabled)
        {
            Debug.Log(speech.text);
            actions[speech.text].Invoke();
        }
        else
        {
            Debug.Log("Press L to turn on the Listener.");
        }

    }
    public void Jump()
    {
        if (playerController.IsGrounded())
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
            player.AddForce(Vector3.up * playerController.jumpForce, ForceMode.Impulse);
            playerController.PlayShockWave(playerController.jumpWave.GetComponent<ParticleSystem>());
            StartCoroutine(JumpOver());
        }
    }
    private IEnumerator JumpOver()
    {
        yield return new WaitForSeconds(0.5f);
        // move right a bit
        player.AddForce(Vector3.right * playerController.moveSpeed, ForceMode.Impulse);
    }
    private void Stop()
    {
        // stop from continuous dash
        playerController.horizontalInput = 0;
        Debug.Log("Stopped!");
        playerController.running = false;
    }
    private void Forward()
    {
        player.AddForce(Vector3.right * playerController.moveSpeed, ForceMode.Impulse);
        playerController.horizontalInput = 1;
        Debug.Log("Moving right");
    }
    private void Back()
    {
        player.AddForce(Vector3.left * playerController.moveSpeed, ForceMode.Impulse);
        playerController.horizontalInput = -1;
        Debug.Log("Moving left");
    }

    private void Fire()
    {
        // debug
        playerController.Fire();
        Debug.Log("Voice Fire!");
    }

}