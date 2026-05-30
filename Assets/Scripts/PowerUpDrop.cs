using UnityEngine;

public class PowerUpDrop : MonoBehaviour
{
    [SerializeField] private GameObject powerUpPrefab;

    [Range(0, 100)]
    [SerializeField] private int dropChance = 30;

    public void Drop()
    {
        Debug.Log("DROP DIPANGGIL!");

        int random = Random.Range(0, 100);

        Debug.Log("Random = " + random);

        if (random < dropChance)
        {
            Debug.Log("POWERUP SPAWN!");

            Instantiate(
                powerUpPrefab,
                transform.position,
                Quaternion.identity
            );
        }
    }
}