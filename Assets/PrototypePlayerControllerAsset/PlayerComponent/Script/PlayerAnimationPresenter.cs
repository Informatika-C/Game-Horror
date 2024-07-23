using UnityEngine;

public class PlayerAnimationPresenter : MonoBehaviour
{
    PlayerAnimation playerAnimation;
    PlayerMovement playerMovement;

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
        playerAnimation.SetLocomotionVelocity(playerMovement.GetPlayerInput());
    }
}