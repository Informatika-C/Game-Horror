using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    Camera playerCamera;

    Vector2 movementInput;
    Vector3 processedInput;

    [SerializeField]
    float moveSpeed = 5f;

    [SerializeField]
    float gravity = 9.81f;

    CharacterController characterController;

    public void OnMove(CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        processedInput = ProcessInputRelativeToCamera(movementInput, playerCamera);
    }

    void FixedUpdate()
    {        
        processedInput = moveSpeed * Time.fixedDeltaTime * processedInput;
        processedInput = AddGravity(processedInput);
        characterController.Move(processedInput);
    }

    Vector3 ProcessInputRelativeToCamera(Vector2 input, Camera camera)
    {
        Vector3 forward = camera.transform.forward;
        Vector3 right = camera.transform.right;

        forward.y = 0;
        right.y = 0;

        Vector3 processedInput = forward * input.y + right * input.x;
        processedInput.Normalize();

        return processedInput;
    }

    Vector3 AddGravity(Vector3 input)
    {
        if(!characterController.isGrounded){
            input.y = -gravity * Time.fixedDeltaTime;
        }
        else{
            input.y = 0;
        }

        return input;
    }
}
