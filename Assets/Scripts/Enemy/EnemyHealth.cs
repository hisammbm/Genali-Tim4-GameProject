using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private GameObject explodeParticle;
    private int _currentHealth;
    public int CurrentHealth { 
        get => _currentHealth;
        set
        {
            _currentHealth = Mathf.Clamp(value, 0, maxHealth);
            OnHealthChanged();
        }
    }

    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        CurrentHealth = maxHealth;   
    }

    private void OnHealthChanged()
    {
        if(_currentHealth <= 0)
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
        GameManager.instance.AddScore(10);
        Destroy(gameObject);
    }

    void TriggerExplode()
    {
        if (gameObject == null) return;

        GameObject particle =  Instantiate(explodeParticle, gameObject.transform.position, Quaternion.identity);
        audioManager.PlaySFX(audioManager.Explosion);

        Destroy(particle, 3f);
    }
}
