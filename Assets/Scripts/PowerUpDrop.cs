using UnityEngine;

public class PowerUpDrop : MonoBehaviour
{
    [SerializeField] private GameObject powerUpPrefab;

    [Range(0, 100)]
    [SerializeField] private int dropChance = 30;

    public void Drop()
    {
        int random = Random.Range(0, 100);

        if (random < dropChance)
        {
            Instantiate(
                powerUpPrefab,
                transform.position,
                Quaternion.identity
            );
        }
    }
}