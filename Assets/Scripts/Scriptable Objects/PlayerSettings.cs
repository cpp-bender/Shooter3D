using UnityEngine;

[CreateAssetMenu(menuName = "TopDown Shooter/Player Movement Settings")]
public class PlayerSettings : ScriptableObject
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private bool isPlayerDead;
    [SerializeField] private Color skinColor;
    [SerializeField] private float startingHealth;

    public float MoveSpeed { get => moveSpeed; }
    public bool IsPlayerDead { get => isPlayerDead; set => isPlayerDead = value; }
    public Color SkinColor { get => skinColor; set => skinColor = value; }
    public float StartingHealth { get => startingHealth; set => startingHealth = value; }
}
