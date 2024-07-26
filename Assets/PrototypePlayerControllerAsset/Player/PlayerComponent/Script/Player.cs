using System.Collections.Generic;
using Cinemachine;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement)),
 RequireComponent(typeof(PlayerCameraMovement)),
 RequireComponent(typeof(PlayerAnimation)),
 RequireComponent(typeof(PlayerAnimationPresenter))]
public class Player : NetworkBehaviour
{
    public CinemachineVirtualCamera FPSCamera;
    
    [SerializeField]
    Transform cameraFollowPoint;
    [SerializeField]
    Transform cameraLookPoint;

    [HideInInspector]
    public PlayerMovement playerMovement;
    [HideInInspector]
    public PlayerCameraMovement playerCameraMovement;
    [HideInInspector]
    public PlayerAnimation playerAnimation;
    [HideInInspector]
    public PlayerAnimationPresenter playerAnimationPresenter;

    private List<SkinnedMeshRenderer> LocalHideMeshs = new List<SkinnedMeshRenderer>();

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerCameraMovement = GetComponent<PlayerCameraMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerAnimationPresenter = GetComponent<PlayerAnimationPresenter>();

        playerAnimation.SetAnimator(GetComponent<Animator>());
        playerAnimationPresenter.SetPlayerAnimation(playerAnimation);
        playerAnimationPresenter.SetPlayerMovement(playerMovement);
        playerCameraMovement.SetPlayerMovement(playerMovement);

        playerMovement.SetCamera(FPSCamera);
        playerCameraMovement.SetCameraFollowPoint(cameraFollowPoint);

        FPSCamera.Follow = cameraFollowPoint;
        FPSCamera.LookAt = cameraLookPoint;

        GetAllHideMeshs();
    }

    void GetAllHideMeshs()
    {
        SkinnedMeshRenderer[] skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshRenderers)
        {
            if (skinnedMeshRenderer.CompareTag("LocalHide"))
            {
                LocalHideMeshs.Add(skinnedMeshRenderer);
            }
        }
    }

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