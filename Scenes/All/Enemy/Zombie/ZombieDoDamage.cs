using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieDoDamage : EnemyDoDamage
{
    public override void Attack()
    {

        switch (_sequence)
        {
            case 1:
                FirstAttack();
                break;
        }
    }

    private void FirstAttack()
    {
        _anim.SetBool("Attack", true);
        DoDamage();
        StartCoroutine(AnimationOff());
    }

    private IEnumerator AnimationOff()
    {
        yield return new WaitForSeconds(0.2f);
        _anim.SetBool("Attack", false);
    }
}
