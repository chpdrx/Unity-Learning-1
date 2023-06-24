using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WagonRun : MonoBehaviour
{
    [SerializeField] public NavMeshAgent navMeshAgent;
    // waypoints по которым ходит враг
    [SerializeField] public Transform[] waypoints;
    // параметры обнаружения врагом
    [SerializeField] public float speedWalk = 2;
    protected int currentWaypointIndex = 0;
    Vector3 velocity = Vector3.zero;

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.updatePosition = false;
    }

    private void Update()
    {
        Patroling();
    }

    private void Patroling()
    {
        if (navMeshAgent.speed == 0)
        {
            navMeshAgent.speed = speedWalk;
        }
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
        transform.position = Vector3.SmoothDamp(transform.position, navMeshAgent.nextPosition, ref velocity, 1.5f);
        //_animator.SetBool("isPatroling", true);
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            NextPoint();
            Move(speedWalk);
        }
    }

    public virtual void NextPoint()
    {
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
    }

    void Move(float speed)
    {
        navMeshAgent.speed = speed;
    }
}

