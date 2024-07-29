using UnityEngine;

[RequireComponent(typeof(PlayerAnimationPresenter))]
public class Player : PlayerAbstract
{
    void Start()
    {
        FPSCamera.enabled = true;
        playerMovement.SetUpInput(InputManager.instance.playerInput);
        playerCameraMovement.SetUpInput(InputManager.instance.playerInput);
        animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        foreach (var mesh in skinnedMeshes)
        {
            mesh.updateWhenOffscreen = true;
        }
    }   
}