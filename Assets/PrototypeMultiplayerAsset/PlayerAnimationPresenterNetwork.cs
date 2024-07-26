using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationPresenterNetwork : PlayerAnimationPresenterAbstract
{
     void Update()
    {
        if(!IsOwner) return;
        LocomotionAnimation();
        CrouchAnimation();
        JumpAnimation();
    }
}
