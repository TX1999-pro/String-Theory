using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    private bool isGrounded;
    private void OnCollisionEnter(UnityEngine.Collision collision)
    {
        isGrounded = true;
    }

    private void OnCollisionExit(UnityEngine.Collision collision)
    {
        isGrounded = false;
    }
}
