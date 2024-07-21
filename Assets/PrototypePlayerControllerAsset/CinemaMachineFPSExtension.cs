using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class CinemaMachineFPSExtension : CinemachineExtension
{
    [SerializeField]
    float lookSpeed;

    Vector2 lookInput;
    Vector3 localRotation;

    public void OnLook(CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    protected override void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        localRotation = transform.localRotation.eulerAngles;

        base.Awake();
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if(vcam.Follow == null)
            return;
        
        if(stage == CinemachineCore.Stage.Aim)
        {
            localRotation.x += lookInput.x * Time.deltaTime * lookSpeed;
            localRotation.y += lookInput.y * Time.deltaTime * lookSpeed;
            localRotation.y = Mathf.Clamp(localRotation.y, -90, 90);

            state.RawOrientation = Quaternion.Euler(-localRotation.y, localRotation.x, 0);
        }
    }
}
