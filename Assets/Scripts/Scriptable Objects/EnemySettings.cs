using UnityEngine;

[CreateAssetMenu(menuName = "TopDown Shooter/Enemy Settings")]
public class EnemySettings : ScriptableObject
{
    [SerializeField] private float followRefreshRate;

    public float FollowRefreshRate { get => followRefreshRate;}
}
