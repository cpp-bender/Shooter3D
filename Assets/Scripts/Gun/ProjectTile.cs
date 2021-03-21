using UnityEngine;

public class ProjectTile : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private ProjectTileSettings projectTileSettings;
    [SerializeField] private LayerMask collisionMask;

    private PoolManager pool;
    private float lifeTime;

    private void OnEnable()
    {
        lifeTime = 0f;
    }

    private void Awake()
    {
        pool = new PoolManager();
    }

    private void Update()
    {
        IncrementPassedTime();
        Destroy();
        CheckCollision();
        Move();
    }

    private void Destroy()
    {
        if (lifeTime >= projectTileSettings.MaxLifeTime)
        {
            pool.ReturnToPool(gameObject);
        }
    }

    private void IncrementPassedTime()
    {
        lifeTime += Time.deltaTime;
    }

    private void CheckCollision()
    {
        float moveDistance = projectTileSettings.Speed * Time.deltaTime;
        RaycastHit hitInfo;
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out hitInfo, moveDistance, collisionMask))
        {
            OnHitEnter(hitInfo);
        }
    }

    private void OnHitEnter(RaycastHit hitInfo)
    {
        IDamageable damageable = hitInfo.collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeHit(projectTileSettings.Damage, hitInfo);
        }
        pool.ReturnToPool(gameObject);
    }

    private void Move()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * projectTileSettings.Speed);
    }
}
