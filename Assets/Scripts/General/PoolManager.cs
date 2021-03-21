using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    private List<GameObject> bullets = new List<GameObject>();

    public List<GameObject> Bullets { get => bullets; set => bullets = value; }

    public void AddToPool(GameObject projectTile)
    {
        bullets.Add(projectTile);
    }

    public GameObject GetFromPool()
    {
        GameObject bullet = null;
        for (int i = 0; i < bullets.Count; i++)
        {
            if (!bullets[i].activeSelf)
            {
                bullet = bullets[i];
                break;
            }
        }
        return bullet;
    }

    public void ReturnToPool(GameObject bullet)
    {
        bullet.SetActive(false);
    }
}