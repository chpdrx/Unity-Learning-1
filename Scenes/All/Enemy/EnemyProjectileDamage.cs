using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyProjectileDamage : EnemyDamage
{
    // �������, �� ������� ��� �������������� (��������� � ���������)
    [SerializeField] public Transform _damagePoint;
    // ������ ���� ��������
    [SerializeField] public float _weaponPointRadius = 0.4f;
    // ���� ��������, � �������� ��������������� ����� ����
    [SerializeField] public LayerMask _damagableMask;
    // ������ ����������� ����������� ����� � ������� ��������������
    public readonly Collider[] _weapon_colliders = new Collider[3];
    // ���������� ��������� ����������� ����������� ����� � ������� ��������������
    [SerializeField] public int _damagableFound;
    // ��������� ��� ��������������
    public IEnemyInteractable _interactable;
    // ������ �������� �����
    public ParticleSystem _hitprefab;
    // ���� ������� - ���� ����
    public bool _canDoDamage = true;
    // ���� ������
    public float _skilldamage;

    private void OnEnable()
    {
        _canDoDamage = true;
        _hitprefab.Pause();
        // ����������� �� ������� ������������� �������
        StartCoroutine(ReturnProjectile(3f));
    }

    private void Update()
    {
        // � ���������� ������������ ���������� ����������� ���������� ���������� ����� � ��������, � �������� �������� ��������������
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
