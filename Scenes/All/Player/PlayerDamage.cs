using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;
using UnityEngine.VFX;

public abstract class PlayerDamage : DoDamage
{
    // объекты, от которых идёт взаимодействие (привязаны к персонажу)
    [SerializeField] public Transform _damagePoint;
    // радиус этих объектов
    [SerializeField] public float _weaponPointRadius = 0.4f;
    // слои объектов, с которыми взаимодействуют точки выше
    [SerializeField] public LayerMask _damagableMask;
    // список пересечений коллайдеров точки и объекта взаимодействия
    public readonly Collider[] _weapon_colliders = new Collider[3];
    // количество найденных пересечений коллайдеров точки и объекта взаимодействия
    [SerializeField] public int _damagableFound;
    //таймаут на взаимодействие
    public bool canDamageInteract = true;
    public bool canCastInteract = true;

    // интерфейс для взаимодействий
    public IEnemyInteractable _interactable;

    // ввод, из Input System
    public StarterAssetsInputs _input;

    // аниматор персонажа
    [SerializeField] public Animator _animator;

    // последовательность обычных аттак
    public float _attacksequence = 0.0f;

    // сохранение скорости персонажа
    public float _speed;

    private void Start()
    {
        _input = GetComponent<StarterAssetsInputs>();
    }

    private void Update()
    {
        BaseUpdate();
    }

    public virtual void BaseUpdate()
    {
        // в переменные записывается количество пересечений созданного коллайдера точки и объектов, с которыми возможно взаимодействие
        _damagableFound = Physics.OverlapSphereNonAlloc(_damagePoint.position, _weaponPointRadius, _weapon_colliders, _damagableMask);

        // Вызов функций, которые проверяют наличие взаимодействий и делают что то при возникновании его.
        if (_input.lmb && canDamageInteract && !_animator.GetBool("Dash"))
        {
            StopMoving();
            AttackSequence();
            DamageInteract();
        }
        else if (_input.lmb && !canDamageInteract) _input.lmb = false;

        // то же самое для пкма
        if (_input.rmb && canCastInteract && !_animator.GetBool("Dash"))
        {
            StopMoving();
            SkillInteract();
        }
        else if (_input.rmb && !canCastInteract) _input.rmb = false;
    }

    // отрисовывает радиус коллайдеров точек взаимодействия, просто для визуального удобства.
    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_damagePoint.position, _weaponPointRadius);
    }

    private void AttackSequence()
    {
        _animator.SetBool("Attack", true);
        if (_attacksequence >= 2.0f) _attacksequence = 0.0f;
        else _attacksequence += 1.0f;
        _animator.SetFloat("AttackCounter", _attacksequence);
    }

    public abstract void DamageInteract();

    public abstract void SkillInteract();

    // кулдаун атак
    public IEnumerator LmbCooldown()
    {
        canDamageInteract = false;
        _input.lmb = false;
        yield return new WaitForSeconds(0.7f);
        _animator.SetBool("Attack", false);
        StartMoving();
        yield return new WaitForSeconds(StatHolder.Haste);
        _input.lmb = false;
        canDamageInteract = true;
    }

    // кулдаун скилла
    public virtual IEnumerator SkillCooldown()
    {
        canCastInteract = false;
        _input.rmb = false;
        _animator.SetBool("ClassSkill", true);
        yield return new WaitForSeconds(1f);
        _animator.SetBool("ClassSkill", false);
        StartMoving();
        yield return new WaitForSeconds(StatHolder.SkillCD);
        _input.rmb = false;
        canCastInteract = true;
    }

    public virtual void StopMoving()
    {
        _speed = StatHolder.Speed;
        StatHolder.Speed = 0;
    }

    public virtual void StartMoving()
    {
        StatHolder.Speed = _speed;
    }

    // шансовые бафы от предметов
    public void BuffChecker()
    {
        if (StatHolder.HasteBuff == true)
        {
            HasteUp(StatHolder.HasteChance, StatHolder.HasteValue);
        }
        if (StatHolder.CdBuff == true)
        {
            CdUp(StatHolder.CdChance, StatHolder.CdValue);
        }
        if (StatHolder.DamageUp == true)
        {
            ChanceDamageUp(StatHolder.DamageUpChance, StatHolder.DamageUpValue);
        }
    }

    public void HasteUp(float chance, float value)
    {
        if (Random.Range(0, 1.0f) <= chance)
        {
            var _buffer = (StatHolder.Haste / 100) * value;
            StatHolder.Haste -= _buffer;
            StartCoroutine(HasteBuffDuration(_buffer));
        }
    }

    IEnumerator HasteBuffDuration(float stat)
    {
        yield return new WaitForSeconds(10.0f);
        StatHolder.Haste += stat;
    }

    public void ChanceDamageUp(float chance, float value)
    {
        if (Random.Range(0, 1.0f) <= chance)
        {
            StatHolder.Damage += value;
        }
    }

    public void CdUp(float chance, float value)
    {
        if (Random.Range(0, 1.0f) <= chance)
        {
            var _buffer = (StatHolder.SkillCD / 100) * value;
            StatHolder.SkillCD -= _buffer;
            StartCoroutine(CdBuffDuration(_buffer));
        }
    }

    IEnumerator CdBuffDuration(float stat)
    {
        yield return new WaitForSeconds(10.0f);
        StatHolder.SkillCD += stat;
    }
}
