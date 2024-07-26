using Unity.Netcode;
using UnityEngine;

public class PlayerAnimationPresenterAbstract : NetworkBehaviour
{
    protected PlayerAnimation playerAnimation;
    protected PlayerMovement playerMovement;

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

    protected void LocomotionAnimation()
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
        playerAnimation.SetVelocity(currentInput.magnitude);
    }

    protected void CrouchAnimation()
    {
        bool isCrouch = playerMovement.GetIsCrouch();
        playerAnimation.SetIsCrouch(isCrouch);
    }

    protected void JumpAnimation()
    {
        playerAnimation.SetIsJump(playerMovement.GetIsJumping());
        playerAnimation.SetIsFall(playerMovement.GetIsFalling());
        playerAnimation.SetIsGround(playerMovement.GetIsGrounded());
    }
}
