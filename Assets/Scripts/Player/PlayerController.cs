using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    private float moveX, moveY;
    private Quaternion angle;
    [SerializeField] private float movementSpeed = 5f, smoothTurn;
    [SerializeField] private float minX = -9.9f, maxX = 9.9f;
    [SerializeField] private float minY = -3f, maxY = 9.7f;

    [Header("Shooting")]
    public Transform firePoint;
    public Transform firePoint2;
    public GameObject projectilePrefab;
    [SerializeField] private float normalFireRate = 0.4f;
    [SerializeField] private float rapidFireRate = 0.15f;
    private float nextFireTime = 0f;
    private bool rapidFireActive = false;

    [Header("Health")]
    [SerializeField]private int maxHealth = 100;
    private int _currentHealth;
    public Image healthImg;
    public GameObject GameOverUI;

    [Header("Shield")]
    [SerializeField] private GameObject shieldVisual;
    private bool shieldActive = false;
    private Coroutine shieldCoroutine;

    private Coroutine rapidFireCoroutine;
    private Coroutine healthRegenCoroutine;
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
        CurrentHealth = maxHealth;
        Time.timeScale = 1;

        if (shieldVisual != null) shieldVisual.SetActive(false);
    }

    void Update()
    {
        moveX = Input.GetAxis("Horizontal");
        moveY = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveX, moveY, 0) * movementSpeed * Time.deltaTime;
        transform.position += movement;

        angle.x = Mathf.Lerp(angle.x, moveX * movementSpeed, smoothTurn * Time.deltaTime);
        angle.y = Mathf.Lerp(angle.y, moveY * movementSpeed, smoothTurn * Time.deltaTime);

        angle.x = Mathf.Clamp(angle.x, -55, 55);
        angle.y = Mathf.Clamp(angle.y, -25, 25);

        transform.rotation = Quaternion.Euler(-angle.y, 0, -angle.x);
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minX, maxX),
            Mathf.Clamp(transform.position.y, minY, maxY),
            transform.position.z
            );

        Shoot();
    }

    public void Shoot()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {
            float currentFireRate = rapidFireActive ? rapidFireRate : normalFireRate;
            nextFireTime = Time.time + currentFireRate;

            ShootForm(firePoint);
            ShootForm(firePoint2);
            audioManager.PlaySFX(audioManager.Shoot);
        }
    }

   void ShootForm(Transform point)
{
    GameObject bulletObj = Instantiate(projectilePrefab, point.position, point.rotation);
    Bullet bullet = bulletObj.GetComponent<Bullet>();
    bullet.Init(point.forward, "Player");
}

    public void ActivateRapidFire(float duration, Sprite iconSprite)
    {
        if (rapidFireCoroutine != null)
        {
            StopCoroutine(rapidFireCoroutine);
        }
        rapidFireCoroutine = StartCoroutine(RapidFireRoutine(duration));

        if (BuffUIManager.Instance != null && iconSprite != null)
        {
            BuffUIManager.Instance.TriggerBuff("RapidFire", iconSprite, duration);
        }
    }

    IEnumerator RapidFireRoutine(float duration)
    {
        rapidFireActive = true;
        Debug.Log("Rapid Fire ON");

        yield return new WaitForSeconds(duration);

        rapidFireActive = false;
        Debug.Log("Rapid Fire OFF");
    }
    void OnHealthChanged()
    {
        healthImg.fillAmount = (float)_currentHealth / maxHealth;
        if(_currentHealth <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(int dmg)
    {
        if (shieldActive)
        {
            Debug.Log("DAMAGE DIABSORB OLEH SHIELD!");
            return;
        }

        CurrentHealth -= dmg;
        Debug.Log("Current Health Player: " + CurrentHealth);
    }

    public void Heal(int amount)
    {
        CurrentHealth += amount;
        Debug.Log("Player Healed: " + amount + ". Current Health: " + CurrentHealth);
    }

    public void StartHealthRegen(int totalAmount, float duration, Sprite iconSprite)
    {
        if (healthRegenCoroutine != null)
        {
            StopCoroutine(healthRegenCoroutine);
        }
        healthRegenCoroutine = StartCoroutine(HealthRegenRoutine(totalAmount, duration));

        if (BuffUIManager.Instance != null && iconSprite != null)
        {
            BuffUIManager.Instance.TriggerBuff("HealthRegen", iconSprite, duration);
        }
    }

    private IEnumerator HealthRegenRoutine(int totalAmount, float duration)
    {
        float elapsed = 0f;
        float tickInterval = 0.5f; // Setiap 0.5 detik darah bertambah
        int ticks = Mathf.CeilToInt(duration / tickInterval);
        if (ticks <= 0) ticks = 1;
        
        float amountPerTick = (float)totalAmount / ticks;
        float accumulatedHeal = 0f;
        int totalHealed = 0;

        for (int i = 0; i < ticks; i++)
        {
            yield return new WaitForSeconds(tickInterval);
            
            accumulatedHeal += amountPerTick;
            int healThisTick = Mathf.FloorToInt(accumulatedHeal);
            if (healThisTick > 0)
            {
                CurrentHealth += healThisTick;
                totalHealed += healThisTick;
                accumulatedHeal -= healThisTick;
            }
            
            Debug.Log($"Health Regen: +{healThisTick} HP. Current Health: {CurrentHealth}");
        }

        int remaining = totalAmount - totalHealed;
        if (remaining > 0)
        {
            CurrentHealth += remaining;
            Debug.Log($"Health Regen Remaining: +{remaining} HP. Current Health: {CurrentHealth}");
        }
    }

    public void ActivateShield(float duration, Sprite iconSprite)
    {
        if (shieldCoroutine != null)
        {
            StopCoroutine(shieldCoroutine);
        }
        shieldCoroutine = StartCoroutine(ShieldRoutine(duration));

        if (BuffUIManager.Instance != null && iconSprite != null)
        {
            BuffUIManager.Instance.TriggerBuff("Shield", iconSprite, duration);
        }
    }

    private IEnumerator ShieldRoutine(float duration)
    {
        shieldActive = true;
        if (shieldVisual != null) shieldVisual.SetActive(true);

        yield return new WaitForSeconds(duration);

        shieldActive = false;
        if (shieldVisual != null) shieldVisual.SetActive(false);
    }

    void Die()
    {
        Time.timeScale = 0;
        GameOverUI.SetActive(true);
    }
}
