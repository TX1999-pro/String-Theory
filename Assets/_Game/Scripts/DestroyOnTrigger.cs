using UnityEngine;
using System.Collections;

public class DestroyOnTrigger : MonoBehaviour
{
    public string triggerTag = "Shock Wave";
    private int hitCount = 0;
    private void OnParticleCollision(GameObject other)
    {
        // Debug.Log("collision detected");
        if (other.CompareTag(triggerTag))
        {
            hitCount += 1;
            Debug.Log( this.gameObject.name + "Hit Count: " + hitCount);

            if (hitCount > 6)
            {
                Destroy(this.gameObject, 0.5f); // destory the gameObject it is attached to after delay
                Debug.Log("Obstacle is destroyed.");
            }

        }
    }
}
