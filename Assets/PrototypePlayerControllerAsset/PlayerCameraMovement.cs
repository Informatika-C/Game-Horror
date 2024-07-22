using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class PlayerCameraMovement : MonoBehaviour
{
    [SerializeField]
    Transform FollowCamera;
    float lookSpeed = 10f;
    [SerializeField]
    float verticalRotationLimit = 60f;
    Vector2 lookInput;
    Vector3 localRotation;

    public void OnLook(CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

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
        Vector3 cameraRotation = FollowCamera.localRotation.eulerAngles;
        cameraRotation.x -= lookInput.y * lookSpeed * Time.deltaTime;
        
        if(cameraRotation.x > verticalRotationLimit && cameraRotation.x < 180f)
        {
            cameraRotation.x = verticalRotationLimit;
        }
        else if(cameraRotation.x < 360f - verticalRotationLimit && cameraRotation.x > 180f)
        {
            cameraRotation.x = 360f - verticalRotationLimit;
        }

        FollowCamera.localRotation = Quaternion.Euler(cameraRotation);
    }

    void HorizontalLook()
    {
        localRotation.y += lookInput.x * lookSpeed * Time.deltaTime;
        transform.localRotation = Quaternion.Euler(localRotation);
    }
}
