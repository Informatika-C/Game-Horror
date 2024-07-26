using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    int playerXHash;
    int playerYHash;
    int velocityHash;
    int isCrouchHash;
    int isJumpHash;
    int isFallHash;
    int isGroundHash;
    Animator animator;

    public void SetAnimator(Animator animator)
    {
        this.animator = animator;
    }

    void Awake()
    {
        playerXHash = Animator.StringToHash("PlayerX");
        playerYHash = Animator.StringToHash("PlayerY");
        velocityHash = Animator.StringToHash("Velocity");
        isCrouchHash = Animator.StringToHash("IsCrouch");
        isJumpHash = Animator.StringToHash("IsJump");
        isFallHash = Animator.StringToHash("IsFall");
        isGroundHash = Animator.StringToHash("IsGround");
    }

    public void SetLocomotionVelocity(Vector2 playerVelocity)
    {
        float playerX = playerVelocity.x;
        float playerY = playerVelocity.y;
        animator.SetFloat(playerXHash, playerX);
        animator.SetFloat(playerYHash, playerY);
    }

    public void SetIsCrouch(bool isCrouch)
    {
        animator.SetBool(isCrouchHash, isCrouch);
    }

    public void SetIsJump(bool isJump)
    {
        animator.SetBool(isJumpHash, isJump);
    }

    public void SetIsFall(bool isFall)
    {
        animator.SetBool(isFallHash, isFall);
    }

    public void SetIsGround(bool isGrounded)
    {
        animator.SetBool(isGroundHash, isGrounded);
    }

    public void SetVelocity(float velocity)
    {
        animator.SetFloat(velocityHash, velocity);
    }
}
