using UnityEngine;

public class ProjectTile : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private ProjectTileSettings projectTileSettings;
    [SerializeField] private LayerMask collisionMask;


    private void Update()
    {
        CheckCollision();
        Move();
    }

    private void CheckCollision()
    {
        float moveDistance = projectTileSettings.Speed * Time.deltaTime;
        RaycastHit hitInfo;
        Ray ray = new Ray(transform.position, Vector3.forward);
        if (Physics.Raycast(ray, out hitInfo, moveDistance, collisionMask))
        {
            OnHit(hitInfo);
        }
    }

    private void OnHit(RaycastHit hitInfo)
    {
        IDamageable damageable = hitInfo.collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeHit(projectTileSettings.Damage, hitInfo);
        }
        Destroy(gameObject);
    }

    private void Move()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * projectTileSettings.Speed);
    }
}
