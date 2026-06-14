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
    // Update is called once per frame
    void Start()
    {
        StartCoroutine(ShootDelay());
    }

    IEnumerator ShootDelay()
    {
        // Beri delay awal agar musuh tidak langsung menembak begitu spawn
        yield return new WaitForSeconds(initialDelay);
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(shootInterval);
        }
    }
    public void Shoot()
    {
        ShootForm(firePoint);
        ShootForm(firePoint2);
    }

    void ShootForm(Transform point)
    {
        GameObject bulletObj = Instantiate(projectilePrefab, point.position, point.rotation);
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        bullet.transform.localScale = new Vector3(bulletSize, bulletSize, bulletSize);
        bullet.damage = 5;

        Vector3 shootDirection = point.forward;

        if (shootType == ShootType.Targeted)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                shootDirection = (player.transform.position - point.position).normalized;
            }
        }

        bullet.Init(shootDirection, "Enemy", bulletSpeed);
    }

}
