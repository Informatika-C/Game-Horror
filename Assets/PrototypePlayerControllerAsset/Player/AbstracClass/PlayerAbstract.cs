using System.Collections.Generic;
using Cinemachine;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement)),
 RequireComponent(typeof(PlayerItem)),
 RequireComponent(typeof(PlayerStat)),
 RequireComponent(typeof(PlayerCameraMovement)),
 RequireComponent(typeof(PlayerAnimation))]
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
    public PlayerStat playerStat;
    [HideInInspector]
    public PlayerItem playerItem;
    [HideInInspector]
    public Animator animator;
    protected SkinnedMeshRenderer[] skinnedMeshes;
    protected List<SkinnedMeshRenderer> LocalHideMeshs = new();

    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerCameraMovement = GetComponent<PlayerCameraMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerAnimationPresenter = GetComponent<PlayerAnimationPresenterAbstract>();
        playerItem = GetComponent<PlayerItem>();
        playerStat = GetComponent<PlayerStat>();
        animator = GetComponent<Animator>();

        playerStat.SetPlayerItem(playerItem);
        playerAnimation.SetAnimator(animator);
        playerAnimationPresenter.SetPlayerAnimation(playerAnimation);
        playerAnimationPresenter.SetPlayerMovement(playerMovement);
        playerAnimationPresenter.SetPlayerCameraMovement(playerCameraMovement);
        playerAnimationPresenter.SetPlayerItem(playerItem);
        playerCameraMovement.SetPlayerMovement(playerMovement);

        playerMovement.SetCamera(FPSCamera);
        playerCameraMovement.SetCameraFollowPoint(cameraFollowPoint);

        FPSCamera.Follow = cameraFollowPoint;
        FPSCamera.LookAt = cameraLookPoint;

        GetAllSkinnedMesh();
        GetAllHideMeshs();
    }


    void GetAllSkinnedMesh(){
        skinnedMeshes = GetComponentsInChildren<SkinnedMeshRenderer>();
    }

    void GetAllHideMeshs()
    {
        foreach (SkinnedMeshRenderer skinnedMeshRenderer in skinnedMeshes)
        {
            if (skinnedMeshRenderer.CompareTag("LocalHide"))
            {
                LocalHideMeshs.Add(skinnedMeshRenderer);
            }
        }
    }
}
