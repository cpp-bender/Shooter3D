using UnityEngine;

[CreateAssetMenu(menuName = "TopDown Shooter/Project Tile Settings")]
public class ProjectTileSettings : ScriptableObject
{
    [SerializeField] private float bulletSpeed;

    public float BulletSpeed { get => bulletSpeed; }
}