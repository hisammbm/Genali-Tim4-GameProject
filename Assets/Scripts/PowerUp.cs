using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private float duration = 5f;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController player =
            other.GetComponent<PlayerController>();

        if(player != null)
        {
            player.ActivateRapidFire(duration);

            Destroy(gameObject);
        }
    }
}