using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    int playerXHash;
    int playerYHash;
    int isCrouchHash;
    int isJumpHash;
    int isFallHash;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        
        playerXHash = Animator.StringToHash("PlayerX");
        playerYHash = Animator.StringToHash("PlayerY");
        isCrouchHash = Animator.StringToHash("IsCrouch");
        isJumpHash = Animator.StringToHash("IsJump");
        isFallHash = Animator.StringToHash("IsFall");
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
}
