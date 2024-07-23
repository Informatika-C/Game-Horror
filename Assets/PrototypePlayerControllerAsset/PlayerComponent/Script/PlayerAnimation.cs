using System;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    int playerXHash;
    int playerYHash;
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        
        playerXHash = Animator.StringToHash("PlayerX");
        playerYHash = Animator.StringToHash("PlayerY");
    }

    public void SetLocomotionVelocity(Vector2 playerVelocity)
    {
        float playerX = playerVelocity.x;
        float playerY = playerVelocity.y;
        animator.SetFloat(playerXHash, playerX);
        animator.SetFloat(playerYHash, playerY);
    }
}
