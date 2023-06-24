using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobRangerDoDamage : EnemyDoDamage
{
    [SerializeField] private GameObject cast_point;

    private Vector3 _player;

    public override void Attack()
    {
        canAttack = false;
        _anim.SetBool("Attack", true);
        _player = GameObject.FindGameObjectWithTag("Player").transform.position.normalized;
        StartCoroutine(AnimationOff());
    }

    private IEnumerator AnimationOff()
    {
        yield return new WaitForSeconds(0.3f);
        var ball = GobRangerPool.Instance.Get();
        ball.transform.SetPositionAndRotation(cast_point.transform.position, transform.rotation);
        ball.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.7f);
        ball.GetComponent<Rigidbody>().velocity = transform.forward * 20;
        _anim.SetBool("Attack", false);
        yield return new WaitForSeconds(haste);
        canAttack = true;
    }
}
