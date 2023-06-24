using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyProjectileDamage : EnemyDamage
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
    // интерфейс для взаимодействий
    public IEnemyInteractable _interactable;
    // префаб анимации удара
    public ParticleSystem _hitprefab;
    // один выстрел - один удар
    public bool _canDoDamage = true;
    // урон скилла
    public float _skilldamage;

    private void OnEnable()
    {
        _canDoDamage = true;
        _hitprefab.Pause();
        // уничтожение по времени существования объекта
        StartCoroutine(ReturnProjectile(3f));
    }

    private void Update()
    {
        // в переменные записывается количество пересечений созданного коллайдера точки и объектов, с которыми возможно взаимодействие
        _damagableFound = Physics.OverlapSphereNonAlloc(_damagePoint.position, _weaponPointRadius, _weapon_colliders, _damagableMask);
        DamageInteract();
    }

    public virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_damagePoint.position, _weaponPointRadius);
    }

    public virtual void DamageInteract()
    {
        if (_damagableFound > 0 && _canDoDamage)
        {
            var _interactable = _weapon_colliders[0].GetComponent<IPlayerInteractable>();

            if (_interactable != null)
            {
                _canDoDamage = false;
                _interactable.DamageInteractable(this, _skilldamage);
                gameObject.GetComponent<Rigidbody>().velocity = transform.forward * 0;
                HitAnimation();
                StartCoroutine(ReturnProjectile(0.5f));
            }
        }
        else
        {
            if (_interactable != null) _interactable = null;
        }
    }

    public abstract IEnumerator ReturnProjectile(float cd);

    public abstract void HitAnimation();
}
