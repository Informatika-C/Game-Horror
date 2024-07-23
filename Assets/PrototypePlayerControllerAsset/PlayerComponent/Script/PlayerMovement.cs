using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    PlayerInput playerInput;
    CinemachineVirtualCamera Camera;

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
    float crouchHeightOffset = -0.5f;
    float crouchCenterOffset = -0.25f;
    bool isCrouching = false;
    bool isRunning = false;

    CharacterController characterController;

    public void SetCamera(CinemachineVirtualCamera camera)
    {
        Camera = camera;
    }

    public Vector3 GetPlayerInput()
    {
        return movementInput;
    }

    public bool GetIsRunning()
    {
        return isRunning;
    }

    void SetInputActions()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.playerInputActions.Player.Move.performed += (context) => movementInput = context.ReadValue<Vector2>();
        playerInput.playerInputActions.Player.Move.canceled += (context) => movementInput = Vector2.zero;
        playerInput.playerInputActions.Player.Jump.performed += (context) => jumpInput = true;
        playerInput.playerInputActions.Player.Jump.canceled += (context) => jumpInput = false;
        playerInput.playerInputActions.Player.Crouch.performed += (context) => crouchInput = true;
        playerInput.playerInputActions.Player.Crouch.canceled += (context) => crouchInput = false;
        playerInput.playerInputActions.Player.Run.performed += (context) => runInput = true;
        playerInput.playerInputActions.Player.Run.canceled += (context) => runInput = false;
    }

    void Start()
    {
        SetInputActions();
        characterController = GetComponent<CharacterController>();
        originalHeight = characterController.height;
        originalCenter = characterController.center;
    }
    
    Vector3 ProcessInputRelativeToCamera(Vector2 input, Transform cameraTransform)
    {
        if(cameraTransform == null)
        {
            return input;
        }

        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.transform.right;

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

        Vector3 move = ProcessInputRelativeToCamera(movementInput, Camera.transform);
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
            characterController.height = originalHeight + crouchHeightOffset;
            characterController.center = new Vector3(originalCenter.x, originalCenter.y + crouchCenterOffset, originalCenter.z);
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
        float offset = 0.1f;
        top += Vector3.up * offset;
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
