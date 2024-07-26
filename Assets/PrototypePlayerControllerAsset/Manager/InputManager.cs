using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    [HideInInspector]
    public PlayerInput playerInput;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.LogError("InputManager already exists. Deleting new one.");
            Destroy(this);
        }

        playerInput = GetComponent<PlayerInput>();
        playerInput.playerInputActions = new PlayerInputActions();
    }

    void OnEnable()
    {
        if(playerInput == null) return;
        playerInput.playerInputActions?.Player.Enable();
    }

    void OnDisable()
    {
        if(playerInput == null) return;
        playerInput.playerInputActions?.Player.Disable();
    }
}
