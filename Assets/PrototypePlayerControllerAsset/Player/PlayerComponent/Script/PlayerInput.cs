using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public PlayerInputActions playerInputActions;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
