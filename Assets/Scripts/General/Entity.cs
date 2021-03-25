using UnityEngine;

public class Entity : MonoBehaviour, IDamageable
{
    protected float health;

    public event System.Action OnDeath;

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    public void TakeHit(float damage, RaycastHit hitInfo)
    {
        //Will do some stuff later
        TakeDamage(damage);
    }

    private void Die()
    {
        OnDeath?.Invoke();    
    }
}
