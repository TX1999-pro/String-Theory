using UnityEngine;

public class CompleteTrigger : MonoBehaviour
{
    public GameManager gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            gameManager.CompleteLevel();
        }
    }
}
