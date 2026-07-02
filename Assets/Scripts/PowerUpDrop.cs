using System.Collections.Generic;
using UnityEngine;

public class PowerUpDrop : MonoBehaviour
{
    [System.Serializable]
    public struct PowerUpItem
    {
        public GameObject prefab;
        [Tooltip("Bobot peluang drop. Semakin besar bobot, semakin sering muncul relatif terhadap item lain.")]
        public float weight;
    }

    [SerializeField] private List<PowerUpItem> powerUpPrefabs;

    [Range(0, 100)]
    [SerializeField] private int dropChance = 30;

    public void Drop()
    {
        Debug.Log("DROP DIPANGGIL!");

        int random = Random.Range(0, 100);

        Debug.Log("Random = " + random);

        if (random < dropChance && powerUpPrefabs != null && powerUpPrefabs.Count > 0)
        {
            // Hitung total weight
            float totalWeight = 0f;
            foreach (var item in powerUpPrefabs)
            {
                if (item.prefab != null)
                {
                    totalWeight += item.weight;
                }
            }

            if (totalWeight <= 0f) return;

            // Roll nilai acak berdasarkan total weight
            float roll = Random.Range(0f, totalWeight);
            float cumulativeWeight = 0f;
            GameObject chosenPowerUp = null;

            foreach (var item in powerUpPrefabs)
            {
                if (item.prefab != null)
                {
                    cumulativeWeight += item.weight;
                    if (roll <= cumulativeWeight)
                    {
                        chosenPowerUp = item.prefab;
                        break;
                    }
                }
            }

            if (chosenPowerUp != null)
            {
                Debug.Log("POWERUP SPAWN: " + chosenPowerUp.name);
                Instantiate(
                    chosenPowerUp,
                    transform.position,
                    Quaternion.identity
                );
            }
        }
    }
}