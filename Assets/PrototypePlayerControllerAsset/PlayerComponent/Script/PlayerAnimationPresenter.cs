using UnityEngine;

public class PlayerAnimationPresenter : MonoBehaviour
{
    PlayerAnimation playerAnimation;
    PlayerMovement playerMovement;

    float walkSpeed = 0.5f;
    float runSpeed = 1f;
    float transitionSpeed = 5f;
    Vector2 currentInput = Vector2.zero;

    public void SetPlayerAnimation(PlayerAnimation playerAnimation)
    {
        this.playerAnimation = playerAnimation;
    }

    public void SetPlayerMovement(PlayerMovement playerMovement)
    {
        this.playerMovement = playerMovement;
    }

    void Update()
    {
        LocomotionAnimation();
        CrouchAnimation();
        JumpAnimation();
        FallAnimation();
    }

    void LocomotionAnimation()
    {
        Vector2 playerInput = playerMovement.GetPlayerInput();
        bool isRunning = playerMovement.GetIsRunning();

        Vector2 targetInput = playerInput * (isRunning ? runSpeed : walkSpeed);
        currentInput = Vector2.Lerp(currentInput, targetInput, Time.deltaTime * transitionSpeed);

        if(currentInput.magnitude < 0.01f)
        {
            currentInput = Vector2.zero;
        }

        if(playerMovement.GetIsJumping() || playerMovement.GetIsFalling())
        {
            currentInput = Vector2.zero;
        }
        
        playerAnimation.SetLocomotionVelocity(currentInput);
    }

    void CrouchAnimation()
    {
        bool isCrouch = playerMovement.GetIsCrouch();
        playerAnimation.SetIsCrouch(isCrouch);
    }

    void JumpAnimation()
    {
        bool isJumping = playerMovement.GetIsJumping();
        playerAnimation.SetIsJump(isJumping);
    }

    void FallAnimation()
    {
        bool isFalling = playerMovement.GetIsFalling();
        playerAnimation.SetIsFall(isFalling);
    }
}