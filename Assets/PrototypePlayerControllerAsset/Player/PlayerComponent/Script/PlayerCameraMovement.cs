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
    public float transitionSpeed = 5f;
    Vector3 targetCameraFollowPointLocalPosition;
    float cameraY;
    public float YsmoothTime = 0.1f;

    private float targetCameraY;
    private float cameraYVelocity;

    public void SetCameraFollowPoint(Transform cameraFollowPoint)
    {
        this.cameraFollowPoint = cameraFollowPoint;
    }

    public void SetPlayerMovement(PlayerMovement playerMovement)
    {
        this.playerMovement = playerMovement;
    }

    public float GetCameraY()
    {
        return cameraY;
    }

    public void SetUpInput(PlayerInput playerInput)
    {
        this.playerInput = playerInput;
        SetInputActions();
    }

    void SetInputActions()
    {
        playerInput.playerInputActions.Player.Look.performed += (context) => lookInput = context.ReadValue<Vector2>();
        playerInput.playerInputActions.Player.Look.canceled += (context) => lookInput = Vector2.zero;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        lookInput = Vector2.zero;
        localRotation = transform.localRotation.eulerAngles;

        originalCameraFollowPointLocalPosition = cameraFollowPoint.localPosition;
    }

    void Update()
    {
        if (playerInput == null) return;
        VerticalLook();
        HorizontalLook();
        CrouchCamera();
    }

    void VerticalLook()
    {
        float angle = cameraFollowPoint.localRotation.eulerAngles.x;

        if(angle > 180f)
        {
            angle -= 360f;
        }

        float xRotation = -lookInput.y * lookSpeed * Time.deltaTime;
        angle = Mathf.Clamp(angle + xRotation, -verticalRotationLimit, verticalRotationLimit);

        cameraFollowPoint.localRotation = Quaternion.Euler(angle, 0, 0);

        targetCameraY = angle / verticalRotationLimit * -1;
        cameraY = Mathf.SmoothDamp(cameraY, targetCameraY, ref cameraYVelocity, YsmoothTime);
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