using UnityEngine;

public class RayCast : MonoBehaviour
{
    Ray ray;
    float rayMaxDistance = 5;
    public LayerMask layersToHit;

    private void Start()
    {
        ray = new Ray(transform.position, transform.right);
    }

    void CheckForColliders()
    {
        if (Physics.Raycast(ray, out RaycastHit hit, rayMaxDistance, layersToHit))
        {
            Debug.Log(hit.collider.gameObject.name + "was hit!");
            // then do something
        }
    }
}
