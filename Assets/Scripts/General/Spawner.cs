using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private EnemyManager enemy;
    [SerializeField] private Wave[] waves;

    Wave currentWave;
    int currentWaveNumber;

    int enemiesRemainingToSpawn;
    int enemiesRemainingAlive;
    float nextSpawnTime;

    private void Start()
    {
        InitializeNextWave();
    }

    private void Update()
    {
        SpawnWave();
    }

    private void SpawnWave()
    {
        if (enemiesRemainingToSpawn > 0 && Time.time > nextSpawnTime)
        {
            var spawnedEnemy = Instantiate(enemy, Vector3.zero, Quaternion.identity);
            spawnedEnemy.OnDeath += OnEnemyDeath;
            enemiesRemainingToSpawn--;
            nextSpawnTime = Time.time + currentWave.TimeBetweenSpawns;
        }
    }

    private void OnEnemyDeath()
    {
        enemiesRemainingAlive--;
        InitializeNextWave();
    }

    private void InitializeNextWave()
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