using UnityEngine;

public class ColliderRaycast : MonoBehaviour
{
    public Collider colliderToHit;
    Ray ray;

    private void Start()
    {
        ray = new Ray(transform.position, transform.right);
    }

    void CheckForColliders()
    {
        if (colliderToHit.Raycast(ray, out RaycastHit hit, 2))
        {
            Debug.Log(hit.collider.gameObject.name + "was hit");
        }
    }
}
