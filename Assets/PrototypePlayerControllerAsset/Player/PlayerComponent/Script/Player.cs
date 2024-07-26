using UnityEngine;

[RequireComponent(typeof(PlayerMovement)),
 RequireComponent(typeof(PlayerCameraMovement)),
 RequireComponent(typeof(PlayerAnimation)),
 RequireComponent(typeof(PlayerAnimationPresenter))]
public class Player : PlayerAbstract
{
    void Start()
    {
        FPSCamera.enabled = true;
        playerMovement.SetUpInput(InputManager.instance.playerInput);
        playerCameraMovement.SetUpInput(InputManager.instance.playerInput);
    }   
}