using UnityEngine;

public class EnemiesSpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs; // Array of enemy prefabs
    public float spawnInterval = 2f;
    public float spawnRadius = 5f;
    public int spawnsPerWave = 10; // Number of spawns per wave

    private Camera mainCamera;
    private float timer = 0f;
    private int remainingSpawns;
    private int currentWave = 1;

    public GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        mainCamera = Camera.main;
        remainingSpawns = spawnsPerWave;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (remainingSpawns > 0)
        {
            if (timer >= spawnInterval)
            {
                SpawnEnemy();
                timer = 0f;
                remainingSpawns--;

                if (remainingSpawns <= 0)
                {
                    StartNewWave();
                }
            }
        }
    }

    void SpawnEnemy()
    {
        int randomEnemyIndex = Random.Range(0, enemyPrefabs.Length); // Randomly choose an enemy prefab
        Vector3 spawnPosition = GetRandomSpawnPosition();
        Instantiate(enemyPrefabs[randomEnemyIndex], spawnPosition, Quaternion.identity);
    }

    Vector3 GetRandomSpawnPosition()
    {
        float cameraHeight = mainCamera.orthographicSize;
        float cameraWidth = cameraHeight * mainCamera.aspect;

        Vector3 cameraPosition = mainCamera.transform.position;

        float randomValue = Random.value;
        float spawnX = cameraPosition.x + (randomValue > 0.5f ? 1 : -1) * (cameraWidth + spawnRadius);
        float spawnY = cameraPosition.y + (randomValue > 0.5f ? 1 : -1) * (cameraHeight + spawnRadius);

        return new Vector3(spawnX, spawnY, 0f);
    }

    void StartNewWave()
    {
        Debug.Log("Wave " + currentWave + " completed!");

        // Increment the wave
        currentWave++;

        // Reset spawns for the new wave
        remainingSpawns = spawnsPerWave;

        Debug.Log("Wave " + currentWave + " started!");
    }

    void StopSpawning()
    {
        if (gameManager != null)
        {
            gameManager.StopSpawning();
        }

    }
}
