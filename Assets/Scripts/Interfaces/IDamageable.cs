using UnityEngine;

public interface IDamageable
{
    void TakeHit(float damage, RaycastHit hitInfo);
    void TakeDamage(float damage);
}
