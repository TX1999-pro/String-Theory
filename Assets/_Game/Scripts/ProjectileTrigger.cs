using UnityEngine;
// code from: http://answers.unity.com/answers/1658742/view.html
public class ProjectileTrigger : MonoBehaviour
{
    /// <summary>
    /// Public fields
    /// </summary>
    public GameObject m_Projectile;    // this is a reference to your projectile prefab
    public Transform m_SpawnTransform; // this is a reference to the transform where the prefab will spawn

    /// <summary>
    /// Message that is called once per frame
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(m_Projectile, m_SpawnTransform.position, m_SpawnTransform.rotation);
        }
    }
}