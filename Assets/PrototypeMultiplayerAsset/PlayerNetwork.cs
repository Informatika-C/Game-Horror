using UnityEngine;

[RequireComponent(typeof(PlayerMovement)),
 RequireComponent(typeof(PlayerCameraMovement)),
 RequireComponent(typeof(PlayerAnimation)),
 RequireComponent(typeof(PlayerAnimationPresenterNetwork))]
public class PlayerNetwork : PlayerAbstract
{
    void UnhideLocalMeshs()
    {
        foreach (SkinnedMeshRenderer skinnedMeshRenderer in LocalHideMeshs)
        {
            skinnedMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
    }
    
    void Start()
    {
        if (IsOwner) 
        {
            FPSCamera.enabled = true;
            playerMovement.SetUpInput(InputManager.instance.playerInput);
            playerCameraMovement.SetUpInput(InputManager.instance.playerInput);
        }
        else if (!IsOwner)
        {
            FPSCamera.enabled = false;
            UnhideLocalMeshs();
        }
    }
}
