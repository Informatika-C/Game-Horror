using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationPresenterNetwork : PlayerAnimationPresenterAbstract
{
     void Update()
    {
        if(!playerAnimation) return;
        if(!playerMovement) return;
        if(!IsOwner) return;
        LocomotionAnimation();
        CrouchAnimation();
        JumpAnimation();
        HeadAnimation();
        HandFlashAnimation();
    }
}
