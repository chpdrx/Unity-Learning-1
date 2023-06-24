using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpSkillDamage : EnemyProjectileDamage
{
    public override IEnumerator ReturnProjectile(float cd)
    {
        yield return new WaitForSeconds(cd);
        ImpSkillPool.Instance.ReturnToPull(this);
    }

    public override void HitAnimation()
    {
        _hitprefab.Play();
    }
}
