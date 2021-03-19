using UnityEngine;

[CreateAssetMenu(menuName = "TopDown Shooter/Shooting Settings")]
public class ShootingSettings : ScriptableObject
{
    [Header("Shooting Settings")]
    [SerializeField] private float delay;

    public float Delay { get => delay; }
}
