using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollDoDamage : EnemyDoDamage
{
    public override void Attack()
    {
        switch (_sequence)
        {
            case 1:
                FirstAttack();
                break;
            case 2:
                FirstAttack();
                break;
            case 3:
                SecondAttack();
                break;
        }
    }

    private void FirstAttack()
    {
        _anim.SetBool("Attack1", true);
        DoDamage();
        _sequence += 1;
        StartCoroutine(AnimationOff());
    }

    private void SecondAttack()
    {
        _anim.SetBool("Attack2", true);
        DoDamage();
        _sequence = 1;
        StartCoroutine(AnimationOff());
    }

    private IEnumerator AnimationOff()
    {
        yield return new WaitForSeconds(0.2f);
        _anim.SetBool("Attack1", false);
        _anim.SetBool("Attack2", false);
    }
}
