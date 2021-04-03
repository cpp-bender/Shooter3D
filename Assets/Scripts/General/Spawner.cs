using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Entity enemy;
    [SerializeField] private Entity player;
    [SerializeField] private MapGenerator map;
    [SerializeField] private Wave[] waves;

    //Current Wave Fields
    private Wave currentWave;
    private int currentWaveNumber;
    private int enemiesRemainingToSpawn;
    private int enemiesRemainingAlive;
    private float nextSpawnTime;

    private void Start()
    {
        InitializeWave();
        player.OnDeath += OnPlayerDeath;
    }

    private void Update()
    {
        SpawnWave();
    }

    private void SpawnWave()
    {
        if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
        {
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.TimeBetweenSpawns;
            StartCoroutine(OnEnemySpawn());
        }
    }

    private IEnumerator OnEnemySpawn()
    {
        System.Random rnd = new System.Random();
        float timer = 0f;
        float delay = timer + 1f;
        float bounceFactor = 6f;
        Transform spawnTile = map.GetRandomOpenTile();

        // 20% change to spawn enemy at player position.
        if (rnd.Next(0, 5) == 0)
        {
            spawnTile = map.GetTileFromPosition(player.transform.position);
        }
        Color baseColor = spawnTile.GetComponent<Renderer>().material.color;
        while (timer < delay)
        {
            spawnTile.GetComponent<Renderer>().material.color = Color.Lerp(baseColor, Color.red, Mathf.PingPong(timer * bounceFactor, 1));
            timer += Time.deltaTime;
            yield return null;
        }
        spawnTile.GetComponent<Renderer>().material.color = baseColor;
        var spawnedEnemy = Instantiate(enemy, spawnTile.position + Vector3.up, Quaternion.identity);
        spawnedEnemy.OnDeath += OnEnemyDeath;
    }

    private void OnEnemyDeath()
    {
        enemiesRemainingAlive--;
        InitializeWave();
    }

    private void OnPlayerDeath()
    {
        gameObject.SetActive(false);
    }

    private void InitializeWave()
    {
        if (currentWaveNumber < waves.Length && enemiesRemainingAlive == 0)
        {
            currentWave = waves[currentWaveNumber];
            enemiesRemainingToSpawn = currentWave.Count;
            enemiesRemainingAlive = enemiesRemainingToSpawn;
            currentWaveNumber++;
        }
    }
}

[System.Serializable]
public class Wave
{
    [SerializeField] private int count;
    [SerializeField] private float timeBetweenSpawns;

    public int Count { get => count; }
    public float TimeBetweenSpawns { get => timeBetweenSpawns; }
}