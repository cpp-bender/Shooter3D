using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyManager : Entity
{
    [Header("Enemy Settings")]
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private ProjectTileSettings projectTileSettings;
    [SerializeField] private Transform target;
    [SerializeField] private float startingHealth;

    private void Start()
    {
        StartCoroutine(FollowPlayer(projectTileSettings.RefireRate));
        health = startingHealth;
    }

    private IEnumerator FollowPlayer(float refreshRate)
    {
        while (target != null)
        {
            agent.SetDestination(target.position);
            yield return new WaitForSeconds(projectTileSettings.RefireRate);
        }
    }
}
