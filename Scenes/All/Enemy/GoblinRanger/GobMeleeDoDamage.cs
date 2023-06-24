using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobMeleeDoDamage : EnemyDoDamage
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
        }
    }

    private void FirstAttack()
    {
        _anim.SetBool("Attack", true);
        DoDamage();
        _sequence = 2;
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
        _anim.SetBool("Attack", false);
        _anim.SetBool("Attack2", false);
    }
}
