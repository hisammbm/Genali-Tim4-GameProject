using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private PowerUpDrop powerUpDrop;

    [SerializeField] private int maxHealth = 100;
    [SerializeField] private GameObject explodeParticle;
    [SerializeField] private int scoreValue = 10;

    private int _currentHealth;

    public int CurrentHealth
    {
        get => _currentHealth;
        set
        {
            _currentHealth = Mathf.Clamp(value, 0, maxHealth);
            OnHealthChanged();
        }
    }

    void Start()
    {
        CurrentHealth = maxHealth;

        powerUpDrop = GetComponent<PowerUpDrop>();
    }

    private void OnHealthChanged()
    {
        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        Debug.Log($"Current Health {gameObject.name}: {CurrentHealth}");
    }

    void Die()
    {
        TriggerExplode();

        if (powerUpDrop != null)
        {
            powerUpDrop.Drop();
        }

        GameManager.instance.AddScore(scoreValue);

        Destroy(gameObject);
    }

    void TriggerExplode()
    {
        if (gameObject == null) return;

        GameObject particle = Instantiate(
            explodeParticle,
            gameObject.transform.position,
            Quaternion.identity
        );

        Destroy(particle, 3f);
    }
}