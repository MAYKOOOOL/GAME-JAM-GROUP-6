using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AIPatrolAndChase : MonoBehaviour
{
    public Transform[] waypoints;
    public float detectionRange = 10f;
    public float fieldOfView = 120f;
    public Transform player;

    public float pauseAtWaypoint = 3f;
    public float lookAroundAngle = 45f;

    public float patrolSpeed = 3.5f;
    public float chaseSpeedMultiplier = 2f; // Multiplier when chasing player

    private NavMeshAgent agent;
    private int currentWaypoint = 0;
    private bool isChasing = false;
    private bool playerInTrigger = false; // NEW: track if player is inside trigger collider
    private Coroutine pauseCoroutine;

    private Quaternion initialRotation;

    private bool isInvestigating = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;
        GoToNextWaypoint();
    }

    void Update()
    {
        // Chase if player is in trigger or seen by vision cone
        if (playerInTrigger || CanSeePlayer())
        {
            if (!isChasing)
            {
                isChasing = true;
                agent.speed = patrolSpeed * chaseSpeedMultiplier;

                if (pauseCoroutine != null)
                {
                    StopCoroutine(pauseCoroutine);
                    pauseCoroutine = null;
                    agent.isStopped = false;
                }
                isInvestigating = false;
            }
            agent.SetDestination(player.position);
        }
        else if (isChasing)
        {
            isChasing = false;
            agent.speed = patrolSpeed;
            GoToNextWaypoint();
        }

        if (!isChasing && !agent.pathPending && agent.remainingDistance < 0.5f && pauseCoroutine == null && !isInvestigating)
        {
            pauseCoroutine = StartCoroutine(PauseAndLookAround());
        }
    }

    bool CanSeePlayer()
    {
        Vector3 directionToPlayer = player.position - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);

        if (directionToPlayer.magnitude <= detectionRange && angle <= fieldOfView * 0.5f)
        {
            Ray ray = new Ray(transform.position + Vector3.up, directionToPlayer.normalized);
            if (Physics.Raycast(ray, out RaycastHit hit, detectionRange))
            {
                if (hit.transform == player)
                    return true;
            }
        }
        return false;
    }

    void GoToNextWaypoint()
    {
        if (waypoints.Length == 0) return;
        agent.destination = waypoints[currentWaypoint].position;
        currentWaypoint = (currentWaypoint + 1) % waypoints.Length;
    }

    public void HearNoise(Vector3 soundPos)
    {
        if (isChasing || isInvestigating) return;

        float dist = Vector3.Distance(transform.position, soundPos);
        if (dist <= detectionRange)
        {
            Debug.Log("Sound heard! Investigating...");
            StartCoroutine(PauseAndInvestigate(soundPos));
        }
    }

    private IEnumerator PauseAndInvestigate(Vector3 soundPos)
    {
        isInvestigating = true;
        agent.isStopped = true;

        Vector3 direction = (soundPos - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(direction);

        yield return new WaitForSeconds(3f);

        agent.isStopped = false;
        agent.SetDestination(soundPos);

        while (agent.pathPending || agent.remainingDistance > 0.5f)
        {
            if (CanSeePlayer())
            {
                isInvestigating = false;
                yield break;
            }
            yield return null;
        }

        isInvestigating = false;
        GoToNextWaypoint();
    }

    private IEnumerator PauseAndLookAround()
    {
        agent.isStopped = true;

        initialRotation = transform.rotation;

        float halfPause = pauseAtWaypoint / 2f;
        float elapsed = 0f;

        while (elapsed < halfPause)
        {
            transform.rotation = Quaternion.Slerp(initialRotation, initialRotation * Quaternion.Euler(0, -lookAroundAngle, 0), elapsed / halfPause);
            elapsed += Time.deltaTime;
            yield return null;

            if (CanSeePlayer())
            {
                break;
            }
        }

        elapsed = 0f;

        while (elapsed < halfPause)
        {
            transform.rotation = Quaternion.Slerp(initialRotation * Quaternion.Euler(0, -lookAroundAngle, 0), initialRotation * Quaternion.Euler(0, lookAroundAngle, 0), elapsed / halfPause);
            elapsed += Time.deltaTime;
            yield return null;

            if (CanSeePlayer())
            {
                break;
            }
        }

        if (!CanSeePlayer())
        {
            float returnTime = 0.5f;
            elapsed = 0f;
            Quaternion fromRotation = transform.rotation;
            while (elapsed < returnTime)
            {
                transform.rotation = Quaternion.Slerp(fromRotation, initialRotation, elapsed / returnTime);
                elapsed += Time.deltaTime;
                yield return null;
                if (CanSeePlayer())
                {
                    break;
                }
            }
            transform.rotation = initialRotation;
        }

        agent.isStopped = false;
        pauseCoroutine = null;

        if (!isChasing && !isInvestigating)
            GoToNextWaypoint();
    }

    // New trigger event handlers
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            playerInTrigger = false;
        }
    }
}
