using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShooting : MonoBehaviour
{
    [Header("Shooting")]
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
        while (true)
        {
            yield return new WaitForSeconds(1.5f);
            Shoot();
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
        bullet.Init(point.forward, "Enemy");
    }

}
