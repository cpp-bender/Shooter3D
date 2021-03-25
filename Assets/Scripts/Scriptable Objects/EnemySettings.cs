using UnityEngine;

[CreateAssetMenu(menuName = "TopDown Shooter/Enemy Settings")]
public class EnemySettings : ScriptableObject
{
    [SerializeField] private float followRefreshRate;
    [SerializeField] private float attackDistanceThreshold;
    [SerializeField] private float timeBetweenAttacks;

    public float FollowRefreshRate { get => followRefreshRate;}
    public float AttackDistanceThreshold { get => attackDistanceThreshold; set => attackDistanceThreshold = value; }
    public float TimeBetweenAttacks { get => timeBetweenAttacks; set => timeBetweenAttacks = value; }
}
