using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDoDamage : EnemyDoDamage
{
    public bool _canBuff;

    public override void Attack()
    {
        if ((gameObject.GetComponent<EnemyTake>().health <= _health / 4) && _canBuff) _sequence = 3;

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

    private void ThirdAttack()
    {
        _canBuff = false;
        _anim.SetBool("Skill", true);
        damage *= 2;
        haste /= 2;
        _sequence = 1;
        StartCoroutine(AnimationOff());
    }

    private IEnumerator AnimationOff()
    {
        yield return new WaitForSeconds(0.2f);
        _anim.SetBool("Attack", false);
        _anim.SetBool("Attack2", false);
        _anim.SetBool("Skill", false);
    }
}
