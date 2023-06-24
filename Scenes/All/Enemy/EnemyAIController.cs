using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyAIController : MonoBehaviour
{
    // navmeshagent ��� �������� ai
    private NavMeshAgent navMeshAgent;
    // ���� ������
    [SerializeField] private LayerMask playerLayer;
    // ���� ����������� ��� ������ ai
    [SerializeField] private LayerMask obstacleMask;
    // �������� ����
    public Animator _animator;
    // waypoints �� ������� ����� ����
    [SerializeField] private Transform[] waypoints;
    // ��������� ����������� ������
    [SerializeField] private float viewRadius = 10;
    [SerializeField] private float attackRange = 2;
    [SerializeField] private float viewAngle = 90;
    [SerializeField] private float stayTime = 4;
    [SerializeField] private float speedRun = 4;
    [SerializeField] private float speedWalk = 2;

    // ���������� ��� ���������� �� ���������� � �� �������
    private float _stayTime;
    public bool playerInRange;
    private Vector3 PlayerPosition;
    private GameObject _player;
    protected int currentWaypointIndex = 0;
    public bool isAttacking = false;

    // ������ ai
    private bool _isdie = false;

    private void Start()
    {
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        _player = GameObject.FindGameObjectWithTag("Player");
        _animator = gameObject.GetComponent<Animator>();
        _stayTime = stayTime;
    }

    private void Update()
    {
        if (!_isdie)
        {
            // �������� ������ �� ���� ������
            EnviromentView();
            // ���� ������, �� ������ � ������, ����� ���������� �������������
            if (playerInRange)
            {
                Chasing();
            }
            else
            {
                Patroling();
            }
        }
    }

    // ���������� ���� �� ��������� � ����������
    public virtual void Patroling()
    {
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (_stayTime <= 0)
            {
                NextPoint();
                Move(speedWalk);
                _stayTime = stayTime;
            }
            else
            {
                Stop();
                _stayTime -= Time.deltaTime;
            }
        }
    }

    // ������������� ������ ��� �����������
    private void Chasing()
    {
        Move(speedRun);
        navMeshAgent.SetDestination(PlayerPosition);
        _animator.SetBool("isChasing", true);
        // ���� ����� ������ ������� ���������, �� ���������� ������
        if (Vector3.Distance(transform.position, _player.transform.position) >= viewRadius)
        {
            playerInRange = false;
        }
        // ���� ����� ����� ������� �����, �� ���������
        if (Vector3.Distance(transform.position, _player.transform.position) <= attackRange)
        {
            isAttacking = true;
            transform.LookAt(_player.transform.position);
            _animator.SetBool("isPatroling", false);
            _animator.SetBool("isChasing", false);
        }
        else
            isAttacking = false;
    }

    // ����� ���������� ���������
    public virtual void NextPoint()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    // ����������� ��� ���������� ��������� ��� ������ ������
    public virtual void Stop()
    {
        _animator.SetBool("isPatroling", false);
        _animator.SetBool("isChasing", false);
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }

    // ������������� ��������
    public virtual void Move(float speed)
    {
        _animator.SetBool("isPatroling", true);
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    // �������� ����� �� ��� ������ ����� �����
    public virtual void EnviromentView()
    {
        // ��������� ������ ����� ��� ����������� �������� �������, playerInRange = true
        Collider[] playerDetect = Physics.OverlapSphere(transform.position, viewRadius, playerLayer);

        if (playerDetect.Length != 0)
        {
            Transform player = playerDetect[0].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);
                //���� ����� � ������� �����������, �� �� ���� ���� ����������� ���� obstacleMask, �� ����� �� ���������, ����� ���������.
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    playerInRange = true;   
                }
                else
                {
                    playerInRange = false;
                }
            }
            // ���� ����� ���� �� ������ �����������, �� ����� �� ���������
            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {
                playerInRange = false; 
            }
            if (playerInRange)
            {
                PlayerPosition = player.transform.position;
            }
        }
    }

    public void AiDeath()
    {
        _isdie = true;
        isAttacking = false;
        navMeshAgent.isStopped = true;
        _animator.SetBool("isPatroling", false);
        _animator.SetBool("isChasing", false);
    }
}
