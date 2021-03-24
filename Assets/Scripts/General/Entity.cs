using System;
using UnityEngine;

public class Entity : MonoBehaviour, IDamageable
{
    protected float health;

    public event Action OnDeath;

    public void TakeHit(float damage, RaycastHit hitInfo)
    {
        health -= damage;
        if (health <= 0)
        {
            Die(hitInfo.collider.gameObject);
        }
    }

    private void Die(GameObject entityToDie)
    {
        OnDeath?.Invoke();    
        Destroy(entityToDie);
    }
}
