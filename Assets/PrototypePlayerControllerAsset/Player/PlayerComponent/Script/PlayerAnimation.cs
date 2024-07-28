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
    int cameraYHash;
    int headStandLayerIndex;
    int headCrouchLayerIndex;
    int handFlashStandLayerIndex;
    int handFlashCrouchLayerIndex;
    Animator animator;

    public void SetAnimator(Animator animator)
    {
        this.animator = animator;
    }

    void Start()
    {
        playerXHash = Animator.StringToHash("PlayerX");
        playerYHash = Animator.StringToHash("PlayerY");
        velocityHash = Animator.StringToHash("Velocity");
        isCrouchHash = Animator.StringToHash("IsCrouch");
        isJumpHash = Animator.StringToHash("IsJump");
        isFallHash = Animator.StringToHash("IsFall");
        isGroundHash = Animator.StringToHash("IsGround");
        cameraYHash = Animator.StringToHash("CameraY");
        headStandLayerIndex = animator.GetLayerIndex("HeadStand");
        headCrouchLayerIndex = animator.GetLayerIndex("HeadCrouch");
        handFlashStandLayerIndex = animator.GetLayerIndex("HandFlashStand");
        handFlashCrouchLayerIndex = animator.GetLayerIndex("HandFlashCrouch");
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

    public void SetCameraY(float cameraY)
    {
        animator.SetFloat(cameraYHash, cameraY);
    }

    public void SetHeadStandWeight(float weight)
    {
        animator.SetLayerWeight(headStandLayerIndex, weight);
    }

    public void SetHeadCrouchWeight(float weight)
    {
        animator.SetLayerWeight(headCrouchLayerIndex, weight);
    }

    public void SetHandFlashStandWeight(float weight)
    {
        animator.SetLayerWeight(handFlashStandLayerIndex, weight);
    }

    public void SetHandFlashCrouchWeight(float weight)
    {
        animator.SetLayerWeight(handFlashCrouchLayerIndex, weight);
    }
}
