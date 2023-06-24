using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonDoDamage : EnemyDoDamage
{
    public override void Attack()
    {
        switch (_sequence)
        {
            case 1:
                FirstAttack();
                break;
            case 2:
                SecondAttack();
                break;
            case 3:
                ThirdAttack();
                break;
        }
    }

    private void FirstAttack()
    {

    }

    private void SecondAttack()
    {

    }

    private void ThirdAttack()
    {

    }
}
