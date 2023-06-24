using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class EnemyAIController : MonoBehaviour
{
    // navmeshagent для контроля ai
    private NavMeshAgent navMeshAgent;
    // слой игрока
    [SerializeField] private LayerMask playerLayer;
    // слой препятствия для зрения ai
    [SerializeField] private LayerMask obstacleMask;
    // аниматор моба
    public Animator _animator;
    // waypoints по которым ходит враг
    [SerializeField] private Transform[] waypoints;
    // параметры обнаружения врагом
    [SerializeField] private float viewRadius = 10;
    [SerializeField] private float attackRange = 2;
    [SerializeField] private float viewAngle = 90;
    [SerializeField] private float stayTime = 4;
    [SerializeField] private float speedRun = 4;
    [SerializeField] private float speedWalk = 2;

    // переменные для следования по вейпоинтам и за игроком
    private float _stayTime;
    public bool playerInRange;
    private Vector3 PlayerPosition;
    private GameObject _player;
    protected int currentWaypointIndex = 0;
    public bool isAttacking = false;

    // смерть ai
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
            // Проверка увидел ли враг игрока
            EnviromentView();
            // Если увидел, то бежать к игроку, иначе продолжать патрулировать
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

    // следование моба по вейпонтам с остановкой
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

    // преследование игрока при обнаружении
    private void Chasing()
    {
        Move(speedRun);
        navMeshAgent.SetDestination(PlayerPosition);
        _animator.SetBool("isChasing", true);
        // Если игрок дальше радиуса видимости, то прекращать погоню
        if (Vector3.Distance(transform.position, _player.transform.position) >= viewRadius)
        {
            playerInRange = false;
        }
        // Если игрок ближе радиуса атаки, то атаковать
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

    // выбор следующего вейпоинта
    public virtual void NextPoint()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    // остановится при достижении вейпоинта или потере игрока
    public virtual void Stop()
    {
        _animator.SetBool("isPatroling", false);
        _animator.SetBool("isChasing", false);
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
    }

    // возобновление движения
    public virtual void Move(float speed)
    {
        _animator.SetBool("isPatroling", true);
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    // проверка видит ли моб игрока перед собой
    public virtual void EnviromentView()
    {
        // коллайдер вокруг врага при пересечении которого игроком, playerInRange = true
        Collider[] playerDetect = Physics.OverlapSphere(transform.position, viewRadius, playerLayer);

        if (playerDetect.Length != 0)
        {
            Transform player = playerDetect[0].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);
                //если игрок в радиусе обнаружения, но на пути есть препятствие слоя obstacleMask, то игрок не обнаружен, иначе обнаружен.
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    playerInRange = true;   
                }
                else
                {
                    playerInRange = false;
                }
            }
            // если игрок ушёл за радиус обнаружения, то игрок не обнаружен
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
