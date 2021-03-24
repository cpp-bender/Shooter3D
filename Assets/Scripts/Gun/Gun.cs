using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("Gun Settings")]
    [SerializeField] private Transform muzzle;
    [SerializeField] private ProjectTile projectTile;
    [SerializeField] private ShootingSettings shootingSettings;

    private PoolManager poolManager;
    private float nextShootTime = 0f;

    private void Awake()
    {
        poolManager = new PoolManager();
    }

    public void Shoot()
    {
        if (Time.time > nextShootTime)
        {
            //Pooling System
            if (poolManager.Bullets.Count == 0)
            {
                poolManager.AddToPool(InstantiateBullet());
            }
            MoveBullet(poolManager.GetFromPool());
            nextShootTime = Time.time + shootingSettings.Delay;
        }
    }

    private GameObject InstantiateBullet()
    {
        var bullet = Instantiate(projectTile, muzzle.position, muzzle.rotation).gameObject;
        bullet.SetActive(false);
        return bullet;
    }

    private void MoveBullet(GameObject bullet)
    {
        if (bullet == null)
        {
            var projectTile = InstantiateBullet();
            poolManager.AddToPool(projectTile.gameObject);
            projectTile.transform.position = muzzle.position;
            projectTile.transform.rotation = muzzle.rotation;
            projectTile.SetActive(true);
            return;
        }
        bullet.transform.position = muzzle.position;
        bullet.transform.rotation = muzzle.rotation;
        bullet.SetActive(true);
    }
}