using UnityEngine;

[CreateAssetMenu(menuName = "TopDown Shooter/Player Movement Settings")]
public class PlayerMovementSettings : ScriptableObject
{
    [Header("Player Movement Settings")]
    [SerializeField] private float moveSpeed;

    public float MoveSpeed { get => moveSpeed; }
}
