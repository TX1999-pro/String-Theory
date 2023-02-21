using System;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Portal destination; // teleport destination
    public PlayerController Player;
    public bool canTeleport = true;

    private void Awake()
    {
        Player = FindObjectOfType<PlayerController>();
}
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && canTeleport)
        {
            destination.canTeleport = false;
            Player.transform.position = destination.transform.position;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("exited the portal");
        canTeleport = true;
    }
}
