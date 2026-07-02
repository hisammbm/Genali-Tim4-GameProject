using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public enum PowerUpType
    {
        RapidFire,
        HealthRestore,
        Shield
    }

    [SerializeField] private PowerUpType type = PowerUpType.RapidFire;
    [SerializeField] private Sprite buffIcon; // Ikon untuk buff UI
    [SerializeField] private float duration = 5f;
    [SerializeField] private int healAmount = 20;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Debug.Log("POWERUP DIAMBIL: " + type);

        PlayerController player = other.GetComponent<PlayerController>();

        if (player != null)
        {
            switch (type)
            {
                case PowerUpType.RapidFire:
                    player.ActivateRapidFire(duration, buffIcon);
                    break;
                case PowerUpType.HealthRestore:
                    player.StartHealthRegen(healAmount, duration, buffIcon);
                    break;
                case PowerUpType.Shield:
                    player.ActivateShield(duration, buffIcon);
                    break;
            }
            Destroy(gameObject);
        }
    }
}