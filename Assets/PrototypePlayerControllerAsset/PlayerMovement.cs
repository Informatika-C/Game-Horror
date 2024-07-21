using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    Camera playerCamera;

    bool groundedPlayer;

    Vector2 movementInput;
    bool jumpInput;

    Vector3 playerVelocity;

    [SerializeField]
    float playerSpeed = 5f;

    [SerializeField]
    float gravity = -15f;

    [SerializeField]
    float jumpHeight = 1.5f;

    CharacterController characterController;

    public void OnMove(CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnJump(CallbackContext context)
    {
        jumpInput = context.ReadValueAsButton();
    }

    void Awake()
    {
        movementInput = Vector2.zero;
        jumpInput = false;

        characterController = GetComponent<CharacterController>();
    }
    
    Vector3 ProcessInputRelativeToCamera(Vector2 input, Camera camera)
    {
        if(camera == null)
        {
            return input;
        }

        Vector3 forward = camera.transform.forward;
        Vector3 right = camera.transform.right;

        forward.y = 0;
        right.y = 0;

        Vector3 processedInput = forward * input.y + right * input.x;
        processedInput.Normalize();

        return processedInput;
    }

    void Update()
    {
        groundedPlayer = characterController.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = ProcessInputRelativeToCamera(movementInput, playerCamera);
        characterController.Move(playerSpeed * Time.deltaTime * move);

        // if (move != Vector3.zero)
        // {
        //     gameObject.transform.forward = move;
        // }

        if (jumpInput && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }

        playerVelocity.y += gravity * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }
}
