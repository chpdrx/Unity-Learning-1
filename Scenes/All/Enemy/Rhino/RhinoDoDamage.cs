using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhinoDoDamage : EnemyDoDamage
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
        _anim.SetBool("Attack", true);
        DoDamage();
        _sequence = 2;
        StartCoroutine(AnimationOff());
    }

    private void SecondAttack()
    {
        _anim.SetBool("Attack", true);
        DoDamage();
        _sequence = 3;
        StartCoroutine(AnimationOff());
    }

    private void ThirdAttack()
    {
        _anim.SetBool("Skill", true);
        DoDamage();
        _sequence = 1;
        StartCoroutine(AnimationOff());
    }

    private IEnumerator AnimationOff()
    {
        yield return new WaitForSeconds(0.2f);
        _anim.SetBool("Attack", false);
        _anim.SetBool("Skill", false);
    }
}
