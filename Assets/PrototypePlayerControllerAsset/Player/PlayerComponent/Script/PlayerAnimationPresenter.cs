public class PlayerAnimationPresenter : PlayerAnimationPresenterAbstract
{
    void Update()
    {
        if(!playerAnimation) return;
        if(!playerMovement) return;
        LocomotionAnimation();
        CrouchAnimation();
        JumpAnimation();
    }
}