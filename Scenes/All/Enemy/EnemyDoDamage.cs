using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyDoDamage : EnemyDamage
{
    // �����, �� ������� ��� �������������� ��� ��������� �����
    [SerializeField] private Transform _damagePoint;
    // ������ ���� �����
    [SerializeField] private float _weaponPointRadius = 0.4f;
    // ���� ��������, � �������� ��������������� �����
    [SerializeField] private LayerMask _damagableMask;
    // ������ ����������� ����������� ����� � ������� ��������������
    private readonly Collider[] _weapon_colliders = new Collider[3];
    // ���������� ��������� ����������� ����������� ����� � ������� ��������������
    [SerializeField] private int _damagableFound;

    // ��������� ��� ��������������
    private IPlayerInteractable _interactable;

    // ����� ����
    [SerializeField] public float damage = 10.0f;
    [SerializeField] public float haste = 1.6f;
    public float _health;
    // ��� �������� �����
    public bool canAttack = true;
    // ����� ��������������
    private EnemyAIController _ai;

    // �������� ����
    public Animator _anim;
    // ������������������ ����
    public int _sequence = 1;

    private void Start()
    {
        _ai = gameObject.GetComponent<EnemyAIController>();
        _anim = gameObject.GetComponent<Animator>();
        _health = gameObject.GetComponent<EnemyTake>().health;
    }

    private void Update()
    {
        // � ���������� ������������ ���������� ����������� ���������� ���������� ����� � ��������, � �������� �������� ��������������
        _damagableFound = Physics.OverlapSphereNonAlloc(_damagePoint.position, _weaponPointRadius, _weapon_colliders, _damagableMask);
        
        if (_ai.isAttacking)
        {
            if (canAttack) Attack();
        }
    }

    // ������������ ������ ����������� ����� ��������������, ������ ��� ����������� ��������.
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(_damagePoint.position, _weaponPointRadius);
    }

    // ������� ����
    public abstract void Attack();

    // ���� ���� ����������� ���������� ����� � �������, �� �������� ����
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
