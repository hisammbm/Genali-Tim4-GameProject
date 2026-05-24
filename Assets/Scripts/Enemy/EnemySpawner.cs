using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FormationType { Line, VShape, Random };

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float delayBetweenWaves = 3f;

    [Header("Setting Formasi")]
    [SerializeField] private Transform spawnCenter;
    [SerializeField] private int enemyCount = 5;
    [SerializeField] private float spacing = 2f;

    [Header("Batas Player")]
    [SerializeField] private float minBoundX = -9.9f;
    [SerializeField] private float maxBoundX = 9.9f;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool isSpawning = false;
    private int waveCount = 0;

    void Start()
    {
        StartCoroutine(SpawnNextWave());
    }

    void Update()
    {
        if (isSpawning) return;

        activeEnemies.RemoveAll(x => x  == null || !x.activeInHierarchy);

        if (activeEnemies.Count == 0)
        {
            StartCoroutine(SpawnNextWave());
        }
    }

    IEnumerator SpawnNextWave()
    {
        isSpawning = true;
        waveCount++;
        Debug.Log($"Wave: {waveCount}");

        yield return new WaitForSeconds(delayBetweenWaves);

        FormationType randomFormation = (FormationType)Random.Range(0, 3);

        TriggerSpawn(randomFormation);

        isSpawning = false;
    }

    void TriggerSpawn(FormationType formation)
    {
        switch (formation)
        {
            case FormationType.Line:
                SpawnLine();
                break;
            case FormationType.VShape:
                SpawnVShape();
                break;
            case FormationType.Random:
                SpawnRandom();
                break;
        }
    }
    float GetClampedX(float localX)
    {
        float worldX = spawnCenter.position.x + localX;
        worldX = Mathf.Clamp(worldX, minBoundX, maxBoundX);
        return worldX - spawnCenter.position.x;
    }


    void SpawnLine()
    {
        float startX = 0f - ((enemyCount - 1) * spacing / 2f);

        for (int i = 0; i < enemyCount; i++)
        {
            float targetX = startX + (i * spacing);
            float clampedLocalX = GetClampedX(targetX);

            Vector3 targetPosition = new Vector3(clampedLocalX, 0f, 0);
            SpawnEnemy(targetPosition);
        }
    }

    void SpawnVShape()
    {
        float startX = 0f - ((enemyCount - 1) * spacing / 2f);
        int middleIndex = enemyCount / 2;

        for (int i = 0; i < enemyCount; i++)
        {
            float targetX = startX + (i * spacing);

            float clampedLocalX = GetClampedX(targetX);
            float depthY = Mathf.Abs(i - middleIndex) * spacing;
            Vector3 targetPosition = new Vector3(clampedLocalX, depthY, 0);

            SpawnEnemy(targetPosition);
        }
    }

    void SpawnRandom()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            float randomWorldX = Random.Range(minBoundX, maxBoundX);
            float localX = randomWorldX - spawnCenter.position.x;

            Vector3 randomPos = new Vector3(localX, Random.Range(-2f, 2f), 0);
            SpawnEnemy(randomPos);
        }
    }

    void SpawnEnemy(Vector3 targetPos)
    {
        GameObject newEnemy = Instantiate(enemyPrefab, Vector3.zero, enemyPrefab.transform.rotation);

        newEnemy.transform.SetParent(spawnCenter, false);

        Vector3 startShootFromSky = new Vector3(targetPos.x, targetPos.y + 10f, 0f);
        newEnemy.transform.localPosition = startShootFromSky;

        activeEnemies.Add(newEnemy);

        EnemyMovement movement = newEnemy.GetComponent<EnemyMovement>();
        if (movement != null)
        {
            movement.SetTargetPosition(targetPos);
        }
    }
}