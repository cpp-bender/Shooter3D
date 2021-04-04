using UnityEngine;

public class Entity : MonoBehaviour, IDamageable
{
    protected float health;

    public event System.Action OnDeath;

    public  void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        OnDeath?.Invoke();
    }
}
