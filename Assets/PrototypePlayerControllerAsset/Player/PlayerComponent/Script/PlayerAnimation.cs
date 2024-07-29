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
        if(playerXHash == 0) return;
        if(playerYHash == 0) return;
        float playerX = playerVelocity.x;
        float playerY = playerVelocity.y;
        animator.SetFloat(playerXHash, playerX);
        animator.SetFloat(playerYHash, playerY);
    }

    public void SetIsCrouch(bool isCrouch)
    {
        if(isCrouchHash == 0) return;
        animator.SetBool(isCrouchHash, isCrouch);
    }

    public void SetIsJump(bool isJump)
    {
        if(isJumpHash == 0) return;
        animator.SetBool(isJumpHash, isJump);
    }

    public void SetIsFall(bool isFall)
    {
        if(isFallHash == 0) return;
        animator.SetBool(isFallHash, isFall);
    }

    public void SetIsGround(bool isGrounded)
    {
        if(isGroundHash == 0) return;
        animator.SetBool(isGroundHash, isGrounded);
    }

    public void SetVelocity(float velocity)
    {
        if(velocityHash == 0) return;
        animator.SetFloat(velocityHash, velocity);
    }

    public void SetCameraY(float cameraY)
    {
        if(cameraYHash == 0) return;
        animator.SetFloat(cameraYHash, cameraY);
    }

    public void SetHeadStandWeight(float weight)
    {
        if(headStandLayerIndex == 0) return;
        animator.SetLayerWeight(headStandLayerIndex, weight);
    }

    public void SetHeadCrouchWeight(float weight)
    {
        if(headCrouchLayerIndex == 0) return;
        animator.SetLayerWeight(headCrouchLayerIndex, weight);
    }

    public void SetHandFlashStandWeight(float weight)
    {
        if(handFlashStandLayerIndex == 0) return;
        animator.SetLayerWeight(handFlashStandLayerIndex, weight);
    }

    public void SetHandFlashCrouchWeight(float weight)
    {
        if(handFlashCrouchLayerIndex == 0) return;
        animator.SetLayerWeight(handFlashCrouchLayerIndex, weight);
    }
}
