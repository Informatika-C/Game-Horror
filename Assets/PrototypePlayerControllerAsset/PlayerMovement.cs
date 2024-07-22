using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    Camera playerCamera;

    Vector2 movementInput = Vector2.zero;
    bool jumpInput = false;
    bool crouchInput = false;
    bool runInput = false;

    Vector3 playerVelocity;

    [SerializeField]
    float playerSpeed = 3f;

    [SerializeField]
    float gravity = -15f;

    [SerializeField]
    float jumpHeight = 1.5f;

    float originalHeight;
    Vector3 originalCenter;
    float crouchHeight = 1.5f;
    float crouchCenter = -0.25f;
    bool isCrouching = false;
    bool isRunning = false;

    CharacterController characterController;

    public void OnMove(CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    public void OnJump(CallbackContext context)
    {
        jumpInput = context.ReadValueAsButton();
    }

    public void OnRun(CallbackContext context)
    {
        runInput = context.ReadValueAsButton();
    }

    public void OnCrouch(CallbackContext context)
    {
        crouchInput = context.ReadValueAsButton();
    }

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        originalHeight = characterController.height;
        originalCenter = characterController.center;
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
        PlayerRun();
        PlayerCrouch();
        PlayerJump();
        PlayerMove();

        playerVelocity.y += gravity * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }

    void PlayerMove()
    {
        float processedSpeed = playerSpeed;
        if(isRunning) processedSpeed *= 2;
        else if(isCrouching) processedSpeed /= 2;
        bool groundedPlayer = characterController.isGrounded;

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector3 move = ProcessInputRelativeToCamera(movementInput, playerCamera);
        characterController.Move(processedSpeed * Time.deltaTime * move);
    }

    void PlayerRun()
    {
        if( runInput && 
            !isCrouching && 
            characterController.isGrounded)
        {
            isRunning = true;
        }
        else{
            isRunning = false;
        }
    }

    void PlayerJump()
    {
        if (jumpInput && 
            characterController.isGrounded)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravity);
        }
    }

    void PlayerCrouch()
    {
        if(crouchInput){
            isCrouching = true;
        }
        else if(!crouchInput && isCrouching){
            if(IsAllowedToStand(characterController)){
                isCrouching = false;
            }
        }

        if(isCrouching){
            characterController.height = crouchHeight;
            characterController.center = new Vector3(0, crouchCenter, 0);
        }
        else{
            characterController.height = originalHeight;
            characterController.center = originalCenter;
        }
    }

    bool IsAllowedToStand(CharacterController characterController)
    {
        Vector3 center = transform.TransformPoint(characterController.center);
        Vector3 top = center + Vector3.up * (characterController.height / 2);
        Collider[] colliders = Physics.OverlapCapsule(center, top, characterController.radius);

        foreach(Collider collider in colliders)
        {
            if(collider != characterController)
            {
                return false;
            }
        }

        return true;
    }
}
