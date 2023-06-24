using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpDoDamage : EnemyDoDamage
{
    [SerializeField] private GameObject cast_point;
    [SerializeField] private ParticleSystem skill_animation;

    public override void Attack()
    {
        canAttack = false;
        _anim.SetBool("Attack", true);
        StartCoroutine(AnimationOff());
    }

    private IEnumerator AnimationOff()
    {
        skill_animation.Play();
        yield return new WaitForSeconds(0.3f);
        var ball = ImpSkillPool.Instance.Get();
        ball.transform.SetPositionAndRotation(cast_point.transform.position, transform.rotation);
        ball.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        ball.GetComponent<Rigidbody>().velocity = transform.forward * 5;
        skill_animation.Pause();
        skill_animation.Clear();
        _anim.SetBool("Attack", false);
        yield return new WaitForSeconds(haste);
        canAttack = true;
    }
}
