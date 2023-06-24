using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyDoDamage : EnemyDamage
{
    // точка, от которой идёт взаимодействие для нанесения урона
    [SerializeField] private Transform _damagePoint;
    // радиус этой точки
    [SerializeField] private float _weaponPointRadius = 0.4f;
    // слои объектов, с которыми взаимодействует точка
    [SerializeField] private LayerMask _damagableMask;
    // список пересечений коллайдеров точки и объекта взаимодействия
    private readonly Collider[] _weapon_colliders = new Collider[3];
    // количество найденных пересечений коллайдеров точки и объекта взаимодействия
    [SerializeField] private int _damagableFound;

    // интерфейс для взаимодействий
    private IPlayerInteractable _interactable;

    // статы моба
    [SerializeField] public float damage = 10.0f;
    [SerializeField] public float haste = 1.6f;
    public float _health;
    // для кулдауна атаки
    public bool canAttack = true;
    // класс патрулирования
    private EnemyAIController _ai;

    // аниматор моба
    public Animator _anim;
    // последовательность атак
    public int _sequence = 1;

    private void Start()
    {
        _ai = gameObject.GetComponent<EnemyAIController>();
        _anim = gameObject.GetComponent<Animator>();
        _health = gameObject.GetComponent<EnemyTake>().health;
    }

    private void Update()
    {
        // в переменные записывается количество пересечений созданного коллайдера точки и объектов, с которыми возможно взаимодействие
        _damagableFound = Physics.OverlapSphereNonAlloc(_damagePoint.position, _weaponPointRadius, _weapon_colliders, _damagableMask);
        
        if (_ai.isAttacking)
        {
            if (canAttack) Attack();
        }
    }

    // отрисовывает радиус коллайдеров точек взаимодействия, просто для визуального удобства.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_damagePoint.position, _weaponPointRadius);
    }

    // ротация моба
    public abstract void Attack();

    // если было пересечение коллайдера атаки с игроком, то наносить урон
    public void DoDamage()
    {
        if (_damagableFound > 0)
        {
            var _interactable = _weapon_colliders[0].GetComponent<IPlayerInteractable>();

            if (_interactable != null)
            {
                StartCoroutine(AttackCooldown(_interactable));
            }
        }
        else
        {
            if (_interactable != null) _interactable = null;
        }
    }

    private IEnumerator AttackCooldown(IPlayerInteractable player)
    {
        canAttack = false;
        player.DamageInteractable(this, damage);
        yield return new WaitForSeconds(haste);
        canAttack = true;

    }
}
