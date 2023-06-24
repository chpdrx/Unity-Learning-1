using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GobRangerSkillDoDamage : EnemyProjectileDamage
{
    public override IEnumerator ReturnProjectile(float cd)
    {
        yield return new WaitForSeconds(cd);
        GobRangerPool.Instance.ReturnToPull(this);
    }

    public override void HitAnimation()
    {
        _hitprefab.Play();
    }
}
