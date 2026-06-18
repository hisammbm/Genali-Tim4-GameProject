using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum FormationType { Line, VShape, Random };

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private GameObject eliteEnemyPrefab;
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private float delayBetweenWaves = 3f;

    [Header("Setting Formasi")]
    [SerializeField] private Transform spawnCenter;
    [SerializeField] private int baseEnemyCount = 5;
    [SerializeField] private float spacing = 2f;

    [Header("Batas Player")]
    [SerializeField] private float minBoundX = -9.9f;
    [SerializeField] private float maxBoundX = 9.9f;

    [Header("Boss Settings")]
    [SerializeField] private GameObject hpBoss;
    [SerializeField] private Image bossHpBar;
    private EnemyHealth currentBossHealth;

    private List<GameObject> activeEnemies = new List<GameObject>();
    private bool isSpawning = false;
    private int waveCount = 0;

    void Start()
    {
        StartCoroutine(SpawnNextWave());
    }

    void Update()
    {
        if(currentBossHealth != null)
        {
            bossHpBar.fillAmount = (float)currentBossHealth.CurrentHealth / currentBossHealth.maxHealth;
        }
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

        if (waveCount % 5 == 0)
        {
            Debug.Log("BOSS FIGHT DETECTED!");
            SpawnBoss();
            hpBoss.SetActive(true);
        }
        else
        {
            hpBoss.SetActive(false);
            int cycle = (waveCount - 1) % 5 + 1; 
            GameObject prefabToSpawn = enemyPrefab;

            if ((cycle == 3 || cycle == 4) && eliteEnemyPrefab != null)
            {
                prefabToSpawn = eliteEnemyPrefab;
            }

            // Jumlah musuh bertambah sedikit seiring berjalannya loop wave
            int currentEnemyCount = baseEnemyCount + (waveCount / 5);

            FormationType randomFormation = (FormationType)Random.Range(0, 2);
            TriggerSpawn(randomFormation, prefabToSpawn, currentEnemyCount);
        }

        isSpawning = false;
    }

    void TriggerSpawn(FormationType formation, GameObject prefab, int count)
    {
        switch (formation)
        {
            case FormationType.Line:
                SpawnLine(prefab, count);
                break;
            case FormationType.VShape:
                SpawnVShape(prefab, count);
                break;
        }
    }

    float GetClampedX(float localX)
    {
        float worldX = spawnCenter.position.x + localX;
        worldX = Mathf.Clamp(worldX, minBoundX, maxBoundX);
        return worldX - spawnCenter.position.x;
    }

    void SpawnLine(GameObject prefab, int count)
    {
        float startX = 0f - ((count - 1) * spacing / 2f);

        for (int i = 0; i < count; i++)
        {
            float targetX = startX + (i * spacing);
            float clampedLocalX = GetClampedX(targetX);

            Vector3 targetPosition = new Vector3(clampedLocalX, 0f, 0);
            SpawnEnemy(targetPosition, prefab);
        }
    }

    void SpawnVShape(GameObject prefab, int count)
    {
        float startX = 0f - ((count - 1) * spacing / 2f);
        int middleIndex = count / 2;

        for (int i = 0; i < count; i++)
        {
            float targetX = startX + (i * spacing);

            float clampedLocalX = GetClampedX(targetX);
            float depthY = Mathf.Abs(i - middleIndex) * spacing;
            Vector3 targetPosition = new Vector3(clampedLocalX, depthY, 0);

            SpawnEnemy(targetPosition, prefab);
        }
    }

    void SpawnBoss()
    {
        if (bossPrefab == null)
        {
            Debug.LogWarning("Boss prefab is not assigned in EnemySpawner!");
            return;
        }

        Vector3 targetPos = new Vector3(0f, 3f, 0f); // Boss berada di tengah-atas layar
        GameObject newBoss = Instantiate(bossPrefab, Vector3.zero, bossPrefab.transform.rotation);
        currentBossHealth = newBoss.GetComponent<EnemyHealth>();


        newBoss.transform.SetParent(spawnCenter, false);

        Vector3 startPos = new Vector3(targetPos.x, targetPos.y + 10f, 0f);
        newBoss.transform.localPosition = startPos;

        activeEnemies.Add(newBoss);

        EnemyMovement movement = newBoss.GetComponent<EnemyMovement>();
        if (movement != null)
        {
            movement.SetTargetPosition(targetPos);
        }
    }

    void SpawnEnemy(Vector3 targetPos, GameObject prefab)
    {
        if (prefab == null) return;

        GameObject newEnemy = Instantiate(prefab, Vector3.zero, prefab.transform.rotation);
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