using UnityEngine;

public class PlayerCameraMovement : MonoBehaviour
{
    PlayerInput playerInput;
    Transform cameraFollowPoint;
    PlayerMovement playerMovement;
    float lookSpeed = 10f;
    [SerializeField]
    float verticalRotationLimit = 60f;
    Vector2 lookInput;
    Vector3 localRotation;
    Vector3 originalCameraFollowPointLocalPosition;
    [SerializeField]
    float crouchCameraOffset = 0.5f;
    [SerializeField]
    public float transitionSpeed = 1f;
    Vector3 targetCameraFollowPointLocalPosition;

    public void SetCameraFollowPoint(Transform cameraFollowPoint)
    {
        this.cameraFollowPoint = cameraFollowPoint;
    }

    public void SetPlayerMovement(PlayerMovement playerMovement)
    {
        this.playerMovement = playerMovement;
    }

    void SetInputActions()
    {
        playerInput = InputManager.instance.playerInput;
        playerInput.playerInputActions.Player.Look.performed += (context) => lookInput = context.ReadValue<Vector2>();
        playerInput.playerInputActions.Player.Look.canceled += (context) => lookInput = Vector2.zero;
    }

    void Start()
    {
        SetInputActions();
        
        lookInput = Vector2.zero;
        localRotation = transform.localRotation.eulerAngles;

        originalCameraFollowPointLocalPosition = cameraFollowPoint.localPosition;
    }

    void Update()
    {
        VerticalLook();
        HorizontalLook();
        CrouchCamera();
    }

    void VerticalLook()
    {
        Vector3 cameraRotation = cameraFollowPoint.localRotation.eulerAngles;
        cameraRotation.x -= lookInput.y * lookSpeed * Time.deltaTime;
        
        if(cameraRotation.x > verticalRotationLimit && cameraRotation.x < 180f)
        {
            cameraRotation.x = verticalRotationLimit;
        }
        else if(cameraRotation.x < 360f - verticalRotationLimit && cameraRotation.x > 180f)
        {
            cameraRotation.x = 360f - verticalRotationLimit;
        }

        cameraFollowPoint.localRotation = Quaternion.Euler(cameraRotation);
    }

    void HorizontalLook()
    {
        localRotation.y += lookInput.x * lookSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(localRotation);
    }

    void CrouchCamera()
    {
        if(playerMovement.GetIsCrouch())
        {
            float yOffSet = originalCameraFollowPointLocalPosition.y - crouchCameraOffset;

            targetCameraFollowPointLocalPosition = new Vector3(
            originalCameraFollowPointLocalPosition.x,
            yOffSet,
            originalCameraFollowPointLocalPosition.z);
        }
        else if(!playerMovement.GetIsCrouch() && targetCameraFollowPointLocalPosition != originalCameraFollowPointLocalPosition)
        {
            targetCameraFollowPointLocalPosition = originalCameraFollowPointLocalPosition;
        }

        cameraFollowPoint.localPosition = Vector3.Lerp(
        cameraFollowPoint.localPosition,
        targetCameraFollowPointLocalPosition,
        Time.deltaTime * transitionSpeed);
    }
}