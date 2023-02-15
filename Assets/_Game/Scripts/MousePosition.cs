using UnityEngine;
using UnityEngine.InputSystem;
public class MousePosition : MonoBehaviour
{
    public Vector2 mousePosition;
    void Update()
    {
        mousePosition = Mouse.current.position.ReadValue();
    }
}