using UnityEngine;

public class PlayerCameraMovement : MonoBehaviour
{
    PlayerInput playerInput;
    Transform cameraFollowPoint;
    float lookSpeed = 10f;
    [SerializeField]
    float verticalRotationLimit = 60f;
    Vector2 lookInput;
    Vector3 localRotation;

    public void SetCameraFollowPoint(Transform cameraFollowPoint)
    {
        this.cameraFollowPoint = cameraFollowPoint;
    }

    void SetInputActions()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.playerInputActions.Player.Look.performed += (context) => lookInput = context.ReadValue<Vector2>();
        playerInput.playerInputActions.Player.Look.canceled += (context) => lookInput = Vector2.zero;
    }

    void Start()
    {
        SetInputActions();
        
        lookInput = Vector2.zero;
        localRotation = transform.localRotation.eulerAngles;
    }

    void Update()
    {
        VerticalLook();
        HorizontalLook();
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
}
