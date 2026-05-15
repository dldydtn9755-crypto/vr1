using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class WeepingAngel : MonoBehaviour
{
    [Header("Player")]
    public Transform player;
    public Camera playerCamera;

    [Header("Movement")]
    public float moveSpeed = 2.0f;
    public float stopDistance = 1.2f;

    [Header("Vision Check")]
    public float viewAngle = 70f;
    public float viewDistance = 20f;

    [Header("Model Direction Fix")]
    public float modelForwardOffsetY = 90f;

    [Header("Return Home")]
    public bool returnHomeWhenNoPath = true;
    public float homeStopDistance = 0.5f;

    [Header("Death Jump Scare")]
    public GameObject deathScareCanvas;
    public float deathScareDuration = 2.0f;

    [Header("Game Over")]
    public GameObject gameOverCanvas;
    public float killDistance = 1.0f;

    [Header("Kill Safety")]
    public float killStartDelay = 3.0f;

    [Header("Destroy Objects")]
    public GameObject house;

    [Header("Angel Visual Root")]
    public GameObject angelVisualRoot;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip deathScareSound;

    private NavMeshAgent agent;
    private bool isDead = false;
    private bool canKill = false;

    private Vector3 startPosition;
    private Quaternion startRotation;
    private bool isReturningHome = false;

    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        agent = GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.speed = moveSpeed;
            agent.stoppingDistance = stopDistance;
            agent.isStopped = false;
        }

        if (deathScareCanvas != null)
            deathScareCanvas.SetActive(false);

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(false);

        StartCoroutine(EnableKillAfterDelay());
    }

    private IEnumerator EnableKillAfterDelay()
    {
        yield return new WaitForSeconds(killStartDelay);
        canKill = true;
    }

    private void Update()
    {
        if (isDead) return;
        if (player == null || playerCamera == null) return;

        if (isReturningHome)
        {
            CheckReturnHomeArrived();
            return;
        }

        bool isSeen = IsSeenByPlayer();

        if (isSeen)
        {
            StopAngel();
        }
        else
        {
            MoveToPlayer();
        }

        LookAtPlayer();
        CheckKillDistance(isSeen);
    }

    private bool IsSeenByPlayer()
    {
        Vector3 camPos = playerCamera.transform.position;
        Vector3 angelPos = transform.position + Vector3.up * 1.2f;

        Vector3 dir = angelPos - camPos;
        float distance = dir.magnitude;

        if (distance > viewDistance)
            return false;

        dir.Normalize();

        float angle = Vector3.Angle(playerCamera.transform.forward, dir);

        if (angle > viewAngle)
            return false;

        return true;
    }

    private void MoveToPlayer()
    {
        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = false;
            agent.speed = moveSpeed;
            agent.stoppingDistance = stopDistance;

            NavMeshHit hit;

            if (NavMesh.SamplePosition(player.position, out hit, 5f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
            else if (returnHomeWhenNoPath)
            {
                ReturnHome();
            }
        }
    }

    private void StopAngel()
    {
        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }
    }

    private void ReturnHome()
    {
        if (agent == null || !agent.isOnNavMesh) return;

        isReturningHome = true;

        agent.isStopped = false;
        agent.speed = moveSpeed;
        agent.stoppingDistance = homeStopDistance;
        agent.SetDestination(startPosition);
    }

    private void CheckReturnHomeArrived()
    {
        if (!isReturningHome) return;
        if (agent == null || !agent.isOnNavMesh) return;

        float distance = Vector3.Distance(transform.position, startPosition);

        if (distance <= homeStopDistance)
        {
            isReturningHome = false;
            agent.isStopped = true;
            agent.ResetPath();
            transform.rotation = startRotation;
        }
    }

    private void LookAtPlayer()
    {
        Vector3 direction = player.position - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion lookRotation = Quaternion.LookRotation(direction);

        transform.rotation = lookRotation * Quaternion.Euler(0f, modelForwardOffsetY, 0f);
    }

    private void CheckKillDistance(bool isSeen)
    {
        if (!canKill) return;
        if (isSeen) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= killDistance)
        {
            isDead = true;
            StartCoroutine(DeathSequence());
        }
    }

    public void ForceDeath()
    {
        if (isDead) return;

        isDead = true;
        StartCoroutine(DeathSequence());
    }

    private void HideAngelVisual()
    {
        if (angelVisualRoot == null) return;

        Renderer[] renderers = angelVisualRoot.GetComponentsInChildren<Renderer>(true);

        foreach (Renderer r in renderers)
        {
            r.enabled = false;
        }
    }

    private IEnumerator DeathSequence()
    {
        moveSpeed = 0f;

        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = true;
            agent.ResetPath();
        }

        if (house != null)
        {
            Destroy(house);
        }

        HideAngelVisual();

        if (deathScareCanvas != null)
            deathScareCanvas.SetActive(true);

        if (audioSource != null && deathScareSound != null)
            audioSource.PlayOneShot(deathScareSound);

        yield return new WaitForSeconds(deathScareDuration);

        if (deathScareCanvas != null)
            deathScareCanvas.SetActive(false);

        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(true);
    }
}