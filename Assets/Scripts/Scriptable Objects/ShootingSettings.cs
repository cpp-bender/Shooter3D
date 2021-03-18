using UnityEngine;

[CreateAssetMenu(menuName = "TopDown Shooter/Shooting Settings")]
public class ShootingSettings : ScriptableObject
{
    [SerializeField] private float shootingDelay;

    public float ShootingDelay { get => shootingDelay; }
}
