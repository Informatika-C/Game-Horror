using System.Collections.Generic;
using Cinemachine;
using Unity.Netcode;
using UnityEngine;

public abstract class PlayerAbstract : NetworkBehaviour
{
    [SerializeField]
    protected CinemachineVirtualCamera FPSCamera;    
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
    public PlayerAnimationPresenterAbstract playerAnimationPresenter;
    [HideInInspector]
    public PlayerItem playerItem;
    [HideInInspector]
    public Animator animator;
    protected List<SkinnedMeshRenderer> LocalHideMeshs = new List<SkinnedMeshRenderer>();

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerCameraMovement = GetComponent<PlayerCameraMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerAnimationPresenter = GetComponent<PlayerAnimationPresenterAbstract>();
        playerItem = GetComponent<PlayerItem>();
        animator = GetComponent<Animator>();

        playerAnimation.SetAnimator(animator);
        playerAnimationPresenter.SetPlayerAnimation(playerAnimation);
        playerAnimationPresenter.SetPlayerMovement(playerMovement);
        playerAnimationPresenter.SetPlayerCameraMovement(playerCameraMovement);
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
}
