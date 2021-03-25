using UnityEngine;

[CreateAssetMenu(menuName = "TopDown Shooter/Player Movement Settings")]
public class PlayerSettings : ScriptableObject
{
    [Header("Player Settings")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool isPlayerDead;

    public float MoveSpeed { get => moveSpeed; }
    public bool IsPlayerDead { get => isPlayerDead; set => isPlayerDead = value; }
}
