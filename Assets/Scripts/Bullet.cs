using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 100f;
    public int damage = 10;

    private Vector3 direction;
    private string ownerTag;
    

    public void Init(Vector3 direction, string owner)
    {
        this.direction = direction.normalized;
        ownerTag = owner;

        Destroy(gameObject, 3f);
    }

    void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ownerTag)) return;

        if(ownerTag == "Enemy" && other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().TakeDamage(damage);
            Destroy(gameObject);
        }

        if(ownerTag == "Player" && other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyHealth>().TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
