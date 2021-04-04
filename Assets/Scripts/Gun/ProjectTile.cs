using UnityEngine;

public class ProjectTile : MonoBehaviour
{
    [SerializeField] private ProjectTileSettings projectTileSettings;
    [SerializeField] private LayerMask collisionMask;

    private PoolManager bulletPool;
    private EnemyManager enemy;
    private Vector3 hitPoint;
    private float lifeTime;

    private void Awake()
    {
        bulletPool = new PoolManager();
    }

    private void OnEnable()
    {
        lifeTime = Time.time + projectTileSettings.MaxLifeTime;
    }

    private void Update()
    {
        CheckCollision();
        Move();
        Destroy();
    }

    private void Move()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * projectTileSettings.Speed);
    }

    private void Destroy()
    {
        if (Time.time > lifeTime)
        {
            lifeTime = Time.time + projectTileSettings.MaxLifeTime;
            bulletPool.ReturnToPool(gameObject);
        }
    }

    private void CheckCollision()
    {
        float moveDistance = projectTileSettings.Speed * Time.deltaTime;
        float moveDistanceOffset = .2f;
        RaycastHit hitInfo;
        Ray ray = new Ray(transform.position, transform.forward);
        if (Physics.Raycast(ray, out hitInfo, moveDistance + moveDistanceOffset, collisionMask))
        {
            OnHitEnter(hitInfo.collider);
        }
    }

    private void OnHitEnter(Collider hitCollider)
    {
        IDamageable damageable = hitCollider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            enemy = hitCollider.gameObject.GetComponent<EnemyManager>();
            hitPoint = hitCollider.gameObject.transform.position;
            enemy.OnDeath += OnEnemyDeath;
            damageable.TakeDamage(projectTileSettings.Damage);
        }
        bulletPool.ReturnToPool(gameObject);
    }

    private void OnEnemyDeath()
    {
        enemy.PlayDeathEffect(hitPoint, transform.forward);
        Destroy(enemy.gameObject);
    }
}
