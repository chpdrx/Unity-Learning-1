using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.VFX;

public class WarriorDoDamage : PlayerDamage
{
    // радиус атаки скилла
    [SerializeField] private float _skillPointRadius = 2f;
    // количество найденных пересечений радиуса атаки скилла и противника
    [SerializeField] private int _skilldamagableFound;
    // префаб скилла
    [SerializeField] public GameObject skill_prefab;

    public override void BaseUpdate()
    {
        base.BaseUpdate();
        _skilldamagableFound = Physics.OverlapSphereNonAlloc(_damagePoint.position, _skillPointRadius, _weapon_colliders, _damagableMask);
    }

    // отрисовывает радиус коллайдеров точек взаимодействия, просто для визуального удобства.
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(_damagePoint.position, _skillPointRadius);
    }

    // взаимодействие по левой кнопки мыши с объектами слоя Damagable
    public override void DamageInteract()
    {
        if (_damagableFound > 0)
        {
            var _interactable = _weapon_colliders[0].GetComponent<IEnemyInteractable>();

            if (_interactable != null)
            {
                _interactable.EnemyInteractable(this, StatHolder.Current_Damage);
                BuffChecker();
            }
        }
        else
        {
            if (_interactable != null) _interactable = null;
        }
        LmbAnimation();
        StartCoroutine(LmbCooldown());
    }

    // взаимодействие по правой кнопке мыши с объектами слоя Damagable
    public override void SkillInteract()
    {
        StartCoroutine(SkillCooldown());
        if (_skilldamagableFound > 0)
        {
            var _interactable = _weapon_colliders[0].GetComponent<IEnemyInteractable>();

            if (_interactable != null)
            {
                _interactable.EnemyInteractable(this, StatHolder.Skill_Damage);
                BuffChecker();

            }
        }
        else
        {
            if (_interactable != null) _interactable = null;
        }
    }

    // кулдаун скилла
    public override IEnumerator SkillCooldown()
    {
        canCastInteract = false;
        _input.rmb = false;
        _animator.SetBool("ClassSkill", true);
        yield return new WaitForSeconds(0.5f);
        SkillAnimation();
        yield return new WaitForSeconds(0.5f);
        _animator.SetBool("ClassSkill", false);
        StartMoving();
        yield return new WaitForSeconds(StatHolder.SkillCD);
        _input.rmb = false;
        canCastInteract = true;
    }

    private void LmbAnimation()
    {

    }

    private void SkillAnimation()
    {
        skill_prefab.GetComponent<VisualEffect>().playRate = 0.5f;
        skill_prefab.GetComponent<VisualEffect>().Play();
    }
}