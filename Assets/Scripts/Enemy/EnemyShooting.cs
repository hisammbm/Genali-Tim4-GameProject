using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShootType { Straight, Targeted }

public class EnemyShooting : MonoBehaviour
{
    [Header("Shooting Settings")]
    public ShootType shootType = ShootType.Straight;
    public float bulletSpeed = 15f;
    public float shootInterval = 2.0f;
    public float initialDelay = 2.0f;

    [Header("Shooting Points")]
    public Transform firePoint;
    public Transform firePoint2;
    public GameObject projectilePrefab;
    float bulletSize = 0.4172799f;

    [Header("Boss Settings")]
    public bool isBoss = false;
    public int spreadCount = 5;
    public float spreadAngle = 45f;
    public float spiralInterval = 0.1f;
    public float spiralAngleStep = 15f;

    private EnemyHealth health;
    private bool isPhase2 = false;
    private float currentSpiralAngle = 0f;

    // Update is called once per frame
    void Start()
    {
        if (isBoss)
        {
            health = GetComponent<EnemyHealth>();
        }
        StartCoroutine(ShootDelay());
    }

    IEnumerator ShootDelay()
    {
        // Beri delay awal agar musuh tidak langsung menembak begitu spawn
        yield return new WaitForSeconds(initialDelay);
        while (true)
        {
            if (isBoss && health != null)
            {
                // Transisi ke fase 2 jika darah <= 50%
                if (!isPhase2 && health.CurrentHealth <= health.maxHealth / 2)
                {
                    isPhase2 = true;
                }

                if (isPhase2)
                {
                    ShootSpiral();
                    yield return new WaitForSeconds(spiralInterval);
                }
                else
                {
                    ShootSpread();
                    yield return new WaitForSeconds(shootInterval);
                }
            }
            else
            {
                // Musuh biasa
                Shoot();
                yield return new WaitForSeconds(shootInterval);
            }
        }
    }
    public void Shoot()
    {
        ShootForm(firePoint);
        if (firePoint2 != null) ShootForm(firePoint2);
    }

    void ShootForm(Transform point)
    {
        Vector3 shootDirection = point.forward;

        if (shootType == ShootType.Targeted)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                shootDirection = (player.transform.position - point.position).normalized;
            }
        }

        FireBullet(point, shootDirection);
    }

    void FireBullet(Transform point, Vector3 direction)
    {
        if (point == null || projectilePrefab == null) return;

        GameObject bulletObj = Instantiate(projectilePrefab, point.position, point.rotation);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.transform.localScale = new Vector3(bulletSize, bulletSize, bulletSize);
            bullet.damage = 5;
            bullet.Init(direction.normalized, "Enemy", bulletSpeed);
        }
    }

    void ShootSpread()
    {
        Vector3 baseDirection1 = firePoint.forward;
        Vector3 baseDirection2 = firePoint2 != null ? firePoint2.forward : baseDirection1;

        if (shootType == ShootType.Targeted)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                baseDirection1 = (player.transform.position - firePoint.position).normalized;
                if (firePoint2 != null)
                    baseDirection2 = (player.transform.position - firePoint2.position).normalized;
            }
        }

        float startAngle = -spreadAngle / 2f;
        float angleStep = spreadCount > 1 ? spreadAngle / (spreadCount - 1) : 0;

        for (int i = 0; i < spreadCount; i++)
        {
            float angle = startAngle + (i * angleStep);
            
            if (firePoint != null)
            {
                Vector3 rotatedDir1 = Quaternion.Euler(0, 0, angle) * baseDirection1;
                FireBullet(firePoint, rotatedDir1);
            }

            if (firePoint2 != null)
            {
                Vector3 rotatedDir2 = Quaternion.Euler(0, 0, angle) * baseDirection2;
                FireBullet(firePoint2, rotatedDir2);
            }
        }
    }

    void ShootSpiral()
    {
        if (firePoint != null)
        {
            Vector3 dir1 = Quaternion.Euler(0, 0, currentSpiralAngle) * firePoint.forward;
            FireBullet(firePoint, dir1);
        }

        if (firePoint2 != null)
        {
            // Reverse spiral untuk firePoint2 biar makin keren
            Vector3 dir2 = Quaternion.Euler(0, 0, -currentSpiralAngle) * firePoint2.forward;
            FireBullet(firePoint2, dir2);
        }

        currentSpiralAngle += spiralAngleStep;
        if (currentSpiralAngle >= 360f) currentSpiralAngle -= 360f;
    }

}
