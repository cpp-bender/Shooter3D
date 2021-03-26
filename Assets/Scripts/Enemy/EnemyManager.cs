using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyManager : Entity
{
    [Header("Enemy Settings")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform target;
    [SerializeField] private float startingHealth;

    //Scriptable Objects
    [SerializeField] private EnemySettings enemyData;
    [SerializeField] private PlayerSettings playerData;

    //Enemy States
    public enum State
    {
        Idle, Chasing, Attacking
    }

    //Enemy Attack Fields
    private State currentState = State.Idle;
    private float nextAttackTime;
    private float enemyCollisionRadius;
    private float playerCollisionRadius;

    private Color originalColor;
    private Entity player;
    private Material skinMaterial;

    private void Awake()
    {
        skinMaterial = GetComponent<Renderer>().material;
        originalColor = skinMaterial.color;
        enemyCollisionRadius = GetComponent<CapsuleCollider>().radius;
        health = startingHealth;
        if (FindObjectOfType<PlayerController>().transform != null)
        {
            target = FindObjectOfType<PlayerController>().transform;
            currentState = State.Chasing;
            player = target.GetComponent<Entity>();
            playerCollisionRadius = target.GetComponent<CapsuleCollider>().radius;
        }
    }

    private void Start()
    {
        StartCoroutine(Follow(enemyData.FollowRefreshRate));
    }

    private void Update()
    {
        TryToAttack();
    }

    private void TryToAttack()
    {
        if (!playerData.IsPlayerDead && Time.time > nextAttackTime)
        {
            float squareDistanceToTarget = (target.position - transform.position).sqrMagnitude;
            if (squareDistanceToTarget < Mathf.Pow(enemyData.AttackDistanceThreshold / 2 + enemyCollisionRadius + playerCollisionRadius, 2))
            {
                nextAttackTime = Time.time + enemyData.TimeBetweenAttacks;
                StartCoroutine(Attack());
            }
        }
    }

    private IEnumerator Attack()
    {
        currentState = State.Attacking;
        agent.enabled = false;
        Vector3 enemyPosition = transform.position;
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - directionToTarget * enemyCollisionRadius;
        float percent = 0;
        float attackSpeed = 3f;
        float damage = 1f;
        skinMaterial.color = Color.red;
        bool hasAppliedDamage = false;

        while (percent <= 1)
        {
            if (percent >= .5f && !hasAppliedDamage)
            {
                hasAppliedDamage = true;
                player.TakeDamage(damage);
            }
            percent += Time.deltaTime * attackSpeed;
            float interpolation = 4 * (-Mathf.Pow(percent, 2) + percent); // -> 4 * (-x ^ 2 + x)
            transform.position = Vector3.Lerp(enemyPosition, attackPosition, interpolation);
            yield return null;
        }
        skinMaterial.color = originalColor;
        currentState = State.Chasing;
        agent.enabled = true;
    }

    private IEnumerator Follow(float refreshRate)
    {
        while (target != null)
        {
            if (currentState == State.Chasing)
            {
                Vector3 directionToTarget = (target.position - transform.position).normalized;
                agent.SetDestination(target.position - directionToTarget * (playerCollisionRadius + enemyCollisionRadius + enemyData.AttackDistanceThreshold / 3));
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }
}