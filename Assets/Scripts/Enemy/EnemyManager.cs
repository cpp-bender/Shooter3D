using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyManager : Entity
{
    public enum State
    {
        Idle, Chasing, Attacking
    }

    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform target;
    [SerializeField] private EnemySettings enemyData;
    [SerializeField] private PlayerSettings playerData;
    [SerializeField] private ParticleSystem deathEffect;

    private State currentState = State.Idle;
    private float nextAttackTime;
    private float enemyCollisionRadius;
    private float playerCollisionRadius;
    private Material skinMaterial;

    private void OnEnable()
    {
        skinMaterial = GetComponent<Renderer>().material;
        enemyData.SkinColor = skinMaterial.color;
        enemyCollisionRadius = GetComponent<CapsuleCollider>().radius;
        health = enemyData.StartingHealth;
    }

    private void Awake()
    {
        if (FindObjectOfType<PlayerController>().transform != null)
        {
            target = FindObjectOfType<PlayerController>().transform;
            playerData.SkinColor = target.GetComponent<Renderer>().material.color;
            currentState = State.Chasing;
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

    public void PlayDeathEffect(Vector3 hitPoint, Vector3 hitDirection)
    {
        Instantiate(deathEffect, hitPoint, Quaternion.FromToRotation(Vector3.forward, hitDirection));
    }

    private void TryToAttack()
    {
        if (!playerData.IsPlayerDead && Time.time > nextAttackTime)
        {
            float squareDistanceToTarget = (target.position - transform.position).sqrMagnitude;
            float enemyRange = Mathf.Pow(enemyData.AttackDistanceThreshold / 2 + enemyCollisionRadius + playerCollisionRadius, 2);
            if (squareDistanceToTarget < enemyRange)
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
        skinMaterial.color = Color.red;
        bool hasAppliedDamage = false;

        while (percent <= 1)
        {
            if (percent >= .5f && !hasAppliedDamage)
            {
                hasAppliedDamage = true;
                target.GetComponent<PlayerController>().TakeDamage(enemyData.Damage);
                if (!playerData.IsPlayerDead)
                {
                    StartCoroutine(BlinkPlayer());
                }
            }
            percent += Time.deltaTime * enemyData.AttackSpeed;
            float interpolation = 4 * (-Mathf.Pow(percent, 2) + percent); // -> 4 * (-x ^ 2 + x)
            transform.position = Vector3.Lerp(enemyPosition, attackPosition, interpolation);
            yield return null;
        }
        skinMaterial.color = enemyData.SkinColor;
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

    private IEnumerator BlinkPlayer()
    {
        for (int i = 0; i < 2; i++)
        {
            float x = 0;
            float interpolation;
            while (x <= 1)
            {
                x += Time.deltaTime * 10;
                interpolation = 4 * (-Mathf.Pow(x, 2) + x); // -> 4 * (-x ^ 2 + x)
                target.GetComponent<Renderer>().material.color = Color.Lerp(playerData.SkinColor, Color.red, interpolation);
                yield return null;
            }
            yield return new WaitForSeconds(.2f);
        }
    }
}