using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(PlayerInput)),
 RequireComponent(typeof(PlayerMovement)),
 RequireComponent(typeof(PlayerCameraMovement)),
 RequireComponent(typeof(PlayerAnimation)),
 RequireComponent(typeof(PlayerAnimationPresenter))]
public class Player : MonoBehaviour
{
    public CinemachineVirtualCamera FPSCamera;
    
    [SerializeField]
    Transform cameraFollowPoint;
    [SerializeField]
    Transform cameraLookPoint;

    [HideInInspector]
    public PlayerInput playerInput;
    [HideInInspector]
    public PlayerMovement playerMovement;
    [HideInInspector]
    public PlayerCameraMovement playerCameraMovement;
    [HideInInspector]
    public PlayerAnimation playerAnimation;
    [HideInInspector]
    public PlayerAnimationPresenter playerAnimationPresenter;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCameraMovement = GetComponent<PlayerCameraMovement>();
        playerAnimation = GetComponent<PlayerAnimation>();
        playerAnimationPresenter = GetComponent<PlayerAnimationPresenter>();

        playerAnimationPresenter.SetPlayerAnimation(playerAnimation);
        playerAnimationPresenter.SetPlayerMovement(playerMovement);
        playerCameraMovement.SetPlayerMovement(playerMovement);

        playerMovement.SetCamera(FPSCamera);
        playerCameraMovement.SetCameraFollowPoint(cameraFollowPoint);

        FPSCamera.Follow = cameraFollowPoint;
        FPSCamera.LookAt = cameraLookPoint;
    }
}
