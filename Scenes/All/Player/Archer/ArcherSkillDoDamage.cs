using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSkillDoDamage : ProjectileDamage
{
    private void Start()
    {
        _currentdamage = StatHolder.Skill_Damage;
    }

    public override IEnumerator ReturnProjectile(float cd)
    {
        yield return new WaitForSeconds(cd);
        ArcherSkillPool.Instance.ReturnToPull(this);
    }

    public override void HitAnimation()
    {
        _hitprefab.Play();
    }
}