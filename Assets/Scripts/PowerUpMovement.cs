using UnityEngine;

public class PowerUpMovement : MonoBehaviour
{
    public float speed = 20f;

    void Update()
    {
        transform.position += Vector3.back * speed * Time.deltaTime;
    }
}