using UnityEngine;

public class ProjectTile : MonoBehaviour
{
    [SerializeField] private ProjectTileSettings projectTileSettings;
    [SerializeField] private LayerMask collisionMask;

    private PoolManager bulletPool;
    private EnemyManager enemy;
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
            OnHitEnter(hitInfo);
        }
    }

    private void OnHitEnter(RaycastHit hitInfo)
    {
        IDamageable damageable = hitInfo.collider.GetComponent<IDamageable>();
        if (damageable != null)
        {
            enemy = hitInfo.collider.gameObject.GetComponent<EnemyManager>();
            enemy.OnDeath += OnEnemyDeath;
            damageable.TakeHit(projectTileSettings.Damage, hitInfo);
        }
        bulletPool.ReturnToPool(gameObject);
    }

    private void OnEnemyDeath()
    {
        Destroy(enemy.gameObject);
    }

    private void Move()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * projectTileSettings.Speed);
    }
}
