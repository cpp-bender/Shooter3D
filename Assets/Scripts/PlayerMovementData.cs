using UnityEngine;

[CreateAssetMenu(menuName = "TopDown Shooter/Player Movement")]
public class PlayerMovementData : ScriptableObject
{
    [SerializeField] private float moveSpeed;

    public float MoveSpeed { get => moveSpeed; }
}
