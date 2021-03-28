using UnityEngine;

[CreateAssetMenu(menuName = "TopDown Shooter/Enemy Settings")]
public class EnemySettings : ScriptableObject
{
    [SerializeField] private float followRefreshRate;
    [SerializeField] private float attackDistanceThreshold;
    [SerializeField] private float timeBetweenAttacks;
    [SerializeField] private float startingHealth;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float damage;
    [SerializeField] private Color skinColor;

    public float FollowRefreshRate { get => followRefreshRate;}
    public float AttackDistanceThreshold { get => attackDistanceThreshold; set => attackDistanceThreshold = value; }
    public float TimeBetweenAttacks { get => timeBetweenAttacks; set => timeBetweenAttacks = value; }
    public float StartingHealth { get => startingHealth; set => startingHealth = value; }
    public float AttackSpeed { get => attackSpeed; set => attackSpeed = value; }
    public float Damage { get => damage; set => damage = value; }
    public Color SkinColor { get => skinColor; set => skinColor = value; }
}
