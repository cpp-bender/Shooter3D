using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyManager : Entity
{
    [Header("Enemy Settings")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform target;
    [SerializeField] private EnemySettings enemySettings;
    [SerializeField] private float startingHealth;
    [SerializeField] private Material skinMaterial;

    public enum State
    {
        Idle, Chasing, Attacking
    }

    private State currentState;
    private float attackDistanceThreshold = 1f;
    private float timeBetweenAttacks = .5f;
    private float nextAttackTime;
    private float enemyCollisionRadius;
    private float playerCollisionRadius;
    private Color originalColor;

    private void OnEnable()
    {
        //TODO:Could be a problem
        target = FindObjectOfType<PlayerController>().transform;
    }

    private void OnDisable()
    {
        skinMaterial.color = Color.green;
    }

    private void Start()
    {
        originalColor = skinMaterial.color;
        enemyCollisionRadius = GetComponent<CapsuleCollider>().radius;
        playerCollisionRadius = target.GetComponent<CapsuleCollider>().radius;
        currentState = State.Chasing;
        StartCoroutine(FollowPlayer(enemySettings.FollowRefreshRate));
        health = startingHealth;
    }

    private void Update()
    {
        OnAttackEnter();
    }

    private void OnAttackEnter()
    {
        if (Time.time > nextAttackTime)
        {
            float squareDistanceToTarget = (target.position - transform.position).sqrMagnitude;
            if (squareDistanceToTarget < Mathf.Pow(attackDistanceThreshold/2 + enemyCollisionRadius + playerCollisionRadius, 2))
            {
                nextAttackTime = Time.time + timeBetweenAttacks;
                StartCoroutine(Attack());
            }
        }
    }

    IEnumerator Attack()
    {
        currentState = State.Attacking;
        agent.enabled = false;
        Vector3 enemyPosition = transform.position;
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - directionToTarget * enemyCollisionRadius;
        float percent = 0;
        float attackSpeed = 3f;
        skinMaterial.color = Color.red;
        while (percent <= 1)
        {
            percent += Time.deltaTime * attackSpeed;
            float interpolation = 4 * (-Mathf.Pow(percent, 2) + percent); // -> 4 * (-x ^ 2 + x)
            transform.position = Vector3.Lerp(enemyPosition, attackPosition, interpolation);
            yield return null;
        }
        skinMaterial.color = originalColor;
        currentState = State.Chasing;
        agent.enabled = true;
    }

    private IEnumerator FollowPlayer(float refreshRate)
    {
        while (target != null)
        {
            if (currentState == State.Chasing)
            {
                Vector3 directionToTarget = (target.position - transform.position).normalized;
                agent.SetDestination(target.position - directionToTarget * (playerCollisionRadius + enemyCollisionRadius + attackDistanceThreshold / 3));
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }
}
