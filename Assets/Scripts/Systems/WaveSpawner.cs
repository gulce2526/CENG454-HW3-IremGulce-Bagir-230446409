using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public GameObject[] enemyPrefabs;
        public int enemyCount;
        public float spawnInterval = 1f;
    }

    [Header("Waves")]
    [SerializeField] private Wave[] waves;
    [SerializeField] private float timeBetweenWaves = 5f;

    [Header("Spawn Points")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform runeStone;

    private int currentWave = 0;
    private int enemiesAlive = 0;

    private void Start()
    {
        GameEvents.OnEnemyKilled += HandleEnemyKilled;
        StartCoroutine(StartNextWave());
    }

    private void OnDestroy()
    {
        GameEvents.OnEnemyKilled -= HandleEnemyKilled;
    }

    private void HandleEnemyKilled(int points)
    {
        enemiesAlive--;
        if (enemiesAlive <= 0 && currentWave < waves.Length)
        {
            GameEvents.WaveCompleted();
            StartCoroutine(StartNextWave());
        }
        else if (enemiesAlive <= 0 && currentWave >= waves.Length)
        {
            GameEvents.GameWon();
            Debug.Log("All waves complete! You win!");
        }
    }

    private IEnumerator StartNextWave()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        if (currentWave >= waves.Length) yield break;

        Wave wave = waves[currentWave];
        currentWave++;

        Debug.Log($"Starting {wave.waveName}!");
        yield return StartCoroutine(SpawnWave(wave));
    }

    private IEnumerator SpawnWave(Wave wave)
    {
        for (int i = 0; i < wave.enemyCount; i++)
        {
            SpawnEnemy(wave);
            enemiesAlive++;
            yield return new WaitForSeconds(wave.spawnInterval);
        }
    }

    private void SpawnEnemy(Wave wave)
    {
        if (spawnPoints.Length == 0) return;

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject prefab = wave.enemyPrefabs[Random.Range(0, wave.enemyPrefabs.Length)];

        GameObject enemyObj = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        Enemy enemy = enemyObj.GetComponent<Enemy>();

        if (enemy != null)
            enemy.SetTargets(player, runeStone);
    }
}