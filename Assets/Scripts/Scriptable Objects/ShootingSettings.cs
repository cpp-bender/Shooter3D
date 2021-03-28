using UnityEngine;

[CreateAssetMenu(menuName = "TopDown Shooter/Shooting Settings")]
public class ShootingSettings : ScriptableObject
{
    [SerializeField] private float delay;

    public float Delay { get => delay; }
}
