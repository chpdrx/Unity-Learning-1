using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonAI : EnemyAIController
{
    public override void Move(float speed)
    {
        base.Move(speed);
        _animator.SetBool("isSleep", false);
    }

    public override void Stop()
    {
        base.Stop();
        _animator.SetBool("isSleep", true);
    }
}

