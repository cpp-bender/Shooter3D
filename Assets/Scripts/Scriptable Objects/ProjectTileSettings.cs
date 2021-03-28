using UnityEngine;

[CreateAssetMenu(menuName = "TopDown Shooter/Project Tile Settings")]
public class ProjectTileSettings : ScriptableObject
{
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    [SerializeField] private float refireRate;
    [SerializeField] private float maxLifeTime;

    public float Speed { get => speed; }
    public float Damage { get => damage; }
    public float RefireRate { get => refireRate; }
    public float MaxLifeTime { get => maxLifeTime; }
}