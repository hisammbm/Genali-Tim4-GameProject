using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float duration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Debug.Log("POWERUP DIAMBIL");

        PlayerController player =
            other.GetComponent<PlayerController>();

        if (player != null)
        {
            player.ActivateRapidFire(duration);
            Destroy(gameObject);
        }
    }
}