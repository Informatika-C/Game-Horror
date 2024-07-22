using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(PlayerInput)),
 RequireComponent(typeof(PlayerMovement)),
 RequireComponent(typeof(PlayerCameraMovement))]
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

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        playerMovement = GetComponent<PlayerMovement>();
        playerCameraMovement = GetComponent<PlayerCameraMovement>();

        playerMovement.SetCamera(FPSCamera);
        playerCameraMovement.SetCameraFollowPoint(cameraFollowPoint);

        FPSCamera.Follow = cameraFollowPoint;
        FPSCamera.LookAt = cameraLookPoint;
    }
}
