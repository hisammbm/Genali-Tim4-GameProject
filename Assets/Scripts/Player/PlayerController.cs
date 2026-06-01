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
    private AudioSource shootAudio;
    public GameObject projectilePrefab;
    public GameObject GameOverUI;

    [Header("Health")]
    private int maxHealth = 100;
    private int _currentHealth;
    public Image healthImg;
    public int CurrentHealth {
        get => _currentHealth;
        set
        {
            _currentHealth = Mathf.Clamp(value, 0, maxHealth);
            OnHealthChanged();
        } 
    }

    private void Awake()
    {
        shootAudio = GetComponent<AudioSource>();
        CurrentHealth = maxHealth;
        Time.timeScale = 1;
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
        if (Input.GetMouseButton(0))
        {
            ShootForm(firePoint);
            ShootForm(firePoint2);
            shootAudio.Play();
        }
    }

    void ShootForm(Transform point)
    {
        GameObject bulletObj = Instantiate(projectilePrefab, point.position, point.rotation);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.Init(point.forward, "Player");
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
        CurrentHealth -= dmg;
        Debug.Log("Current Health Player: " + CurrentHealth);
    }

    void Die()
    {
        Time.timeScale = 0;
        GameOverUI.SetActive(true);
    }
}
