using UnityEngine;

public class Entity : MonoBehaviour, IDamageable
{
    protected float health;

    public void TakeHit(float damage, RaycastHit hitInfo)
    {
        Debug.Log($"{hitInfo.collider.gameObject.name} is hit");
        health -= damage;
        if (health <= 0)
        {
            //TODO: Use a pooling system here
            Destroy(hitInfo.collider.gameObject);
        }
    }
}
