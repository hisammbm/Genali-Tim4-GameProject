using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 100f;
    private int damage = 20;
    public Rigidbody rb;
    public Transform bullet;
    void Start()
    {
        Destroy(gameObject, 3f);
        rb = GetComponent<Rigidbody>();
        bullet = GetComponent<Transform>();
    }

    void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
